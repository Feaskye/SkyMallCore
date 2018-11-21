using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCore.WebApi
{
    public class ApiAuthorizedOptions
    {
        public string Name { get; set; }

        public string EncryptKey { get; set; }

        public int ExpiredSecond { get; set; }
    }
}
