using ProductionHouse.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Core.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<CurrentUserDto> GetCurrentUserAsync(ClaimsPrincipal user);

        Task ChangePasswordAsync(int userId, ChangePasswordDto dto);

    }
}
