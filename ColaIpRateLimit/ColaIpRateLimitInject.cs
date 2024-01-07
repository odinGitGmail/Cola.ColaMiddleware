using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.ColaMiddleware.ColaIpRateLimit;

public static class ColaIpRateLimitInject
{
    public static IServiceCollection AddSingletonColaCache(
        this IServiceCollection services,
        Action<IpRateLimitOptions> action)
    {
        
        services.Configure(action);
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        return services;
    }
    
    public static IServiceCollection AddSingletonColaCache(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<IpRateLimitOptions>(config.GetSection("IpRateLimit"));
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        return services;
    }
}