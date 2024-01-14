using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlanetMobileService.Models
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

    public class UserInfoViewModel
    {
        public string UserName { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string UserName { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "The {0} must be only {1} characters long.")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "The {0} must be only {1} characters long.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = "The {0} must be only {1} characters long.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "For Names use letters only")]
        [Display(Name = "FirstName")]
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
        [Required]
        [Compare("isTrue", ErrorMessage = "Please agree to Terms and Conditions")]
        public bool IAgree { get; set; }

        public bool isTrue
        { get { return true; } }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = "The {0} must be only {1} characters long.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = "The {0} must be only {1} characters long.")]
        public string Email { get; set; }
    }
}
