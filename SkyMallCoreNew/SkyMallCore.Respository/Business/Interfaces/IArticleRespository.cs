using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Respository
{
    public interface IArticleRespository : IRespositoryBase<Article>
    {

        /// <summary>
        /// 返回统计集合  类别、数量、金额
        /// </summary>
        /// <param name="cateIds"></param>
        /// <param name="isTopic"></param>
        /// <returns></returns>
        Dictionary<string, int> GetStaticsCount(List<string> cateIds, bool isTopic = false);


    }
}
