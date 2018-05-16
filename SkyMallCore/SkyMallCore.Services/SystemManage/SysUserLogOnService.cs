using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public class SysUserLogOnService: ISysUserLogOnService
    {
        ISysUserLogOnRespository _SysUserLogOnRespository;
        public SysUserLogOnService(ISysUserLogOnRespository sysUserLogOnRespository)
        {
            _SysUserLogOnRespository = sysUserLogOnRespository;
        }

        public SysUserLogOn GetForm(string key)
        {
            return _SysUserLogOnRespository.FirstOrDefault(w => w.UserId == key);
        }


        public void UpdateForm(SysUserLogOn userLogOnEntity)
        {
            _SysUserLogOnRespository.UpdateFields(userLogOnEntity);
        }


        public void RevisePassword(string userPassword, string keyValue)
        {
            SysUserLogOn userLogOnEntity = new SysUserLogOn();
            userLogOnEntity.Id = keyValue;
            userLogOnEntity.UserSecretkey = Md5Hash.Md5(Common.CreateNo(), 16).ToLower();
            userLogOnEntity.UserPassword = Md5Hash.Md5(DESEncrypt.Encrypt(Md5Hash.Md5(userPassword, 32).ToLower(), userLogOnEntity.UserSecretkey).ToLower(), 32).ToLower();
            _SysUserLogOnRespository.Update(userLogOnEntity);
        }


    }


}
