using System;
using System.ComponentModel.DataAnnotations;

namespace HelloWorld.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
