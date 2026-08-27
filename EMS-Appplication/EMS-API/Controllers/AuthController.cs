using DocumentFormat.OpenXml.Spreadsheet;
using EMS_Core.Domain.Entities;
using EMS_Core.DTOs;
using EMS_Core.Helpers;
using EMS_Core.ReponseWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private IOptions<JwtConfig> _jwtConfig;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AuthController(ILogger<AuthController> _logger, IOptions<JwtConfig> _jwtConfig,
            UserManager<AppUser> _userManager, RoleManager<AppRole> _roleManager)
        {
            this._logger = _logger;
            this._jwtConfig = _jwtConfig;
            this._roleManager = _roleManager;
            this._userManager = _userManager;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponseWrapper<string>>> RegisterUser([FromBody]RegisterDTO register)
        {
            _logger.LogInformation("{controller}.{method}.{message}", nameof(AuthController), nameof(RegisterUser), "Request recieved to add new user");
            AppUser user = new()
            {
                Name = register.Name,
                Email = register.Email,
                PhoneNumber = register.Phone,
                UserName=register.UserName
                
               
            };
            var createUser = await _userManager.CreateAsync(user, register.Password);
            if (createUser.Succeeded)
            {
                _logger.LogInformation("{controller}.{method}.{message}", nameof(AuthController), nameof(RegisterUser), $"{user.Name} has been registered.");

                var userRole = new AppRole()
                {
                    Name = register.Role
                };
                var checkIfRoleExists = await _roleManager.FindByNameAsync(userRole.Name);
                if (checkIfRoleExists is null)
                {
                    _logger.LogInformation("{controller}.{method}.{message}", nameof(AuthController), nameof(RegisterUser), $"{user.Name} has been registered as a {userRole.Name}.");
                    var createNewRole = await _roleManager.CreateAsync(userRole);
                }
                await _userManager.AddToRoleAsync(user, userRole.Name);
             
                return new APIResponseWrapper<string>()
                {
                    Data = null,
                    Message = "User has been added successfully.",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            return  new APIResponseWrapper<string>()
            {
                Data = null,
                Message = "Registration failed.Please try again later..",
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
        [HttpPost]
        public async Task<ActionResult<APIResponseWrapper<string>>> LoginUser([FromBody]LoginDTO login)
        {
            _logger.LogInformation("{controller}.{method}.{message}", nameof(AuthController), nameof(LoginUser), "Request recieved to login to application");
            var userExists = await _userManager.FindByEmailAsync(login.Email);
            if (userExists is not null)
            {
                var validateUser = await _userManager.CheckPasswordAsync(userExists,login.Password);
                if (validateUser)
                {
                    var getUserRole = await _userManager.GetRolesAsync(userExists);
                    var cliams = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,userExists.Name),
                        new Claim(ClaimTypes.NameIdentifier,userExists.Id),
                    };
                    if (getUserRole.Count > 0)
                    {
                        foreach(var role in getUserRole)
                        {
                            cliams.Add(new Claim(ClaimTypes.Role, role));
                        }
                    }
                    var token = JWTAuthTokenGenerator.GenerateToken(cliams,_jwtConfig);
                    _logger.LogInformation("{controller}.{method}.{message}", nameof(AuthController), nameof(LoginUser), $"{userExists.Name} logged in successfully.");
                    return new APIResponseWrapper<string>()
                    {
                        Data = token,
                        Message = "User has been logged successfully.",
                        StatusCode = StatusCodes.Status200OK
                    };

                }
            }
            return new APIResponseWrapper<string>()
            {
                Data = null,
                Message = "Invalid email or password",
                StatusCode = StatusCodes.Status400BadRequest
            };

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<APIResponseWrapper<string>>> ForgotPassword([FromBody] GenerateForgotPasswordToken token)
        {
            _logger.LogInformation("{controller}.{method}.{message}", nameof(AuthController), nameof(ForgotPassword), "Forgot password request has been received.");
            var user = await _userManager.FindByEmailAsync(token.Email);
            if(user==null)
                return new APIResponseWrapper<string>()
                {
                    Data = null,
                    Message = "User does not exists.",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            else
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                _logger.LogInformation("{controller}.{method}.{message}", nameof(AuthController), nameof(ForgotPassword), "Forgot password token has beend generated successfully.");
                return new APIResponseWrapper<string>()
                {
                    Data = resetToken,
                    Message = "User does not exists.",
                    StatusCode = StatusCodes.Status200OK
                };
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<APIResponseWrapper<string>>> ResetPassword([FromBody] ResetPasswordDTO resetPassword)
        {
            _logger.LogInformation("{controller}.{method}.{message}", nameof(AuthController), nameof(ResetPassword), "Password reset request has been received.");
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                return new APIResponseWrapper<string>()
                {
                    Data = null,
                    Message = "User does not exists.",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            var result=await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("{controller}.{method}.{message}", nameof(AuthController), nameof(ResetPassword), "Password has been changed successfully.");
                return new APIResponseWrapper<string>()
                {
                    Data = null,
                    Message = "Password has been updated successfully.",
                    StatusCode = StatusCodes.Status200OK
                };
            }
            else
            {
                _logger.LogError("{controller}.{method}.{message}", nameof(AuthController), nameof(ResetPassword), "Some error occured while updating password.");
                return new APIResponseWrapper<string>()
                {
                    Data = null,
                    Message = "Some error occured while updating password",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
