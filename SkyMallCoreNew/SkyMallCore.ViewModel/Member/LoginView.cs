using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class LoginView
    {
        public string UserName { get; set; }
    
        public bool RemDay { get; set; }

        public string Password { get; set; }

        public string Code { get; set; }

        public string ReturnUrl { get; set; }

    }

    public class RegisterViewModel
    {
        [Display(Name = "用户名")]
        [Required(ErrorMessage ="不能为空")]
        [StringLength(maximumLength:12)]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "不能为空")]
        public string Password { get; set; }

        //public string Password { get; set; }

        [Display(Name = "验证码")]
        [Required(ErrorMessage = "不能为空")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

    }
    




}
