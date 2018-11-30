using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.Respository;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyMallCore.Services
{
    public class SysItemsDetailService : ServiceBase<SysItemsDetail>, ISysItemsDetailService
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
            var expression = base.GetFilterEnabled();
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


        /// <summary>
        /// 获取界面字典集合
        /// </summary>
        /// <param name="enCode"></param>
        /// <returns></returns>
        public List<ListItem> GetListItem(string enCode, string[] childCodes = null)
        {
            var expression = base.GetFilterEnabled();
            if (childCodes == null)
            {
                expression = expression.And(w => w.SysItems != null && w.SysItems.EnCode == enCode);
            }
            else
            {
                expression = expression.And(w => w.SysItems != null && (w.SysItems.EnCode == enCode || w.SysItems.EnCode == enCode + "1")
                                                                    && childCodes.Contains(w.ItemCode));
            }
            return _Respository.GetFeilds(u => new ListItem { Code = u.ItemCode, Text = u.ItemName,ParentId = u.SysItems.EnCode }, expression
                , w => w.OrderBy(b => b.SortCode), "SysItems").ToList();
        }


        /// <summary>
        /// 获取单项
        /// </summary>
        /// <param name="encode"></param>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        public SysItemsDetail GetItem(string encode,string itemcode)
        {
            //w.SysItems.EnCode == encode &&
            return _Respository.Get(w => w.SysItems != null && w.SysItems.EnCode == encode && w.ItemCode == itemcode, w => w.OrderBy(b => b.SortCode), "SysItems").FirstOrDefault();
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
                SysItemsDetail.Id = keyValue;
                _Respository.Update(SysItemsDetail);
            }
            else
            {
                _Respository.Insert(SysItemsDetail);
            }
        }

    }


}
