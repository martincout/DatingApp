using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private IConfiguration Config { get; }
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            Config = config;
        }


        public string CreateToken(AppUser user)
        {
            //Claims of the user, like its name, id, or something else
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username)
            };
            //The secret key
            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            //Describes the user with its claims, the expire date of the token and the key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = cred
            };
            //Handler
            var tokenHandler = new JwtSecurityTokenHandler();
            //Creates the token with the encrypted key
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //Send to user
            return tokenHandler.WriteToken(token);
        }
    }
}
