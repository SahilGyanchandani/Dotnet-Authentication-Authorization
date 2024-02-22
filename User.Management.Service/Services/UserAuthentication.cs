using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Management.Service.Models;
using User.Management.Service.Models.Authentication.Sign_Up;

namespace User.Management.Service.Services
{
    public class UserAuthentication : IUserAuthentication
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserAuthentication(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        { 
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ServiceResponse<string>> RegisterUser (RegisterUser registerUser)
        {
            var response = new ServiceResponse<string>();

            // Check if the user already exists
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null)
            {
                response.isSuccess = false;
                response.status = 403;
                response.message = "User already exists";
                return response;
            }

            // Create a new IdentityUser object
            var user = new IdentityUser
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Check if the role exists
            if (!await _roleManager.RoleExistsAsync(registerUser.role))
            {
                response.isSuccess = false;
                response.status = 500;
                response.message = "Role doesn't exist";
                return response;
            }
            else
            {
                // Create the user
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    response.isSuccess = false;
                    response.status = 500;
                    response.message = "Failed to create user";
                    return response;
                }

                // Assign the role to the user
                await _userManager.AddToRoleAsync(user, registerUser.role);

                ////Add Token to verify email
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                response.isSuccess = true;
                response.status = 201;
                response.message = "User created successfully";
                //response.response = token;
                return response;
            }
        }

    }
}
