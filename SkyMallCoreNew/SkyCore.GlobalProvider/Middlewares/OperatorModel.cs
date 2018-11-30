
using System;

namespace SkyCore.GlobalProvider
{

    public class BaseUser
    {
        public string UserId { get; set; }
        public string Account { get; set; }
        public string RealName { get; set; }

        public string UserPwd { get; set; }
        
        public string LoginIPAddress { get; set; }


    }

    public class OperatorModel: BaseUser
    {
        public string OrganizeId { get; set; }
        public string DepartmentId { get; set; }
        public string RoleId { get; set; }


        public string UserCode{ get; set; }

        //public string LoginIPAddressName { get; set; }
        //public string LoginToken { get; set; }
        //public DateTime LoginTime { get; set; }
        public bool IsSystem { get; set; }
    }


    public class MemberModel : BaseUser
    {
        ///// <summary>
        ///// 积分
        ///// </summary>
        //public int Integral { get; set; }

        public string NickName { get; set; }
        public string HeadIcon { get; internal set; }
        //public int UserLevel { get; set; }



        //public DateTime? Birthday { get; set; }
        //public string MobilePhone { get; set; }
        //public string Telephone { get; set; }
        ///// <summary>
        ///// 单位
        ///// </summary>
        //public string Company { get; set; }
        ///// <summary>
        ///// 传真
        ///// </summary>
        //public string Tax { get; set; }
        //public string Address { get; set; }
        ///// <summary>
        ///// 宅电
        ///// </summary>
        //public string HomePhone { get; set; }

        ///// <summary>
        ///// 邮编
        ///// </summary>
        //public string ZipCode { get; set; }
        ///// <summary>
        ///// 职务
        ///// </summary>
        //public string Position { get; set; }

        //public string Email { get; set; }
    }



}
