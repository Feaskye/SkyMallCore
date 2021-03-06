﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SkyMallCore.WebApi.Controllers
{
    /// <summary>
    /// 代理商
    /// </summary>
    [ApiVersion("2.0")]
    public class AgentV2Controller : ApiControllerBase
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [Route("HasAgent")]
        [HttpGet]
        public ApiResult<bool> HasAgent([FromQuery]ExistAgentParams agentParams, int pageIndex = 1)
        {
            //if (Request.Query.Any(w=>w.Key == "q"))
            //{
            //    return NotFound();
            //}
            if (!ModelState.IsValid)
            {//参数过滤处理
                
                var count = ModelState.ErrorCount;
                return Failed<bool>("参数有误v2");
            }

            if (pageIndex == 1)
            {
                throw new Exception("dddv2");
            }

            return Success(true);
        }

        /// <summary>
        /// Get111111
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {

            return NotFound();
        }

        /// <summary>
        /// Post11111111
        /// </summary>
        /// <param name="value"></param>
        // POST api/values
        [HttpPost]
        public void SubmitPackage([FromBody]string value)
        {
        }

        /// <summary>
        /// 更新资料
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void UpdateInfo([FromForm]string value)
        {
        }

        /// <summary>
        /// Put111111
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }






       










    }
    
}