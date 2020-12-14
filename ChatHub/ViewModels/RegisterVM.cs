using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatHub.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required."), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = " Confirm password is required."), DataType(DataType.Password)]
        [Compare("Password"), Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}