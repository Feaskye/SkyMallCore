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
    public class SysModuleButtonRespository : Data.Respository.AuditedRespository<SysModuleButton>, ISysModuleButtonRespository
    {
        public SysModuleButtonRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        { }

        public void SubmitCloneButton(List<SysModuleButton> entitys)
        {
            using (var db = this.BeginTransaction())
            {
                foreach (var item in entitys)
                {
                    this.Insert(item);
                }
                db.Commit();
            }
        }

    }

}
