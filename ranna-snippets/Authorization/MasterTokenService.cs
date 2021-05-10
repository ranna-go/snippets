using Microsoft.EntityFrameworkCore;
using ranna_snippets.Database;
using ranna_snippets.Models;
using ranna_snippets.Util;
using System.Linq;
using System.Threading.Tasks;

namespace ranna_snippets.Authorization
{
    public class MasterTokenService : IMasterTokenService
    {
        private readonly IContext db;
        private readonly IHasher hasher;

        public MasterTokenService(IContext _db, IHasher _hasher)
        {
            db = _db;
            hasher = _hasher;
        }

        public (string Token, string Hash) Generate()
        {
            var token = CryptoRandom.GetBase64String(128);
            var hash = hasher.Create(token);
            return (token, hash);
        }

        public async Task<User> ValidateAsync(string username, string token)
        {
            var user = await db.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
            if (user == null) return null;
            return hasher.Validate(token, user.MasterKeyHash) ? user : null;
        }
    }
}
