using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyNetCore.Common
{
    public class ApplicationInfo
    {
        public string ApplicationName { get; set; }

        public string ApplicationPassword { get; set; }

        public static IList<ApplicationInfo> GetAllApplication()
        {
            return new List<ApplicationInfo>()
            {
                new ApplicationInfo() { ApplicationName="Member", ApplicationPassword="123" },
                new ApplicationInfo() { ApplicationName="Order", ApplicationPassword="123" },
                new ApplicationInfo() { ApplicationName="Other", ApplicationPassword="123" }
            };
        }
    }
}
