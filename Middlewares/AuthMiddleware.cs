using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Primitives;

namespace authmodule.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Authorization header missing.");
                return;
            }

            var token = authHeader.FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrWhiteSpace(token) || !ValidateJwtToken(token))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid or missing token.");
                return;
            }

            // Token is valid, pass the request to the next middleware
            await _next(context);
        }

        private bool ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Validate the token claims or perform further validation as needed
                var expiration = jwtToken.ValidTo;
                if (expiration < DateTime.UtcNow)
                {
                    return false; // Token expired
                }

                return true; // Token is valid
            }
            catch (Exception)
            {
                return false; // Invalid token format or validation failed
            }
        }
    }   
}