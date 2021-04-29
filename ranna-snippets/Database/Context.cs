using Microsoft.EntityFrameworkCore;
using ranna_snippets.Models;

namespace ranna_snippets.Database
{
    public class Context : DbContext, IContext
    {
        public DbSet<Snippet> Snippets { get; set; }

        public Context(DbContextOptions options) : base(options)
        {
        }
    }
}
