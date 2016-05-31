using System.ComponentModel.DataAnnotations;

namespace SimpleCMS.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Email { get; set; }

        [Display(Name = "Hometown", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string HomeTown { get; set; }

        [Display(Name = "Birthday", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public System.DateTime? Birthday { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(25)]
        [Display(Name = "PhoneNumber", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string PhoneNumber { get; set; }

        [Display(Name = "UserIdCode", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string UserIdCode { get; set; }

        public bool TwoFactorEnabled { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "MsgNewPasswordDontMatch",  ErrorMessageResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Email { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(4)]
        [Display(Name = "UserIdCode", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string UserIdCode { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(25)]
        [Display(Name = "PhoneNumber", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [Compare("Password", ErrorMessageResourceName = "MsgPasswordDontMatch", ErrorMessageResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        [Compare("MsgPasswordDontMatch", ErrorMessageResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(SimpleCMS.Resources.Blog.Resources))]
        public string Email { get; set; }
    }
}
