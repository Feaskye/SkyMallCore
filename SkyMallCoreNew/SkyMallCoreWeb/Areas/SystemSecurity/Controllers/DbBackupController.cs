


using Microsoft.AspNetCore.Mvc;
using SkyMallCore.Services;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyCore.GlobalProvider;

namespace SkyMallCoreWeb.Areas.SystemSecurity.Controllers
{
    public class DbBackupController : SysSecBaseController
    {
        private IDbBackupService DbBackupApp;

        public DbBackupController(IDbBackupService dbBackupApp)
        {
            DbBackupApp = dbBackupApp;
        }


        [HttpGet]
        public ActionResult GetGridJson(string queryJson)
        {
            var data = DbBackupApp.GetList(queryJson);
            return Content(data.ToJson());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(DbBackup dbBackupEntity)
        {
            dbBackupEntity.FilePath = FileHelper.MapPath("~/Resource/DbBackup/" + dbBackupEntity.FileName + ".bak");
            dbBackupEntity.FileName = dbBackupEntity.FileName + ".bak";
            DbBackupApp.SubmitForm(dbBackupEntity);
            return Success("操作成功。");
        }
        [HttpPost]
        [TypeFilter(typeof(SysRoleAuthAttribute))]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            DbBackupApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpPost]
        [TypeFilter(typeof(SysRoleAuthAttribute))]
        public void DownloadBackup(string keyValue)
        {
            var data = DbBackupApp.GetForm(keyValue);
            string filename = System.Web.HttpUtility.UrlDecode(data.FileName);
            string filepath = FileHelper.MapPath(data.FilePath);
            if (FileHelper.FileExists(filepath))
            {
                FileDownHelper.DownLoadold(HttpContext, filepath, filename);
            }
        }
    }
}
