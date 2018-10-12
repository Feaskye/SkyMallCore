using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SkyMallCore.WebApiData;
using SkyMallCore.WebApiData.Models;

namespace SkyMallCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersionNeutral]
    public class TodoController : ApiControllerBase
    {
        IConfiguration Configuration;
        MysqlDbContext MysqlDb;
        public TodoController(IConfiguration configuration, MysqlDbContext mysqlDb)
        {
            Configuration = configuration;
            MysqlDb = mysqlDb;
        }


        //https://mp.weixin.qq.com/s/QL_ysisq4d5w7jh-SKHxNw
        /// <summary>
        /// MysqlDb
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Members")]
        public ApiResult<IEnumerable<Member>> GetMembers()
        {
            var data = MysqlDb.GetAll();
            var result = data.Select(p => new Member
            {
                UserName = p.UserName == "5" ? p.UserName : "0"
            });

            int[] test = new int[] { 1,3,5,6,7,8,0};
            var tdata = test.Select(u => (u == 3) ? u:0);


            return Success(data);
        }

        // GET: api/Todo/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Todo
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        //Reflection
        private object GetReflector()
        {
            //.NetCore 平台已经不支持直接输出到目录，仅仅可以在内存中Run。
            AssemblyBuilder.DefineDynamicAssembly(new System.Reflection.AssemblyName(Guid.NewGuid().ToString()),AssemblyBuilderAccess.RunAndCollect);
            var dllAssembly= AssemblyBuilder.GetAssembly(typeof(WebApiUtils.HttpClientHelper).Assembly.GetType());
            return dllAssembly;
        }






    }
}
