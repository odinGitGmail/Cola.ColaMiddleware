using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using Cola.ColaMiddleware.ColaIpRateLimit;
using Cola.Core.ColaConsole;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.ColaMiddleware.HealthChecks;

/// <summary>
/// ColaHealthChecks
/// </summary>
public static class ColaHealthChecksInject
{
    public static IServiceCollection AddColaHealthChecks(
        this IServiceCollection services)
    {
        services.AddHealthChecks();
        return services;
    }
}