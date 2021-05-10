using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ranna_snippets.Authorization
{
    public class JwtApiTokenService : IApiTokenService
    {
        private static readonly string issuer = "ranna-snippets";
        private static readonly int keyLen = 32;
        private static readonly string securityAlgorithm = SecurityAlgorithms.HmacSha256Signature;

        private readonly TimeSpan tokenLifetime = TimeSpan.FromDays(365);
        private readonly JwtSecurityTokenHandler tokenHandler;
        private SymmetricSecurityKey signingKey;

        public JwtApiTokenService(IConfiguration _config)
        {
            tokenLifetime = TimeSpan.FromSeconds(
                _config.GetValue("Auth:ApiToken:Lifetime", tokenLifetime.TotalSeconds));

            signingKey = GenerateSigningKey(keyLen);
            tokenHandler = new JwtSecurityTokenHandler();
        }

        public string Generate<T>(T identity) where T : AuthClaims
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, identity.UserUid.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, identity.UserName));

            var credentials = new SigningCredentials(signingKey, securityAlgorithm);

            var now = DateTime.UtcNow;
            var deadline = now.Add(tokenLifetime);
            var token = tokenHandler.CreateJwtSecurityToken(
                  issuer: issuer,
                  subject: claims,
                  notBefore: now,
                  expires: deadline,
                  signingCredentials: credentials);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateAndRestore<T>(string key, out T identity) where T : AuthClaims
        {
            identity = null;

            var parameters = new TokenValidationParameters
            {
                IssuerSigningKey = signingKey,
                ValidIssuer = issuer,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
            };

            try
            {
                var principal = tokenHandler.ValidateToken(key, parameters, out var token);
                var now = DateTime.UtcNow;
                if (token.ValidFrom > now || token.ValidTo < now) return false;

                var claims = principal.Identities.First().Claims;
                var userId = claims?.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var userName = claims?.First(c => c.Type == ClaimTypes.Name)?.Value;

                if (userId == null || userName == null) return false;

                identity = new AuthClaims
                {
                    UserUid = Guid.Parse(userId),
                    UserName = userName
                } as T;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private SymmetricSecurityKey GenerateSigningKey(int len)
        {
            var bytes = new byte[len];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(bytes);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
