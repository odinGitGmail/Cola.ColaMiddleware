using AspNetCoreRateLimit;

namespace Cola.ColaMiddleware.ColaIpRateLimit;

public class ColaIpRateLimitOptions : IpRateLimitOptions
{
    public string IpRateLimitCache { get; set; } = "Memory";
}