using Isopoh.Cryptography.Argon2;
using System.Text;

namespace ranna_snippets.Authorization
{
    public class Argon2Hasher : IHasher
    {
        public string Create(string password) =>
            Argon2.Hash(password);

        public bool Validate(string password, string hash) =>
            Argon2.Verify(hash, password);
    }
}
