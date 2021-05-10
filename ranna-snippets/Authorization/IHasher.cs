namespace ranna_snippets.Authorization
{
    public interface IHasher
    {
        string Create(string password);
        bool Validate(string password, string hash);
    }
}
