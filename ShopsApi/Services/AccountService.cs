using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopsApi.Entities;
using ShopsApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ShopsApi.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        public void DeleteUser(int userId);
        public string GenerateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ShopDbContext _context;
        private readonly AuthenticationSettings _authenticationSetting;

        public AccountService(ShopDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _passwordHasher = passwordHasher;
            _context = context;
            _authenticationSetting = authenticationSettings;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                RoleId = dto.RoleId,
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var user = _context
                .Users
                .FirstOrDefault(x => x.Id == userId);
            _context.Users.Remove(user);
            _context.SaveChanges();

        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _context.Users
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Email == dto.Email);
            if (user is null)
            {
                throw new Exception("Wrong email or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Wrong email or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSetting.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSetting.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSetting.JwtIssuer, _authenticationSetting.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

    }

}
