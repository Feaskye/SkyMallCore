using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyMallCore.Services
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        ISysLogRespository _LogRespository;
        IArticleCategoryRespository _Respository;
        ISysModuleService _SysModuleService;
        ISysModuleButtonService _SysModuleButtonService;

        public ArticleCategoryService(ISysLogRespository sysLogRespository, IArticleCategoryRespository ArticleCategoryRespository,
            ISysModuleService sysModuleService,
        ISysModuleButtonService sysModuleButtonService
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = ArticleCategoryRespository;
            _SysModuleService = sysModuleService;
            _SysModuleButtonService = sysModuleButtonService;
        }



        public List<ArticleCategory> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<ArticleCategory>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Title.Contains(keyword));
                expression = expression.Or(t => t.ShortTitle.Contains(keyword));
            }
            expression = expression.And(t => t.ParentId == 2);
            return _Respository.Get(expression).OrderBy(t => t.SortCode).ToList();
        }

        public ArticleCategory GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(keyValue);
        }
        public void SubmitForm(ArticleCategory ArticleCategory, string[] permissionIds, string keyValue)
        {
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    ArticleCategory.Id = keyValue;
            //}
            //else
            //{
            //    ArticleCategory.Id = Common.GuId();
            //}
            //var moduledata = _SysModuleService.GetList();
            //var buttondata = _SysModuleButtonService.GetList();
            //List<ArticleCategoryAuthorize> ArticleCategoryAuthorizes = new List<ArticleCategoryAuthorize>();
            //foreach (var itemId in permissionIds)
            //{
            //    ArticleCategoryAuthorize ArticleCategoryAuthorize = new ArticleCategoryAuthorize();
            //    ArticleCategoryAuthorize.Id = Common.GuId();
            //    ArticleCategoryAuthorize.ObjectType = 1;
            //    ArticleCategoryAuthorize.ObjectId = ArticleCategory.Id;
            //    ArticleCategoryAuthorize.ItemId = itemId;
            //    if (moduledata.Find(t => t.Id == itemId) != null)
            //    {
            //        ArticleCategoryAuthorize.ItemType = 1;
            //    }
            //    if (buttondata.Find(t => t.Id == itemId) != null)
            //    {
            //        ArticleCategoryAuthorize.ItemType = 2;
            //    }
            //    ArticleCategoryAuthorizes.Add(ArticleCategoryAuthorize);
            //}
            //_Respository.SubmitForm(ArticleCategory, ArticleCategoryAuthorizes, keyValue);
        }


        public void SubmitForm(ArticleCategory roleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                roleEntity.Id = keyValue;
                _Respository.Update(roleEntity);
            }
            else
            {
                //roleEntity.Category = 2;
                _Respository.Insert(roleEntity);
            }
        }


    }


}
