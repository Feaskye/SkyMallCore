using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkyMallCore.WebApiUtils;

namespace SkyMallCore.WebApi.Controllers
{
    /// <summary>
    /// 基类
    /// </summary>
    [Route("api/{v:apiVersion}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ApiControllerBase : ControllerBase
    {
        /// <summary>
        /// 请求辅助类
        /// </summary>
        public HttpClientHelper HttpClientHelper;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        //public override CreatedAtActionResult CreatedAtAction(string actionName, string controllerName, object routeValues, object value)
        //{
        //      todo HttpClientHelper 待处理
        //    HttpClientHelper = new HttpClientHelper((IHttpClientFactory)HttpContext.RequestServices.GetService(typeof(IHttpClientFactory)), "http://localhost:63656/");
        //    return base.CreatedAtAction(actionName, controllerName, routeValues, value);
        //}





        /// <summary>
        /// 成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ApiResult<T> Success<T>(T data, string message = null)
        {
            return new ApiResult<T>(data, message: message);
        }


        /// <summary>
        /// 失败
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public ApiResult<T> Failed<T>(string message)
        {
            return new ApiResult<T>(default(T), false, message: message);
        }


    }
    


}