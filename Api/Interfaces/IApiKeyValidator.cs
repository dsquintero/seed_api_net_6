namespace Api.Interfaces
{
    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}