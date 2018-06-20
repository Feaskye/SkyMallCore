using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface IArticleService
    {

        List<Article> GetList(string keyword = "");


        Article GetForm(string keyValue);


        void DeleteForm(string keyValue);


        void SubmitForm(Article Article, string[] permissionIds, string keyValue);


        void SubmitForm(Article roleEntity, string keyValue);
    }


}
