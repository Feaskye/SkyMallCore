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
    public class ArticleRespository : AuditedRespository<Article>, IArticleRespository
    {
        IArticleCategoryRespository _ArticleCategoryRespository;
        public ArticleRespository(ISkyMallDbContext skyMallDbContext,
            IArticleCategoryRespository articleCategoryRespository) : base(skyMallDbContext)
        {
            _ArticleCategoryRespository = articleCategoryRespository;
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
