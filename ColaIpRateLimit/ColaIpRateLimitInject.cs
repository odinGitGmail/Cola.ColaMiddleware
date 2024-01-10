using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using Cola.Core.Utils.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.ColaMiddleware.ColaIpRateLimit;
/// <summary>
/// ColaIpRateLimitInject - cola ip rete limit inject
/// </summary>
public static class ColaIpRateLimitInject
{
    public static IServiceCollection AddColaIpRateLimit(
        this IServiceCollection services,
        Action<ColaIpRateLimitOptions> action)
    {
        var colaIpRateLimitOptions = new ColaIpRateLimitOptions();
        action(colaIpRateLimitOptions);
        if (string.Compare(colaIpRateLimitOptions.IpRateLimitCache, "Memory", StringComparison.OrdinalIgnoreCase) == 0)
        {
            services.AddMemoryCache();
        }

        if (string.Compare(colaIpRateLimitOptions.IpRateLimitCache, "Redis", StringComparison.OrdinalIgnoreCase) == 0)
        {
            services.AddRedisRateLimiting();
        }
        services.Configure(action);
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        return services;
    }
    
    public static IServiceCollection AddColaIpRateLimit(this IServiceCollection services, IConfiguration config)
    {
        
        var colaIpRateLimitOptions = config.GetSection(SystemConstant.CONSTANT_COLAIPRATELIMIT_SECTION);
        var cache = colaIpRateLimitOptions.GetSection("IpRateLimitCache").Get<string>();
       
        if (string.Compare(cache, "Memory", StringComparison.OrdinalIgnoreCase) == 0)
        {
            services.AddMemoryCache();
        }

        if (string.Compare(cache, "Redis", StringComparison.OrdinalIgnoreCase) == 0)
        {
            services.AddRedisRateLimiting();
        }
        services.AddDistributedMemoryCache();
        services.Configure<IpRateLimitOptions>(config.GetSection(SystemConstant.CONSTANT_COLAIPRATELIMIT_SECTION));
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        return services;
    }
}