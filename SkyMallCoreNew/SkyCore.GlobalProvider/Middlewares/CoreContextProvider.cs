using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkyCoreLib.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkyCore.GlobalProvider
{

    /// <summary>
    /// Core 全局支持上下文
    /// 配置、Service、HttpContext、用户
    /// （可以改为注入的方式，这里为了避免传参麻烦）
    /// MiddleWare中间件都可以对管道中的请求进行拦截，它可以决定是否将请求转移给下一个中间件
    /// </summary>
    public class CoreContextProvider
    {
        //private string LoginUserKey = ConstParameters.SysLoginUserKey;
        //private string LoginProvider = ConstParameters.SysLoginProvider;

        private static IHttpContextAccessor _accessor;
        private readonly RequestDelegate _next;
        private static ConcurrentDictionary<string, object> _serviceDictionary;

        public static int PageSize = 20;

        public static IConfiguration Configuration { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }
        public static IHostingEnvironment HostingEnvironment { get; set; }
        public static HttpContext HttpContext => _accessor.HttpContext;

        
        public CoreContextProvider(RequestDelegate next, IHttpContextAccessor accessor, 
            IHostingEnvironment hostingEnvironment,IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _next = next;
             _accessor = accessor;
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;

            //https://www.cnblogs.com/artech/p/net-core-di-06.html
            //在利用ServiceCollection创建出代表DI容器的IServiceProvider对象之后，我们调用其CreateScope方法创建了两个所谓的“服务范围”，
            //后者的ServiceProvider属性返回一个新的IServiceProvider对象，
            //它实际上是当前IServiceProvider对象的子容器。我们最后利用作为子容器的IServiceProvider对象来提供相应的服务实例。
            //即ServiceProvider是服务集合子容器
            ServiceProvider = serviceProvider.CreateScope().ServiceProvider;

            FileHelper.AppRootPath = HostingEnvironment.WebRootPath;
        }


        //只有Singleton服务可以通过中间件中的构造函数注入来解决
        //services.Singleton<IEmailRepository, EmailRepository>();
        //例如：public async Task Invoke(HttpContext context, IEmailRepository emailRepository)
        public async Task Invoke(HttpContext context)
        {
            _serviceDictionary = new ConcurrentDictionary<string, object>();
            await _next(context);
        }


        /// <summary>
        /// 获取MemCache上下文
        /// </summary>
        public static IMemCache MemCache
        {
            get
            {
                return GetService<IMemCache>();
            }
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        public static byte[] NewVerifyCode(string codeKey = null)
        {
            return new VerifyCode(HttpContext).GetVerifyCode(codeKey);
        }


        
        /// <summary>
        /// 获取当前登录会员
        /// </summary>
        public static MemberModel CurrentMember
        {
            get
            {
                var claimsIdentity = HttpContext.User.Identities.Where(w => w.AuthenticationType == ConstParameters.MemberAuthScheme).FirstOrDefault();
                if (claimsIdentity == null)
                {
                    return null;
                }
                var claims = claimsIdentity.Claims;
                return new MemberModel()
                {
                    UserId = claims.Where(w => w.Type == ClaimTypes.Sid).Select(u => u.Value).FirstOrDefault(),
                    Account = claims.Where(w => w.Type == ClaimTypes.NameIdentifier).Select(u => u.Value).FirstOrDefault(),
                    NickName = claims.Where(w => w.Type == ClaimTypes.Name).Select(u => u.Value).FirstOrDefault(),
                    UserPwd = claims.Where(w => w.Type == ClaimTypes.Spn).Select(u => u.Value).FirstOrDefault(),
                    LoginIPAddress = claims.Where(w => w.Type == ClaimTypes.Dns).Select(u => u.Value).FirstOrDefault(),
                    HeadIcon = claims.Where(w => w.Type == ClaimTypes.Actor).Select(u => u.Value).FirstOrDefault(),
                };
            }
        }




        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        public static OperatorModel CurrentSysUser
        {
            get
            {
                var claimsIdentity = HttpContext.User.Identities.Where(w => w.AuthenticationType == ConstParameters.SysManageAuthScheme).FirstOrDefault();
                if (claimsIdentity == null)
                {
                    return null;
                }
                var claims = claimsIdentity.Claims;
                return new OperatorModel()
                {
                    UserId = claims.Where(w => w.Type == ClaimTypes.Sid).Select(u => u.Value).FirstOrDefault(),
                    Account = claims.Where(w => w.Type == ClaimTypes.Name).Select(u => u.Value).FirstOrDefault(),
                    RealName = claims.Where(w => w.Type == ClaimTypes.GivenName).Select(u => u.Value).FirstOrDefault(),
                    OrganizeId = claims.Where(w => w.Type == ClaimTypes.PrimarySid).Select(u => u.Value).FirstOrDefault(),
                    DepartmentId = claims.Where(w => w.Type == ClaimTypes.PrimaryGroupSid).Select(u => u.Value).FirstOrDefault(),
                    RoleId = claims.Where(w => w.Type == ClaimTypes.Role).Select(u => u.Value).FirstOrDefault(),
                    LoginIPAddress = claims.Where(w => w.Type == ClaimTypes.Dns).Select(u => u.Value).FirstOrDefault(),
                    IsSystem = claims.Where(w => w.Type == ClaimTypes.IsPersistent).Select(u => u.Value).FirstOrDefault().ToBool()
                };
            }
        }




        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILogger GetLogger(string name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return GetService<ILoggerFactory>().CreateLogger(name);
            }
            return GetService<ILogger>();
        }
        
        public static T GetService<T>()
        {
            //放入字典集合，一次请求同一Service重复利用
            var typeName = typeof(T).Name;
            return (T)_serviceDictionary.GetOrAdd(typeName, w => ServiceProvider.GetService<T>());
            //可考虑
            //return (T)HttpContext.RequestServices.GetService(typeof(T));
        }


        /// <summary>
        /// 删除文件，, 或 ; 分开
        /// </summary>
        /// <param name="entyFile"></param>
        public static void DeleteFiles(string entyFile)
        {
            Task.Run(() =>
            {
                var logger = GetLogger("CoreDeleteFiles");
                try
                {
                    var files = entyFile.Split(new char[] { ',', ';' });
                    foreach (var file in files)
                    {
                        try
                        {
                            FileHelper.DeleteFile(FileHelper.MapFilePath(file));
                        }
                        catch (Exception ex)
                        {
                            logger.LogError("删除文件["+file+"]"+ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("删除文件[" + entyFile + "]" + ex.Message);
                }
            });
        }

        /// <summary>
        /// 转换图片方法
        /// </summary>
        /// <param name="attFiles"></param>
        /// <param name="endPage"></param>
        /// <param name="logger"></param>
        public static void ConvertFileToImage(List<string> attFiles, int endPage,ILogger logger)
        {
            var startPage = 1;
            //生成图片
            Task.Factory.StartNew(() =>
            {
                try
                {
                    attFiles.ForEach(cfile =>
                    {
                        var fileFullPath = FileHelper.MapFilePath(cfile);
                        if (!File.Exists(fileFullPath))
                        {
                            logger.LogError("文件生成图片失败：文件不存在，" + fileFullPath);
                        }

                        System.Threading.Thread.Sleep(1000);

                        //cmd 执行图片生成
                        FileProcess.RunProcess(ConfigManager.ConvertFile46AppPath, $"{FileHelper.MapFilePath(cfile)} {startPage} {endPage}");
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError("文件生成图片失败：" + ex.ToString());
                }
            });
        }
    }

    /// <summary>
    /// CoreProvider 上下文支持
    /// </summary>
    public static class StaticCoreContextExtensions
    {

        public static void AddCoreProvider(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IMemCache, MemCache>();

            //add configuration
            //CoreContextProvider.Configuration =configuration;
            //去除此方式 本页已有ServiceProvider = serviceProvider.CreateScope().ServiceProvider;取容器写法
            //CoreContextProvider.ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 公用上下文
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCoreContextProvider(this IApplicationBuilder app)
        {
            //var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            //var hostingEnvironment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

            //CoreContextProvider.Configure(httpContextAccessor, hostingEnvironment);
            app.UseMiddleware<CoreContextProvider>();
            return app;
        }
    }


}
