using BaseProjectDataContext;
using BaseProjectDataContext.Entity;
using BaseProjectDomain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BaseProjectDomain.Implementations
{
    public class JWTTokenService : IJWTTokenService
    {
        private readonly SqLiteContextUsers _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public JWTTokenService(
            SqLiteContextUsers context,
            IConfiguration configuration,
            UserManager<User> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }
        public string CreateToken(User user)
        {
            var roles = _userManager.GetRolesAsync(user).Result;
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("email", user.Email.ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("roles", role));
            }

            var jwtTokenSecretKey = _configuration.GetValue<string>("SecretPhrase");
            var singInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSecretKey));
            var singInCredentials = new SigningCredentials(singInKey, SecurityAlgorithms.HmacSha256);

            var JWT = new JwtSecurityToken(
                signingCredentials: singInCredentials,
                claims: claims,
                expires: DateTime.Now.AddDays(7)
                );

            return new JwtSecurityTokenHandler().WriteToken(JWT);
        }
    }
}
