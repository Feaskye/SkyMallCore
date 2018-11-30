using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCore.Respository
{
    public class HelpRespository : AuditedRespository<Help>, IHelpRespository
    {
        public HelpRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        {
        }

        public void GetFullTextResult()
        {
            //单子段索引
            //this.Get(w=>EF.Functions.FreeText(w.Title,"kkk"));
            //全文索引 
            //考虑：https://github.com/npgsql/Npgsql.EntityFrameworkCore.PostgreSQL/blob/dev/doc/release-notes/2.1.md
            //或者使用FromSql方式Contains("","'123' or 'wer' or 'sdfsdf'")
        }



        //public void SubmitForm(SysRole sysRole, List<SysRoleAuthorize> sysRoleAuthorizes, string keyValue)
        //{
        //    using (var db =this.BeginTransaction())
        //    {
        //        if (!string.IsNullOrEmpty(keyValue))
        //        {
        //            this.Update(sysRole);
        //        }
        //        else
        //        {
        //            sysRole.Category = 1;
        //            this.Insert(sysRole);
        //        }
        //        SysRoleAuthorizeRespository.Delete(t => t.ObjectId == sysRole.Id);
        //        SysRoleAuthorizeRespository.Insert(sysRoleAuthorizes);
        //        db.Commit();
        //    }
        //}




    }

}
