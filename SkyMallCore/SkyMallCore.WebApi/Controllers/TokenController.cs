using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SkyMallCore.WebApiData;
using SkyMallCore.WebApiData.Models;

namespace SkyMallCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersionNeutral]
    public class TokenController : ApiControllerBase
    {

        public TokenController()
        {
        }

        /// <summary>
        /// 获取Token     ---- IdentityServer
        /// </summary>
        /// <returns></returns>
        public async Task<JObject> Get()
        {
            var disco = await DiscoveryClient.GetAsync($"{Request.Scheme}://{Request.Host}");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
            }

            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }

            return tokenResponse.Json;
        }
        





    }
}
