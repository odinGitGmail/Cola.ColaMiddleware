using AspNetCoreRateLimit;

namespace Cola.ColaMiddleware.ColaIpRateLimit;
/// <summary>
/// ColaIpRateLimitOptions cola ip rate limit model
/// </summary>
public class ColaIpRateLimitOptions : IpRateLimitOptions
{
    public string IpRateLimitCache { get; set; } = "Memory";
}