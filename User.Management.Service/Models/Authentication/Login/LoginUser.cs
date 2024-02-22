using System.ComponentModel.DataAnnotations;

namespace User.Management.Service.Models.Authentication.Login
{
    public class LoginUser
    {
        [Required(ErrorMessage ="Username is required")]
        public string? userName { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public string? password { get; set; }
    }
}
