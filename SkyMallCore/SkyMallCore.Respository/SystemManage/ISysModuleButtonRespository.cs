using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Respository
{
    public interface ISysModuleButtonRespository : IRespositoryBase<SysModuleButton>
    {
        void SubmitCloneButton(List<SysModuleButton> entitys);
    }
}
