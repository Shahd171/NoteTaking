using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Note_Taking_App.Core.DTOs;
using Note_Taking_App.Core.Entities;
using Note_Taking_App.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Applcation.Services
{
    public class AuthService : IAuthService
    {
        private IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(IConfiguration config, UserManager<ApplicationUser> UserManager)
        {
            _config = config;
            _userManager = UserManager;

        }
        public async Task<AuthDto> Login(LoginDto userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            if (user == null)
            {
                throw new ArgumentException("Invalid credentials.");
            }

            bool isAuthenticated = await _userManager.CheckPasswordAsync(user, userDTO.password);
            if (!isAuthenticated)
            {
                throw new Exception("Invalid Credentials");
            }
            var token = await GenerateJWT(user);
            return new AuthDto() { Token = token, Username = user.UserName };
        }

        public async Task<AuthDto> Register(UserDto userDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(userDTO.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("This email is already registered.");
            }

            var user = new ApplicationUser
            {
                UserName = userDTO.FullName,
                Email = userDTO.Email,
            };
            var result = await _userManager.CreateAsync(user, userDTO.password);
            if (!result.Succeeded)
            {
                // Collect all identity errors and send them as a single message
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ArgumentException(string.Join(" | ", errors));
            }
            var token = await GenerateJWT(user);
            return new AuthDto() { Token = token, Username = user.UserName };
        }
        public async Task<string> GenerateJWT(ApplicationUser user)
        {
            var keyFromConfig = _config.GetValue<string>("Jwt:Key")!;
            var keyInBytes = Encoding.ASCII.GetBytes(keyFromConfig);
            var key = new SymmetricSecurityKey(keyInBytes);

            var signingCredentials = new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256Signature);

            var expiryDateTime = DateTime.Now.AddDays(7);

            var userClaims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var jwt = new JwtSecurityToken(
                claims: userClaims,
                expires: expiryDateTime,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}
