using Movie_StructureCode.API.DependencyInjection.Extensions;

namespace Movie_StructureCode.API.DependencyInjection.Extensions
{
    /// <summary>
    /// API layer dependency injection extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Thêm t?t c? API services vào DI container
        /// </summary>
        public static IServiceCollection AddConfigureApi(this IServiceCollection services)
        {
            // HttpContextAccessor - c?n thi?t ?? truy c?p HttpContext trong handlers
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
