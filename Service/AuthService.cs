using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DTO;
using WebApplication1.IService;
using WebApplication1.Models;
using static WebApplication1.DTO.Responses;

namespace WebApplication1.Service
{
    public class AuthService : IAuth
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        public AuthService(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<GeneralResponse> CreateAccount(RegisterUserDTO userDTO)
        {
            if (userDTO is null)
            {
                return new GeneralResponse(false, "Model is Empty");
            }
            var newUser = new ApplicationUser()
            {
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                PasswordHash = userDTO.Password,
                UserName = userDTO.Email

            };

            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user != null)
            
                return new GeneralResponse(false, "The Email Already Exist");
            

            var createUser = await userManager.CreateAsync(newUser!, userDTO.Password);
            if(!createUser.Succeeded)
            {
                return new GeneralResponse(false, "An Error Occured");
            }

            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if (checkAdmin == null)
            {
                await roleManager.CreateAsync(new IdentityRole() {Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return new GeneralResponse(true, "Account Created Successfully");
            }
            else
            {
                var checkUser = await roleManager.FindByNameAsync("User");
                if (checkUser == null)
                {
                    await roleManager.CreateAsync(new IdentityRole() {Name = "User" });
                    await userManager.AddToRoleAsync(newUser, "User");
                    return new GeneralResponse(true, "Account Created Successfully");
                }
            }
            return new GeneralResponse(false, "Something is wrong");
        }



        public async Task<LogInResponse> LogInAccount(LogInDTO logInDTO)
        {
            if (logInDTO == null)
            {
                return new LogInResponse(false, null, "The Login Container is empty");
            }
            var getUser = await userManager.FindByEmailAsync(logInDTO.Email);
            if (getUser == null)
            {
                return new LogInResponse(false, null, "User not found");
            }

            bool IsPasswordChecked = await userManager.CheckPasswordAsync(getUser, logInDTO.Password);
            if (!IsPasswordChecked)
            {
                return new LogInResponse(false, null, "Invalid Email/Password");
            }
            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.FirstName, getUser.LastName, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);
            return new LogInResponse(true, token, "Login Successful");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Name, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
