using Cola.ColaMiddleware.ColaMiddle.SysUser;
using Cola.Core.ColaConsole;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.ColaMiddleware.ColaMiddle;

public static class AddColaModulesInject
{
    /// <summary>
    /// cola 常用组件注入
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddColaModules(this IServiceCollection services)
    {
        // 注入 HttpContext 
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        ConsoleHelper.WriteInfo("AddColaModules 注入HttpContext上下文 【 IHttpContextAccessor, HttpContextAccessor 】");
        
        services.AddScoped<ISysCurrentUser, SysCurrentUser>();
        ConsoleHelper.WriteInfo("AddColaModules SysCurrentUser 【 ISysCurrentUser, SysCurrentUser 】");
        return services;
    }
}