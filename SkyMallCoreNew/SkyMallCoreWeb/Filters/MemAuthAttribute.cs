using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCoreWeb.Filters
{
    /// <summary>
    /// todo 加入需要的逻辑
    /// </summary>
    public class MemAuthAttribute: ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }


    }
}
