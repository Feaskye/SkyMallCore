using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Data;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyMallCore.Respository
{
    public class SysItemsDetailRespository : AuditedRespository<SysItemsDetail>, ISysItemsDetailRespository
    {
        public SysItemsDetailRespository(ISkyMallDbContext skyMallDbContext) : base(skyMallDbContext)
        { }


        public List<SysItemsDetail> GetItemList(string enCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.*
                            FROM    Sys_ItemsDetail d
                                    INNER  JOIN Sys_Items i ON i.Id = d.ItemId
                            WHERE   1 = 1
                                    AND i.EnCode = @enCode
                                    AND d.EnabledMark = 1
                                    AND d.DeleteMark = 0
                            ORDER BY d.SortCode ASC");
            DbParameter[] parameter =
            {
                 new SqlParameter("@enCode",enCode)
            };
            return this.FromSql(strSql.ToString(), parameter);
        }



    }

}
