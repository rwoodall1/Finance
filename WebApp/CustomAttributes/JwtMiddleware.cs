using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using ApiBindingModels;
namespace WebApp.CustomAttribute
{
    //https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
      //  private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
           // _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null && token !="null")
                attachUserToContext(context,token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context,string token)
        {
            //https://stackoverflow.com/questions/67663848/idx10503-signature-validation-failed-token-does-not-have-a-kid-keys-tried-s
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(ApplicationConfig.APISecretKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var _user = new LoggedInUser()
                {
                    UserName = jwtToken.Claims.First(x => x.Type == "UserName").Value,
                    FirstName = jwtToken.Claims.First(x => x.Type == "FirstName").Value,
                    LastName = jwtToken.Claims.First(x => x.Type == "LastName").Value,
                    EmailAddress = jwtToken.Claims.First(x => x.Type == "EmailAddress").Value,
                    Role = jwtToken.Claims.First(x => x.Type == "Role").Value,
                    Rank = int.Parse(jwtToken.Claims.First(x => x.Type == "Rank").Value),
                    Id = jwtToken.Claims.First(x => x.Type == "Id").Value,
                };
                context.Items["User"] = _user; 
            }
          catch(Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}