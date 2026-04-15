namespace Movie_StructureCode.Application.Common
{
    public interface ICacheableQuery
    {
        string CacheKey { get; }
        TimeSpan? Expiration { get; }
        bool BypassCache { get; }

        Type ValueType { get; }
    }
}
