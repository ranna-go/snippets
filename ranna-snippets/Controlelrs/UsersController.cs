using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ranna_snippets.Authorization;
using ranna_snippets.Database;
using ranna_snippets.Extensions;
using ranna_snippets.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ranna_snippets.Controlelrs
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static readonly Regex UserNameRX = new Regex(@"^[a-z0-9\-_]{2,32}$");

        private readonly IContext db;
        private readonly IMasterTokenService masterTokens;
        private readonly IApiTokenService apiTokens;

        public UsersController(IContext _db, 
                               IMasterTokenService _masterTokens, 
                               IApiTokenService _apiTokens)
        {
            db = _db;
            masterTokens = _masterTokens;
            apiTokens = _apiTokens;
        }

        [HttpPut("{username}")]
        public async Task<ActionResult<UserCreateResponse>> Create([FromRoute] string username)
        {
            if (!UserNameRX.IsMatch(username))
                return BadRequest("invalid username pattern");

            if (await db.Users.CountAsync(u => u.Username == username) > 0)
                return BadRequest("username already exists");

            var (token, hash) = masterTokens.Generate();

            var user = new User()
            {
                Username = username,
                MasterKeyHash = hash,
            };

            db.Add(user);
            await db.SaveChangesAsync();

            var res = new UserCreateResponse()
            {
                Id = user.Id,
                TimeStamp = user.TimeStamp,
                Username = user.Username,
                MasterKey = token,
            };

            return Ok(res);
        }

        [HttpDelete("{username}")]
        public async Task<ActionResult> Delete([FromRoute] string username)
        {
            if (!HttpContext.TryGetAuthorizationToken(out var token, "bearer"))
                return Unauthorized();

            var user = await masterTokens.ValidateAsync(username, token);
            if (user == null) return Unauthorized();

            var userLinks = await db.Snippets
                .Where(s => s.Owner.Id == user.Id)
                .ToListAsync();

            db.RemoveRange(userLinks);
            db.Remove(user);
            await db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{username}/apitoken")]
        public async Task<ActionResult> ApiToken([FromRoute] string username)
        {
            if (!HttpContext.TryGetAuthorizationToken(out var token, "bearer"))
                return Unauthorized();

            var user = await masterTokens.ValidateAsync(username, token);
            if (user == null) return Unauthorized();

            user.ApiKey = apiTokens.Generate(new AuthClaims()
            {
                UserName = username,
                UserUid = user.Id,
            });

            db.Update(user);
            await db.SaveChangesAsync();

            return Ok(new TokenResponse() { Token = user.ApiKey });
        }
    }
}
