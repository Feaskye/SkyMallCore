using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class ChangeEmailView
    {
        public string Password { get; set; }

        public string Email { get; set; }
        

        public string VerifyCode { get; set; }

        public string UserId { get; set; }

    }


    public class ZoomImageView
    {
        public string HeadIcon { get; set; }

        public string ErrorMessage { get; set; }

    }


}
