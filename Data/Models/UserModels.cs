using Data.DataAccess.Constant;
using Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserModel
    {
        public Guid Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        
    }

    public class UserCreateModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class ProfileUpdateModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class ChangePasswordModel
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string Email { get; set; }
    }

    public class UserSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }

    public class ProfileModel
    {
        public Guid Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
    }
}
