﻿
using System;

namespace SkyMallCore.Core
{
    public class OperatorModel
    {
        public string UserId { get; set; }
        public string Account { get; set; }
        public string RealName { get; set; }
        public string UserPwd { get; set; }
        public string OrganizeId { get; set; }
        public string DepartmentId { get; set; }
        public string RoleId { get; set; }
        public string LoginIPAddress { get; set; }


        public string UserCode{ get; set; }

        //public string LoginIPAddressName { get; set; }
        //public string LoginToken { get; set; }
        //public DateTime LoginTime { get; set; }
        public bool IsSystem { get; set; }
    }
}
