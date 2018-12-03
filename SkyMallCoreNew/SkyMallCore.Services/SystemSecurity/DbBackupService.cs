using SkyCoreLib.Utils;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyMallCore.Services
{
    public class DbBackupService : ServiceBase<DbBackup>, IDbBackupService
    {
        ISysLogRespository _SysLogRespository;
        IDbBackupRespository _Respository;

        public DbBackupService(ISysLogRespository sysLogRespository, IDbBackupRespository respository)
        {
            _SysLogRespository = sysLogRespository;
            _Respository = respository;
        }


        public List<DbBackup> GetList(string queryJson)
        {
            var expression = base.GetFilterEnabled();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyword = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "DbName":
                        expression = expression.And(t => t.DbName.Contains(keyword));
                        break;
                    case "FileName":
                        expression = expression.And(t => t.FileName.Contains(keyword));
                        break;
                }
            }
            return _Respository.Get(expression).OrderByDescending(t => t.BackupTime).ToList();
        }
        public DbBackup GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(keyValue);
        }
        public void SubmitForm(DbBackup dbBackup)
        {
            dbBackup.Id = Common.GuId();
            dbBackup.EnabledMark = true;
            dbBackup.BackupTime = DateTime.Now;
            _Respository.ExecuteDbBackup(dbBackup);
        }
    }


}
