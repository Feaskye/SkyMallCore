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
    public class DbBackupRespository : RespositoryBase<DbBackup>, IDbBackupRespository
    {
        public DbBackupRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        { }

        public void DeleteForm(string keyValue)
        {
            using (var db =this.BeginTransaction())
            {
                var dbBackup = this.Get(keyValue);
                if (dbBackup != null)
                {
                    FileHelper.DeleteFile(dbBackup.FilePath);
                }
                this.Delete(dbBackup);
                db.Commit();
            }
        }
        public void ExecuteDbBackup(DbBackup DbBackup)
        {
            this.ExecuteSql(string.Format("backup database {0} to disk ='{1}'", DbBackup.DbName, DbBackup.FilePath));
            DbBackup.FileSize = FileHelper.ToFileSize(FileHelper.GetFileSize(DbBackup.FilePath));
            DbBackup.FilePath = "/Resource/DbBackup/" + DbBackup.FileName;
            this.Insert(DbBackup);
        }

    }

}
