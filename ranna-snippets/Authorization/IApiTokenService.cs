namespace ranna_snippets.Authorization
{
    public interface IApiTokenService
    {
        string Generate<T>(T identity) where T : AuthClaims;
        bool ValidateAndRestore<T>(string key, out T identity) where T : AuthClaims;
    }
}