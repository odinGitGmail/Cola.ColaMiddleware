using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.ColaMiddleware.ColaSwagger;
/// <summary>
/// ColaSwaggerMiddlewareExtensions - cola swagger middleware
/// </summary>
public static class ColaSwaggerMiddlewareExtensions
{
    /// <summary>
    /// UseColaSwagger - Register the SwaggerUI middleware with optional setup action for DI-injected options
    /// </summary>
    /// <param name="app">Can be fully qualified or relative to the current host</param>
    /// <param name="url">Can be fully qualified or relative to the current host</param>
    /// <param name="name">The description that appears in the document selector drop-down</param>
    /// <param name="routePrefix">The swagger routePrefix,if change must edit launchSettings.json launchUrl node</param>
    /// <returns></returns>
    public static IApplicationBuilder UseColaSwagger(this IApplicationBuilder app, Dictionary<string,string> urlAndName)
    {
        app.UseSwagger();
            
        var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwaggerUI(c =>
        {
            foreach (var item in provider.ApiVersionDescriptions) 
            {
                c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.GroupName);
            }
        });
        return app;
    }
}