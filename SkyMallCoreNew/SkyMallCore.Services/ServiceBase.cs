using SkyCore.GlobalProvider;
using SkyMallCore.Core;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SkyMallCore.Services
{
    public class ServiceBase<TModel> where TModel : ModelEntity,new()
    {

        /// <summary>
        /// 过滤条件，基础参数
        /// </summary>
        /// <returns></returns>
        public virtual Expression<Func<TModel, bool>> GetFilterEnabled() 
        {
            bool ignore = CoreContextProvider.CurrentSysUser != null
                                    && (CoreContextProvider.HttpContext.Request.Path.Value.ToLower().Contains("systemmanage") ||
                                    CoreContextProvider.HttpContext.Request.Path.Value.ToLower().Contains("systemsecurity"));
            if (ignore)
            {
                return ExtLinq.True<TModel>();
            }
            return param => param.EnabledMark == true;
        }


        public Func<IQueryable<TModel>, IOrderedQueryable<TModel>> Order()
        {
            return o => o.OrderBy(b => b.SortCode);
        }

    }
}
