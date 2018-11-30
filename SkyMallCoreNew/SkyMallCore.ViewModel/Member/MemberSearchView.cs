using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class MemberSearchView
    {
        public string keyword { get; set; }

        public string GroupId { get; set; }
        public UserLevel? UserLevel { get; set; }
    }
}
