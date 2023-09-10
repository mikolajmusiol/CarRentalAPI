using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Dtos;
using CarRentalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRentalAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly CarRentalDbContext _context;
        private readonly IPasswordHasher<User> _hasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(CarRentalDbContext dbContext, IPasswordHasher<User> hasher, AuthenticationSettings autheticationSettings)
        {
            _context = dbContext;
            _hasher = hasher;
            _authenticationSettings = autheticationSettings;
        }

        public async Task RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Address = new Address()
                {
                    City = dto.City,
                    Street = dto.Street,
                    PostalCode = dto.PostalCode
                },
                RoleId = dto.RoleId
            };

            newUser.PasswordHash = _hasher.HashPassword(newUser, dto.Password);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GenerateJwt(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid useranme or password");
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid useranme or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString("yyyy-MM-dd")),
                new Claim(ClaimTypes.Role, user.Role.Name.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, null, expires, cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
