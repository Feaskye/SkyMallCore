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
    /// 文件上传
    /// </summary>
    public class FileUploadController : ApiControllerBase
    {
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


    }











}
