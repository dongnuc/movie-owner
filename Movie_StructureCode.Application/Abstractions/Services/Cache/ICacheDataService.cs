namespace Movie_StructureCode.Application.Abstractions.Services.Cache
{
    public interface ICacheDataService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration);
        Task RemoveAsync(string key);
    }
}
