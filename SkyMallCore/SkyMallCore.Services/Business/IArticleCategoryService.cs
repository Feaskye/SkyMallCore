using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface IArticleCategoryService
    {

        List<ArticleCategory> GetList(string keyword = "");


        ArticleCategory GetForm(string keyValue);


        void DeleteForm(string keyValue);


        void SubmitForm(ArticleCategory ArticleCategory, string[] permissionIds, string keyValue);


        void SubmitForm(ArticleCategory roleEntity, string keyValue);
    }


}
