using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FilmReview
{
    public class JWTOptions
    {
        public string SecKey { get; set; }
        public int ExpireSeconds { get; set; }

        //封装jwt builder
        public static string BuildToken(List<Claim> claims, IOptionsSnapshot<JWTOptions> jWTOptions)
        {
            string key = jWTOptions.Value.SecKey;
            DateTime expire = DateTime.Now.AddSeconds(jWTOptions.Value.ExpireSeconds);
            byte[] secBytes = Encoding.UTF8.GetBytes(key);
            var secKey = new SymmetricSecurityKey(secBytes);
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(claims:claims,expires:expire,signingCredentials:credentials);
            string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return jwt;
        }
    }

    
   
}
