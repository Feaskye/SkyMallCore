using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Core;
using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCore.Respository
{
    public class SysUserRespository: RespositoryBase<SysUser>,ISysUserRespository
    {
        ISysUserLogOnRespository SysUserLogOnRespository;
        public SysUserRespository(ISkyMallDbContext skyMallDbContext
            , ISysUserLogOnRespository sysUserLogOnRespository) 
            : base(skyMallDbContext)
        {
            SysUserLogOnRespository = sysUserLogOnRespository;
        }


        public IList<SysUser> GetSysUsers()
        {
            return this.GetAll().ToList();
        }


        public void DeleteForm(string userId)
        {
            using (var db =base.BeginTransaction())
            {
                try
                {
                    this.Delete(t => t.Id == userId);
                    SysUserLogOnRespository.Delete(t => t.UserId == userId);
                    this.Commit();
                }
                catch (Exception ex) {
                    this.Rollback();
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        public void SubmitForm(SysUser userEntity, SysUserLogOn userLogOnEntity, string userId)
        {
            using (var db = base.BeginTransaction())
            {
                try
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        this.Update(userEntity);
                    }
                    else
                    {
                        userLogOnEntity.Id = userEntity.Id;
                        userLogOnEntity.UserId = userEntity.Id;
                        userLogOnEntity.UserSecretkey = Md5Hash.Md5(Common.CreateNo(), 16).ToLower();
                        userLogOnEntity.UserPassword = Md5Hash.Md5(DESEncrypt.Encrypt(Md5Hash.Md5(userLogOnEntity.UserPassword, 32).ToLower(), userLogOnEntity.UserSecretkey).ToLower(), 32).ToLower();
                        this.Insert(userEntity);
                        SysUserLogOnRespository.Insert(userLogOnEntity);
                    }
                    this.Commit();
                }
                catch (Exception ex)
                {
                    this.Rollback();
                    Console.WriteLine(ex.ToString());
                }
            }
        }


    }

    public class RespositoryFactory
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            DbContextFactory.Initialize(services, configuration);

            services.AddScoped(typeof(IRespositoryBase<>), typeof(RespositoryBase<>));
            //services.AddScoped<ISysUserRespository, SysUserRespository>();
            var scopedServices = Reflector.GetScopedList(typeof(RespositoryFactory).Assembly)
                .Where(w => w.Interface.Name.EndsWith("Respository")).ToList();
               scopedServices .ForEach(item =>
                {
                    services.AddScoped(item.Interface, item.Class);
                });
        }
    }

}
