using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ranna_snippets.Authorization;
using ranna_snippets.Database;
using ranna_snippets.Models;
using ranna_snippets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ranna_snippets.Controlelrs
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [TypeFilter(typeof(ExtractApiTokenFilter))]
    public class SnippetsController : ControllerBase
    {
        private readonly IContext db;

        public SnippetsController(IContext _db)
        {
            db = _db;
        }

        [HttpPost]
        public async Task<ActionResult<Snippet>> Post([FromBody] SnippetRequest snippetIn)
        {
            var snippet = new Snippet()
            {
                Language = snippetIn.Language,
                Code = snippetIn.Code,
                Ident = RandomUtil.GetString(6),
            };

            if (HttpContext.Items.TryGetValue("user", out var userObject))
                snippet.Owner = userObject as User;

            db.Snippets.Add(snippet.Encode());
            await db.SaveChangesAsync();

            return snippet.Decode();
        }

        [HttpGet("{ident}")]
        public async Task<ActionResult<Snippet>> Get([FromRoute] string ident)
        {
            var isGuid = Guid.TryParse(ident, out var guid);
            var snippet = await db.Snippets
                .Where(s => s.Ident == ident || isGuid && s.Id == guid)
                .Include(s => s.Owner)
                .FirstOrDefaultAsync();

            if (snippet == null) 
                return NotFound(new ErrorModel()
                {
                    Code = 404,
                    Message = "Not Found"
                });
            return snippet.Decode();
        }

        [HttpGet]
        public async Task<ActionResult<List<SnippetListEntry>>> List()
        {
            if (!HttpContext.Items.TryGetValue("user", out var userObject))
                return Unauthorized();

            var user = userObject as User;

            var snippets = await db.Snippets
                .Where(s => s.Owner.Id == user.Id)
                .Select(s => new SnippetListEntry(s))
                .ToListAsync();

            return snippets;
        }

        [HttpDelete("{ident}")]
        public async Task<ActionResult<Snippet>> Delete([FromRoute] string ident)
        {
            if (!HttpContext.Items.TryGetValue("user", out var userObject))
                return Unauthorized();

            var user = userObject as User;

            var isGuid = Guid.TryParse(ident, out var guid);
            var snippet = await db.Snippets
                .Where(s => s.Owner.Id == user.Id && s.Ident == ident || isGuid && s.Id == guid)
                .FirstOrDefaultAsync();

            if (snippet == null)
                return NotFound(new ErrorModel()
                {
                    Code = 404,
                    Message = "Not Found"
                });

            db.Remove(snippet);
            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}
