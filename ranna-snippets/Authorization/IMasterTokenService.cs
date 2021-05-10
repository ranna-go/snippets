using ranna_snippets.Models;
using System.Threading.Tasks;

namespace ranna_snippets.Authorization
{
    public interface IMasterTokenService
    {
        (string Token, string Hash) Generate();
        Task<User> ValidateAsync(string username, string token);
    }
}
