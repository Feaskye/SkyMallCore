using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyMallCore.WebApi.Models;

namespace SkyMallCore.WebApi.Controllers
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class FileUploadController : ApiControllerBase
    {
        //private IHttpClientFactory HttpClientFactory;
        //public FileUploadController(IHttpClientFactory httpClientFactory)
        //{
        //    //
        //    HttpClientFactory = httpClientFactory;
        //}


        /// <summary>
        /// 请求测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<string>> Get()
        {
            //在Core2.1推出ActionResult<object>
            //ApiResult<object> 跟 ActionResult<object> 性质一样
            //http://www.iaspnetcore.com/Blog/BlogPost/5b84eac48078ff1124d9455c/exploring-actionresultt-in-aspnet-core-web-api-21
            
            var result = await HttpClientHelper.Get<string>("swagger/v1/swagger.json");
            return Success(result);
        }


        ///// <summary>
        ///// 提交文件
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ApiResult<bool> SubmitFile([FromBody]FileUploadParam param)
        //{
        //    FileStream fileStream = param.fileStreams;

        //    //提交到远程服务器
        //    //using (var _httpClientFactory = HttpClientFactory.CreateClient())
        //    //{
        //    //    var client = _httpClientFactory.CreateClient();
        //    //    client.BaseAddress = new Uri("http://api.github.com");
        //    //    string result = await client.GetStringAsync("/");
        //    //    return Ok(result);
        //    //}


        //    return new ApiResult<bool>(true);
        //}


    }











}
