using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using User.Management.api.Models;
using User.Management.Service.Models;
using User.Management.Service.Models.Authentication.Login;
using User.Management.Service.Models.Authentication.Sign_Up;
using User.Management.Service.Services;

namespace User.Management.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IUserAuthentication _userAuth;
        public AuthenticationController(UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration,IEmailService emailService,IUserAuthentication userAuth)
        {
            _userManager = userManager;

            _roleManager = roleManager;

            _configuration = configuration;

            _emailService = emailService;

            _userAuth = userAuth;

        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] RegisterUser registerUser )
        {
           var userReg= await _userAuth.RegisterUser(registerUser);

            return Ok(userReg);
            
        }
        [HttpPost("sendMail")]
        public IActionResult TestEmailSending(string toEmail)
        {
            string emailSubject = "Test Email";

            string emailBody = $@"
                <html>
                <body>
                    <p>Dear {toEmail},</p>
                    <h1>Test mail</h1>-
                </body>
                </html>
            ";
            var message = new Message(new string[] { toEmail }, emailSubject, emailBody);
           

            _emailService.SendEmail(message);

            return StatusCode(StatusCodes.Status200OK,
                new Response { status = "Success", message = "Email sent successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser login)
        {
            //checking the user
            var user = await _userManager.FindByNameAsync(login.userName);

            //checking the password
            if (user != null && await _userManager.CheckPasswordAsync(user,login.password))
            {
                //claimList creation
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };

                //add role to list
                var userRole = await _userManager.GetRolesAsync(user);
                foreach(var role in userRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                //generate the token with the claims
                var jwtToken=GetToken(authClaims);

                //returning the token
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = jwtToken.ValidTo
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

             if(user != null)
            {
                var token= await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);

                string emailBody = $@"
                <html>
                <body>
                    <p>Dear {user.UserName},</p>
                    <ol>
                        <li>Click the following link to reset your password:</li>
                        <a href=""{forgotPasswordLink}"">Reset Password Link</a>
                    </ol>
                </body>
                </html>
            ";
                var message = new Message(new string[] { user.Email }, "Forgot password link", emailBody);
                _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status200OK,
               new Response { status = "Success", message = $"Password reset link send to your Email: {user.Email}" });

            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound,
               new Response { status = "Error", message = "Email not found ,plz register" });
            }

        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token,string email)
        {
            var model = new ResetPasswordModel{ token = token, email = email };

            return Ok(new
            {
                model
            });
            
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordPost([FromBody] ResetPasswordModel resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.email);

            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.token, resetPassword.password);
                if (!resetPassResult.Succeeded)
                {
                    foreach(var error  in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }

                return StatusCode(StatusCodes.Status200OK,
               new Response { status = "Success", message = $"Password has been changed " });

            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
               new Response { status = "Error", message = "Error while reseting the password." });
            }

        }

        private JwtSecurityToken GetToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddDays(1),
                claims: claims,
                signingCredentials: signIn);

            return token;
        }
    }
}
