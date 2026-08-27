using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS_Core.Helpers
{
    public static class JWTAuthTokenGenerator
    {
        public static string GenerateToken(List<Claim> cliams, IOptions<JwtConfig> _jwtConfig)
        {
            var jwt = _jwtConfig.Value;
            var issuer = jwt.issuer;
            var audi = jwt.audience;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var jwtToken = new JwtSecurityToken(
               signingCredentials: cred,
               issuer: issuer,
               claims: cliams,
               audience: audi,
               expires: DateTime.UtcNow.AddHours(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
