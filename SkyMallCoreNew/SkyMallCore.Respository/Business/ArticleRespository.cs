using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkyMallCore.Core;

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

        /// <summary>
        /// 返回统计集合  类别、数量、金额
        /// </summary>
        /// <param name="cateIds"></param>
        /// <param name="isTopic"></param>
        /// <returns></returns>
        public Dictionary<string,int> GetStaticsCount(List<string> cateIds,bool isTopic = false)
        {
            var expression = ExtLinq.True<Article>();
            expression = expression.And(w => w.EnabledMark == true);
            if (!isTopic)
            {
                expression = expression.And(w => cateIds.Contains(w.CategoryId));
            }
            else
            {
                expression = expression.And(w => cateIds.Contains(w.SpecialTopicId));
            }
            var data = this.GetFeilds(u => new { u.Id, u.CategoryId, u.SpecialTopicId,u.RequireAmount }, expression,o=>o.OrderBy(b=>b.SortCode));
            if (!isTopic)
            {
                return data.GroupBy(g => g.CategoryId).Select(u => new { u.Key, Count = u.Count() })
                        .ToDictionary(k=>k.Key,v=> v.Count);
            }
            return data.GroupBy(g => g.SpecialTopicId).Select(u => new { u.Key, Count = u.Count() })
                        .ToDictionary(k => k.Key, v => v.Count);
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
