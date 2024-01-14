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
    /// <param name="urlAndName">Can be fully qualified or relative to the current host</param>
    /// <returns></returns>
    public static IApplicationBuilder UseColaSwagger(this IApplicationBuilder app, Dictionary<string,string> urlAndName)
    {
        app.UseSwagger();
            
        var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
        app.UseSwaggerUI(c =>
        {
            if (provider != null)
            {
                foreach (var item in provider.ApiVersionDescriptions) 
                {
                    c.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.GroupName);
                }
            }
            else
            {
                foreach (var item in urlAndName.Keys)
                {
                    c.SwaggerEndpoint(item, urlAndName[item]);
                }
            }
            
        });
        return app;
    }
}