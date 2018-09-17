using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SkyMallCore.WebApi.Controllers
{
    /// <summary>
    /// 基类
    /// </summary>
    [Route("api/[controller]")]
    public class ApiControllerBase : Controller
    {
        //public static ApiResult<T> Success(T data) where T : class
        //{
        //    return new ApiResult<T>() { Success = true, Data = data };
        //}
    }
}