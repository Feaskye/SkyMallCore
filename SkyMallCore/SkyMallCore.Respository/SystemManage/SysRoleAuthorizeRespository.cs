using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCore.Respository
{
    public class SysRoleAuthorizeRespository : Data.Respository.AuditedRespository<SysRoleAuthorize>, ISysRoleAuthorizeRespository
    {
        public SysRoleAuthorizeRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        { }


     
    }

}
