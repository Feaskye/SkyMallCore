using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyNetCore.Common
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/auth/token";

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(10);

        public SigningCredentials SigningCredentials { get; set; }

    }
}
