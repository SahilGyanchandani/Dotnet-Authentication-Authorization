using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Management.Service.Models.Authentication.Sign_Up;
using User.Management.Service.Models;

namespace User.Management.Service.Services
{
    public interface IUserAuthentication
    {
        Task<ServiceResponse<string>> RegisterUser(RegisterUser registerUser);
    }
}
