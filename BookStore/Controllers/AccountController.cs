using AutoMapper;
using BookStore.DTOs.Account;
using BookStore.DTOs.Password;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        SignInManager<IdentityUser> SignInManager;
        UserManager<IdentityUser> UserManager;
        IMapper mapper;

        public AccountController(IMapper mapper, UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> signInManager)
        {
            this.mapper = mapper;
            this.UserManager = UserManager;
            SignInManager = signInManager;
        }

        //Login
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Logs in a user and returns a JWT token.", Description = "Authenticates the user with the provided username and password and returns a JWT token if successful.")]
        [SwaggerResponse(200, "JWT token returned successfully", typeof(string))]
        [SwaggerResponse(401, "Invalid username or password")]
        public IActionResult Login(LoginDTO loginDTO ) 
        {
            var r = SignInManager.PasswordSignInAsync(loginDTO.username, loginDTO.password, false, false).Result;
            if (r.Succeeded) 
            {
                var user = UserManager.FindByNameAsync(loginDTO.username).Result;
                #region claims
                List<Claim> userdata = new List<Claim>();
                userdata.Add(new Claim(ClaimTypes.Name,user.UserName));
                userdata.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));

                var roles=UserManager.GetRolesAsync(user).Result;
                foreach (var role in roles) 
                {
                    userdata.Add(new Claim(ClaimTypes.Role, role));
                }
                #region Secret Key
                var key = "welcome to my secret key Habiba Mohamed";
                var secetkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                var signingCredential = new SigningCredentials(secetkey,SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    claims: userdata,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signingCredential
                    );
                var tokenstring = new JwtSecurityTokenHandler().WriteToken(token);
                #endregion
                // Map to LoginResponseDTO
                var response = mapper.Map<LoginResponseDTO>(user);
                response.Token = tokenstring;
                response.Roles = roles.ToList();

                return Ok(response);
                //return Ok(tokenstring);

                #endregion
            }
            else { return Unauthorized("invaild username or password"); }
        }

        //Change password
        [HttpPost("ChangePassword")]
        [SwaggerOperation(Summary = "Changes the password of a user.", Description = "Changes the password of the user identified by the given ID, using the provided old and new passwords.")]
        [SwaggerResponse(200, "Password changed successfully")]
        [SwaggerResponse(400, "Invalid request or validation errors")]
        [SwaggerResponse(401, "Unauthorized if the user is not authenticated")]
        public IActionResult ChangePassword(ChangePasswordDTO pass)
        {
            if (ModelState.IsValid) 
            {
               var password=UserManager.FindByIdAsync(pass.Id).Result;
                var r = UserManager.ChangePasswordAsync(password, pass.OldPassword, pass.NewPassword).Result;
                if (r.Succeeded) { return Ok("Password Changed successfully"); }
                else {  return BadRequest(r.Errors); }
            } 
            else 
            { return BadRequest(ModelState); }
        }


        //Logout
        [HttpGet("Logout")]
        [SwaggerOperation(Summary = "Logs out the user.", Description = "Signs out the user and invalidates their authentication.")]
        [SwaggerResponse(200, "User logged out successfully")]
        public IActionResult Logout()
        {
            SignInManager.SignOutAsync();
            return Ok("logged out succssfully");
        }
    }
}
