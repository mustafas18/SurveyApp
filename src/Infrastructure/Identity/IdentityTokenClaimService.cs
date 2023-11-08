using Core.Constants;
using Core.Dtos;
using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityTokenClaimService : ITokenClaimsService
    {
        private readonly UserManager<AppUser> _userManager;


        public IdentityTokenClaimService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<string>> GetUserRolesAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);
            List<string> result=new();
            foreach (var role in roles)
            {
                result.Add(role);
            }
            return result;
        }
        public async Task<string> GetTokenAsync(LoginInfoDto loginInfo)
        {
            var user = await _userManager.FindByNameAsync(loginInfo.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginInfo.Password))
                throw new Exception("UserName or Password is wrong.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(AuthorizationConstants.SecurityKey);

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);


            var tokenOptions = new JwtSecurityToken(
               issuer: AuthorizationConstants.ValidIssuer,
               audience: AuthorizationConstants.ValidAudience,
               claims: claims,
               expires: DateTime.Now.AddHours(Convert.ToDouble(AuthorizationConstants.ExpiryInHours)),
               signingCredentials: signingCredentials
               );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);


        }
    }

}
