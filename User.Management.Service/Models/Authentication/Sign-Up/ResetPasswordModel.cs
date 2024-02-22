using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel.DataAnnotations;

namespace User.Management.Service.Models.Authentication.Sign_Up
{
    public class ResetPasswordModel
    {
        [Required]
        public string password { get; set; } = null!;

        [Compare("password", ErrorMessage = "The password and confirmation do not match")]
        public string ConfirmPassword { get; set; } = null!;

        public string email { get; set; } = null!;

        public string token { get; set; } = null!;
    }
}
