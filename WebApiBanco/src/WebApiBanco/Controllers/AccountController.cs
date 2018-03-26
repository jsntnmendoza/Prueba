using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiBanco.Models;
using System.Security.Claims;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;

namespace WebApiBanco.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        [HttpPost]
        public string GetAuthToken(User user)
        {
            var existUser = UserStorage.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

            if (existUser != null)
            {
                var requestAt = DateTime.Now;
                var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
                var token = GenerateToken(existUser, expiresIn);

                return JsonConvert.SerializeObject(new
                {
                    stateCode = 1,
                    requertAt = requestAt,
                    expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds,
                    accessToken = token
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new { stateCode = -1, errors = "Username or password is invalid" });
            }
        }

        private string GenerateToken(User user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Username, "TokenAuth"),
                new[] {
                    new Claim("ID", user.ID.ToString())
                }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);
        }
    }

    public class User
    {
        public Guid ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }

    public static class UserStorage
    {
        public static List<User> Users { get; set; } = new List<User> {
            new User {ID=Guid.NewGuid(),Username="Operador1",Password = "oper" },
            new User {ID=Guid.NewGuid(),Username="Operador2",Password = "oper" },
            new User {ID=Guid.NewGuid(),Username="Administrador",Password = "admin" }
        };
    }
}