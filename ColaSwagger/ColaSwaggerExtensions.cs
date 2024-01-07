using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Routing;

namespace Cola.ColaMiddleware.ColaSwagger;

public static class ColaSwaggerExtensions
{
    /// <summary>
    /// UseColaSwagger - Register the SwaggerUI middleware with optional setup action for DI-injected options
    /// </summary>
    /// <param name="app">Can be fully qualified or relative to the current host</param>
    /// <param name="url">Can be fully qualified or relative to the current host</param>
    /// <param name="name">The description that appears in the document selector drop-down</param>
    /// <param name="routePrefix">The swagger routePrefix,if change must edit launchSettings.json launchUrl node</param>
    /// <returns></returns>
    public static IApplicationBuilder UseColaSwagger(this IApplicationBuilder app, string url = "/swagger/v1/swagger.json", string name = "WebApi V1", string? routePrefix=null)
    {
        //启用Swagger中间件
        app.UseSwagger();
        //配置SwaggerUI
        app.UseSwaggerUI(u =>
            {
                u.SwaggerEndpoint(url, name);
                if (!string.IsNullOrEmpty(routePrefix))
                {
                    u.RoutePrefix = routePrefix;
                }
            });
        return app;
    }
}