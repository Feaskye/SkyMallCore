using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyMallCore.Services
{
    public class SysItemsDetailService : ISysItemsDetailService
    {
        ISysLogRespository _LogRespository;
        ISysItemsDetailRespository _Respository;
        public SysItemsDetailService(ISysLogRespository sysLogRespository, ISysItemsDetailRespository sysItemsDetailRespository
            )
        {
            _LogRespository = sysLogRespository;
            _Respository = sysItemsDetailRespository;
        }


        public IList<SysItemsDetail> GetList(string itemId = "", string keyword = "")
        {
            var expression = ExtLinq.True<SysItemsDetail>();
            if (!string.IsNullOrEmpty(itemId))
            {
                expression = expression.And(t => t.ItemId == itemId);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.ItemName.Contains(keyword));
                expression = expression.Or(t => t.ItemCode.Contains(keyword));
            }
            return _Respository.Get(expression).OrderBy(t => t.SortCode).ToList();
        }

        public List<SysItemsDetail> GetItemList(string enCode)
        {
            return _Respository.GetItemList(enCode);
        }
        public SysItemsDetail GetForm(string keyValue)
        {
            return _Respository.Get(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            _Respository.Delete(t => t.Id == keyValue);
        }
        public void SubmitForm(SysItemsDetail SysItemsDetail, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                _Respository.Update(SysItemsDetail);
            }
            else
            {
                _Respository.Insert(SysItemsDetail);
            }
        }

    }


}
