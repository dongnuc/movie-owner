using MediatR;
using Movie_StructureCode.Application.Abstractions.Services.Cache;
using Movie_StructureCode.Application.Common;
using Movie_StructureCode.Contract.Abstractions.Shared;

namespace Movie_StructureCode.Application.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ICacheDataService _redis;

        public CachingBehavior(ICacheDataService cacheDataService)
        {
            _redis = cacheDataService;
        }

        private static TResponse CreateResult<TResponse>(object value, Type valueType)
        {
            var resultType = typeof(Result<>).MakeGenericType(valueType);

            var method = typeof(Result)
                .GetMethods()
                .First(x => x.Name == "Success" && x.IsGenericMethod)
                .MakeGenericMethod(valueType);

            var result = method.Invoke(null, new[] { value });

            return (TResponse)result;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is not ICacheableQuery cacheable)
                return await next();

            if (cacheable.BypassCache)
                return await next();

            var getMethod = typeof(ICacheDataService)
                .GetMethod(nameof(ICacheDataService.GetAsync))
                .MakeGenericMethod(cacheable.ValueType);

            var cachedTask = (Task)getMethod.Invoke(_redis, new object[] { cacheable.CacheKey });
            await cachedTask;

            var cachedValue = cachedTask.GetType().GetProperty("Result")?.GetValue(cachedTask);

            if (cachedValue != null)
            {
                return CreateResult<TResponse>(cachedValue, cacheable.ValueType);
            }

            var response = await next();

            if (response is Result result && result.IsSuccess)
            {
                var value = response.GetType().GetProperty("Value")?.GetValue(response);

                if (value != null)
                {
                    var setMethod = typeof(ICacheDataService)
                        .GetMethod(nameof(ICacheDataService.SetAsync))
                        .MakeGenericMethod(cacheable.ValueType);

                    var setTask = (Task)setMethod.Invoke(_redis,
                        new object[] { cacheable.CacheKey, value, cacheable.Expiration });

                    await setTask;
                }
            }

            return response;
        }
    }
}
