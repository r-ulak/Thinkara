using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Phonegap.OWIN.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "The {0} must be only {1} characters long.")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "FirstName")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "For Names use letters only")]
        [StringLength(45, ErrorMessage = "The {0} must be only {1} characters long.")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "LastName")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "For Names use letters only")]
        [StringLength(45, ErrorMessage = "The {0} must be only {1} characters long.")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "CountryCode")]
        [StringLength(2, ErrorMessage = "The {0} must be only {1} characters long.")]
        public string CountryCode { get; set; }
        public string LoginProvider { get; set; }
        [Required]
        [Compare("isTrue", ErrorMessage = "Please agree to Terms and Conditions")]
        public bool IAgree { get; set; }
        public bool isTrue
        { get { return true; } }
    }
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string UserName { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string UserName { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
    //
    // Summary:
    //     Used to return information needed to associate an external login
    public class ExternalLoginInfo
    {
        // Summary:
        //     Suggested user name for a user
        public string DefaultUserName { get; set; }
        //
        // Summary:
        //     Email claim from the external identity
        public string Email { get; set; }
        //
        // Summary:
        //     The external identity
        public ClaimsIdentity ExternalIdentity { get; set; }
        //
        // Summary:
        //     Associated login data
        public UserLoginInfoViewModel Login { get; set; }
    }
}
