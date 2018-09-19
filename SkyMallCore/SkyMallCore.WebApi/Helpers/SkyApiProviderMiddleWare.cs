using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyMallCore.WebApi.Helpers
{
    /// <summary>
    /// api 中间件处理相关请求
    /// 1：请求日志
    /// 2：请求参数、返回结果处理（如：加密、Token、Sign等）
    /// </summary>
    public class SkyApiProviderMiddleWare
    {

        private RequestDelegate _next;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDelegate"></param>
        public SkyApiProviderMiddleWare(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }
        

        public async Task Invoke(HttpContext context,ILoggerFactory loggerFactory)
        {

            ILogger<SkyApiProviderMiddleWare> logger = loggerFactory.CreateLogger<SkyApiProviderMiddleWare>();
            logger.LogInformation(GetRequestLog(context.Request));
            //创建http的原始请求和响应流
            var reqOrigin = context.Request.Body;
            var resOrigin = context.Response.Body;

            try
            {
                using (var reqStream = new MemoryStream())
                {
                    //替换Request
                    context.Request.Body = reqStream;
                    //请求参数数据
                    var request = context.Request;
                    var requestData = request.QueryString.ToString();
                    if (request.Method.ToLower() == HttpMethods.Post.ToLower())
                    {
                        if (request.Form.Any())
                        {
                            requestData = string.Join("&", request.Form.Select(u => u.Key + "=" + u.Value));
                        }
                        else
                        {
                            using (var reqReader = new StreamReader(resOrigin))
                            {
                                requestData += $"\r\n requestBody:{reqReader.ReadToEnd()}";
                            }
                        }
                    }
                    logger.LogInformation($"requestData:{requestData}");

                    using (var resStream = new MemoryStream())
                    {
                        //替换Response
                        context.Response.Body = resStream;
                        
                        //处理请求数据并写入
                        string writeStr = JsonConvert.SerializeObject(new
                        {
                            Name = "test"
                        });
                        //将数据写入请求流reqStream
                        using (var stWriter = new StreamWriter(reqStream))
                        {
                            stWriter.Write(writeStr);
                            stWriter.Flush();
                            //此处一定要设置=0，否则controller的action里模型绑定不了数据
                            reqStream.Position = 0;
                            //进入action
                            await _next(context);
                        }

                        //读取action返回的结果
                        string result = null;
                        using (var sreader = new StreamReader(resStream))
                        {
                            resStream.Position = 0;
                            result = sreader.ReadToEnd();
                            if (!string.IsNullOrEmpty(result))
                            {
                                logger.LogInformation($"result:{result}");
                            }
                        }

                        //返回结果
                        using (var streamWriter = new StreamWriter(resOrigin))
                        {
                            streamWriter.Write(result);
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
            finally
            {
                context.Request.Body = reqOrigin;
                context.Response.Body = resOrigin;
            }
        }



        private string GetRequestLog(HttpRequest request)
        {
            StringBuilder requestContent = new StringBuilder();
            requestContent.Append($"{DateTime.Now.ToString()}\r\n");
            requestContent.Append($"请求Uri：{request.Path}{request.QueryString} \r\n");
            requestContent.Append($"Method：{request.Method}\r\n");
            return requestContent.ToString();
        }
        





    }



    public static class SkyApiProviderExtensions
    {
        public static void AddSkyApiProvider(this IServiceCollection services)
        {
            //services.AddSingleton();
        }


        public static IApplicationBuilder UseSkyApiProvider(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<SkyApiProviderMiddleWare>();
            return builder;
        }



    }

    /*
     其他参考：
     http://www.iaspnetcore.com/Blog/BlogPost/5a5f5aefdb0fc9325cfee8e0/aspnet-core-middleware-intercepts-http-streams-to-encrypt-request-and-response-data
     */










}
