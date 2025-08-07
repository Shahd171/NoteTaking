using Note_Taking_App.Core.DTOs;
using Note_Taking_App.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> GenerateJWT(ApplicationUser user);
        Task<AuthDto> Register(UserDto userDTO);
        Task<AuthDto> Login(LoginDto userDTO);
    }
}
