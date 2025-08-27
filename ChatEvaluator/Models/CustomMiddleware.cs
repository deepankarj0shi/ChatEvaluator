using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatEvaluator.Models
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public CustomMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("Authorization", out StringValues auth))
            {
                var tokenValue = auth[0].Replace("Bearer ", "");
                if (ValidateToken(tokenValue, out JwtSecurityToken jwt))
                {
                    var identity = new ClaimsIdentity(jwt.Claims, "Custom");
                    httpContext.User = new ClaimsPrincipal(identity);
                }
            }
            await _next(httpContext);
        }
        public bool ValidateToken(string token, out JwtSecurityToken jwt)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["ApiSettings:JwtOptions:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["ApiSettings:JwtOptions:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ApiSettings:JwtOptions:Secret"])),
                ValidateLifetime = true
            };
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                jwt = (JwtSecurityToken)validatedToken;

                return true;
            }
            catch (SecurityTokenValidationException)
            {
                jwt = null;
                return false;
            }
        }

    }
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }
}
