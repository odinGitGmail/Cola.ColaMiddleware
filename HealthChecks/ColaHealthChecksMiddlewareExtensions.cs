using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Cola.ColaMiddleware.HealthChecks;

public static class ColaHealthChecksMiddlewareExtensions
{
    /// <summary>
    /// ColaHealthChecks - Register the HealthChecks
    /// </summary>
    /// <param name="app">Can be fully qualified or relative to the current host</param>
    /// <returns></returns>
    public static IApplicationBuilder UseColaHealthChecks(this WebApplication app, string healthPath)
    {
        app.MapHealthChecks(healthPath, new HealthCheckOptions
        {
            ResponseWriter = HealthWrite,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });
        return app;
    }

    private static Task HealthWrite(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json";
        var result = JsonConvert.SerializeObject(new
        {
            status = healthReport.Status.ToString(),
            error = healthReport.Entries.Select(e=>new
            {
                key = e.Key,
                value =e.Value
            })
        });
        return context.Response.WriteAsync(result);
    }
}