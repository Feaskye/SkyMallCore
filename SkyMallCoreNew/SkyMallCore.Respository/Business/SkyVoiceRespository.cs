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
    public class SkyVoiceRespository : AuditedRespository<SkyVoice>, ISkyVoiceRespository
    {
        public SkyVoiceRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        {
        }
        




    }

}
