using Newtonsoft.Json;
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
    }

    public class UserCreateModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
