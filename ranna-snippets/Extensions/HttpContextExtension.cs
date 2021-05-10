using Microsoft.AspNetCore.Http;

namespace ranna_snippets.Extensions
{
    public static class HttpContextExtension
    {
        public static bool TryGetAuthorizationToken(this HttpContext ctx, out string token, string prefix = "")
        {
            token = null;

            var ok = ctx.Request.Headers.TryGetValue("authorization", out var authHeader);
            if (!ok) return false;

            token = authHeader.ToString();

            if (prefix.Length > 0 && !token.ToLower().StartsWith(prefix)) return false;
            if (prefix.Length > 0) token = token[(prefix.Length+1)..];

            return true;
        }
    }
}
