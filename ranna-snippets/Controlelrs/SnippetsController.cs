using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ranna_snippets.Database;
using ranna_snippets.Models;
using ranna_snippets.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ranna_snippets.Controlelrs
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class SnippetsController : ControllerBase
    {
        private readonly IContext db;

        public SnippetsController(IContext _db)
        {
            db = _db;
        }

        [HttpGet("{ident}")]
        public async Task<ActionResult<Snippet>> Get([FromRoute] string ident)
        {
            var isGuid = Guid.TryParse(ident, out var guid);
            var snippet = await db.Snippets.Where(s => s.Ident == ident || isGuid && s.Id == guid).FirstOrDefaultAsync();

            if (snippet == null) return NotFound();
            return snippet.Decode();
        }

        [HttpPost]
        public async Task<ActionResult<Snippet>> Post([FromBody] Snippet snippet)
        {
            if (!snippet.Validate(out var errMsg)) 
                return BadRequest(errMsg);

            snippet.TimeStamp = DateTimeOffset.Now;
            snippet.Id = Guid.NewGuid();
            snippet.Ident = RandomUtil.GetString(6);

            db.Snippets.Add(snippet.Encode());
            await db.SaveChangesAsync();

            return snippet.Decode();
        }
    }
}
