using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel.SystemManage
{
    public class ManagerView
    {
        public string Id { get; set; }

        public string Account { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public string HeadIcon { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string WeChat { get; set; }
        public string ManagerId { get; set; }
        public int? SecurityLevel { get; set; }
        public string Signature { get; set; }
        public string OrganizeId { get; set; }
        public string DepartmentId { get; set; }
        public string RoleId { get; set; }
        public string DutyId { get; set; }
        public bool? IsAdministrator { get; set; }

        public string Description { get; set; }
    }
}
