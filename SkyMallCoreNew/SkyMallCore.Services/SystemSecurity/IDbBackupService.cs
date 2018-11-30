using SkyMallCore.Core;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface IDbBackupService
    {
        List<DbBackup> GetList(string keyword);


        DbBackup GetForm(string keyValue);

        void DeleteForm(string keyValue);

        void SubmitForm(DbBackup dbBackup);


    }


}
