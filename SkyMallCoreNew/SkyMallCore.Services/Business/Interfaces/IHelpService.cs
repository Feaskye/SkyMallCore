using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Services
{
    public interface IHelpService
    {

        PagedList<Help> GetList(HelpSearchView searchView, int pageIndex = 1,int pageSize = 20);

        PagedList<HelpDetailView> GetHelps(HelpSearchView searchView, int pageIndex = 1, int pageSize = 20);

        List<Help> GetList(string keyword = "");


        Help GetForm(string keyValue);


        InvokeResult<bool> DeleteForm(string keyValue);

        InvokeResult<bool> DelHelp(string memberId,string keyValue);

        void SubmitForm(Help Help, string[] permissionIds, string keyValue);


        InvokeResult<bool> SubmitForm(Help roleEntity);


        HelpDetailView GetHelpByCode(HelpCode helpCode);
    }


}
