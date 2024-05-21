using Banana.Services.AuthApi.Data;
using Banana.Services.AuthApi.Models;
using Banana.Services.AuthApi.Models.Dto;
using Banana.Services.AuthApi.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace Banana.Services.AuthApi.Services
{
    public class AuthService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtGenerator generator) : IAuthService
    {
        private readonly AppDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IJwtGenerator _generator = generator;

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _userManager.Users.SingleOrDefault(e => e.Email.ToLower() == email.ToLower());

            if (user != null)
            {
                if(!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user,roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            var user = _context.Users.SingleOrDefault(e => e.UserName.ToLower() == loginRequest.Email.ToLower());

            var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (user == null || result == false)
            {
                return new LoginResponseDto() { Token = "", User = null };
            }
            var token = _generator.GenerateToken(user);
            return new LoginResponseDto() { Token = token, User = new() {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id
            }
            };
        }        

        public async Task<string> Register(RegisterDto registerDto)
        {
            ApplicationUser user = new()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                NormalizedEmail = registerDto.Email.ToUpper(),
                Name = registerDto.Name
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (result.Succeeded)
                {
                    var registeredUser = _context.Users.SingleOrDefault(u => u.UserName == registerDto.Email);
                    var userDto = new UserDto()
                    {
                        Email = registeredUser.Email,
                        PhoneNumber = registeredUser.PhoneNumber,
                        Name = registeredUser.Name,
                        Id = registeredUser.Id
                    };
                    return "";
                }else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }catch(Exception ex)
            {

            }
            return "Error Encountered";
        }
    }
}
