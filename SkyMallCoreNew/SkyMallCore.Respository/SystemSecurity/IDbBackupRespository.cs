using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Respository
{
    public interface IDbBackupRespository : IRespositoryBase<DbBackup>
    {
        void DeleteForm(string keyValue);
        void ExecuteDbBackup(DbBackup dbBackupEntity);
    }
}
