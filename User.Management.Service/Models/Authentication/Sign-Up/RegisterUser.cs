using System.ComponentModel.DataAnnotations;

namespace User.Management.Service.Models.Authentication.Sign_Up
{
    public class RegisterUser
    {
        [Required(ErrorMessage ="User Name is requiered")]
        public string? UserName { get; set; }

        [Required(ErrorMessage ="Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage ="Roles is required")]
        public string? role { get; set; }

    }
}
