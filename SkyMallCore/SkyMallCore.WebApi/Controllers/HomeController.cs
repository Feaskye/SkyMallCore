using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.WebApi.Models;

namespace SkyMallCore.WebApi.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : ApiControllerBase
    {
        /// <summary>
        /// 获取首页内容  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Index()
        {
            return new { data=100};
        }

        /// <summary>
        /// 提交文件
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<bool> SubmitFile([FromBody]FileUploadParam param)
        {
            FileStream fileStream = param.fileStreams;

            return new ApiResult<bool>(true);
        }





        //private static ApiResult<T> Success(T data) where T:class
        //{
        //    return new ApiResult<T>() { Success = true,Data = data };
        //}


    }











}
