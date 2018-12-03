using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public class SysUserLogOnService : ISysUserLogOnService
    {
        ISysUserLogOnRespository _Respository;
        public SysUserLogOnService(ISysUserLogOnRespository sysUserLogOnRespository)
        {
            _Respository = sysUserLogOnRespository;
        }

        public SysUserLogOn GetForm(string key)
        {
            return _Respository.FirstOrDefault(w => w.UserId == key);
        }


        public void UpdateForm(SysUserLogOn userLogOnEntity)
        {
            _Respository.Update(userLogOnEntity);
        }


        public InvokeResult<bool> RevisePassword(string oldPassword, string userPassword, string keyValue)
        {
            var oldUserLogOn = _Respository.Get(keyValue);
            oldPassword = Md5Hash.Md5(DESEncrypt.Encrypt(Md5Hash.Md5(oldPassword, 32).ToLower(), oldUserLogOn.UserSecretkey).ToLower(), 32).ToLower();
            if (oldUserLogOn.UserPassword != oldPassword)
            {
                return RequestResult.Failed<bool>("原始密码错误");
            }
            SysUserLogOn userLogOnEntity = new SysUserLogOn();
            userLogOnEntity.Id = keyValue;
            userLogOnEntity.UserSecretkey = Md5Hash.Md5(Common.CreateNo(), 16).ToLower();
            userLogOnEntity.UserPassword = Md5Hash.Md5(DESEncrypt.Encrypt(Md5Hash.Md5(userPassword, 32).ToLower(), userLogOnEntity.UserSecretkey).ToLower(), 32).ToLower();
            var b= _Respository.Update(userLogOnEntity);
            return RequestResult.Success(b);
        }


    }


}
