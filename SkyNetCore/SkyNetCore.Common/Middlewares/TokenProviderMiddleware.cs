using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SkyNetCore.Common
{
    /// <summary>
    /// 通过POST访问/auth/token
    /// 只支持POST来生成access_token
    /// 其他都是非法请求
    /// </summary>
    public class TokenProviderMiddleware
    {
        private RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        public TokenProviderMiddleware(RequestDelegate request,IOptions<TokenProviderOptions> options)
        {
            _next = request;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                await _next(context);
            }
            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
                || !context.Request.HasFormContentType)
            {
                await ReturnBadRequest(context);
            }

            await GenericAuthorizedResult(context);
        }

        private async Task GenericAuthorizedResult(HttpContext context)
        {
            var userName = context.Request.Form["username"];
            var password = context.Request.Form["password"];

            var identity = await GetIdentity(userName,password);
            if (identity == null)
            {
                await ReturnBadRequest(context);
                return;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(GetJwt(userName));
        }

        private string GetJwt(string userName)
        {
            var now = DateTime.Now;
            var claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Sub,userName),//JwtRegisteredClaimNames是结构体，里面包含了所有的可选项
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,now.ToUniversalTime().ToString(),
                                ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                now,
                now.Add(_options.Expiration),
                _options.SigningCredentials);

            //加密之后的字符串，这个字符串由3部分组成用‘.’分隔
            //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWV9.TJVA95OrM7E2cBab30RMHrHDcEfxjoYZgeFONFh7HgQ
            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new {
                access_token=encodeJwt,
                expires_in=(int)_options.Expiration.TotalSeconds,
                token_type="Bearer"
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        private Task<ClaimsIdentity> GetIdentity(string userName, string password)
        {
            var list = ApplicationInfo.GetAllApplication();
            bool isValidated = list.Any(x=>x.ApplicationName == userName && x.ApplicationPassword == password);
            if (isValidated)
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(userName,"Token"),new Claim[] { }));
            }
            return Task.FromResult<ClaimsIdentity>(null);
        }

        private async Task ReturnBadRequest(HttpContext context)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                error = "invalid_grant",
                error_msg = "valid failed"
            }));
        }
    }
}
