using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Exceptions;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Infrastructure.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;


        public AuthService(
            AppDbContext context,
            IJwtService jwtService,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {

            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == dto.Email );

            if (admin == null)
                throw new UnauthorizedAccessException("Invalid email or password.");
            

            var hasher = new PasswordHasher<Admin>();
            
            var result = hasher.VerifyHashedPassword(
                admin, admin.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var token = _jwtService.GenerateToken(admin);
            return new LoginResponseDto
            {
                Id = admin.Id,
                Name = admin.Name,
                Email = admin.Email,
                Role = admin.Role,
                Token = token
            };
        }

        public Task<CurrentUserDto> GetCurrentUserAsync(
        ClaimsPrincipal user)
        {
            var dto = new CurrentUserDto
            {
                Id = int.Parse(
                    user.FindFirst(ClaimTypes.NameIdentifier)!.Value),

                Name = user.FindFirst(ClaimTypes.Name)!.Value,

                Email = user.FindFirst(ClaimTypes.Email)!.Value,

                Role = user.FindFirst(ClaimTypes.Role)!.Value
            };

            return Task.FromResult(dto);
        }
        public async Task ChangePasswordAsync(
    int userId,
    ChangePasswordDto dto)
        {
            var admin =
                await _unitOfWork.Admins.GetByIdAsync(userId);

            if (admin == null)
                throw new NotFoundException("Admin not found.");

            var hasher =
                new PasswordHasher<Admin>();

            var result =
                hasher.VerifyHashedPassword(
                    admin,
                    admin.PasswordHash,
                    dto.OldPassword);

            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Old password is incorrect.");

            admin.PasswordHash =
                hasher.HashPassword(admin, dto.NewPassword);

            _unitOfWork.Admins.Update(admin);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
