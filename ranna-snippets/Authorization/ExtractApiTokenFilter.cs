using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ranna_snippets.Database;
using ranna_snippets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ranna_snippets.Authorization
{
    public class ExtractApiTokenFilter : ActionFilterAttribute
    {
        private readonly IApiTokenService apiToken;
        private readonly IContext db;

        public ExtractApiTokenFilter(IApiTokenService _apiToken, IContext _db)
        {
            apiToken = _apiToken;
            db = _db;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as ControllerBase;

            if (context.HttpContext.TryGetAuthorizationToken(out var token, "bearer"))
            {
                if (!apiToken.ValidateAndRestore(token, out AuthClaims identity)) 
                {
                    context.Result = controller.Unauthorized();
                    return;
                }

                var user = await db.Users.FindAsync(identity.UserUid);
                if (user == null)
                {
                    context.Result = controller.Unauthorized();
                    return;
                }

                context.HttpContext.Items.Add("user", user);
            }
            
            await next();
        }
    }
}
