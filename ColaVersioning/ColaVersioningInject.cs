using Cola.Core.ColaConsole;
using Cola.Core.Utils.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.ColaMiddleware.ColaVersioning;

public static class ColaVersioningInject
{
    public static IServiceCollection AddColaVersioning(
        this IServiceCollection services,
        Action<ColaVersioningOption> action)
    {
        var colaVersioningOption = new ColaVersioningOption();
        action(colaVersioningOption);
        services = InjectColaVersion(services, colaVersioningOption);
        ConsoleHelper.WriteInfo("ColaVersioning 注入");
        return services;
    }

    public static IServiceCollection AddColaVersioning(
        this IServiceCollection services,
        IConfiguration config)
    {
        var colaVersioningOption = config.GetSection(SystemConstant.CONSTANT_COLAVERSIONING_SECTION)
            .Get<ColaVersioningOption>();
        colaVersioningOption ??= new ColaVersioningOption();
        services = InjectColaVersion(services, colaVersioningOption);
        ConsoleHelper.WriteInfo("ColaVersioning 注入");
        return services;
    }

    private static IServiceCollection InjectColaVersion(IServiceCollection services,
        ColaVersioningOption colaVersioningOption)
    {
        //注册Swagger
        return services
            .AddApiVersioning(o =>
            {
                // 如果设置为true, 在Api请求的响应头部，会追加当前Api支持的版本     api-supported-versions: 1.0
                o.ReportApiVersions = colaVersioningOption.ReportApiVersions;
                // 标记当客户端没有指定版本号的时候，是否使用默认版本号 
                o.AssumeDefaultVersionWhenUnspecified = colaVersioningOption.AssumeDefaultVersionWhenUnspecified;
                // 默认版本号
                o.DefaultApiVersion =
                    new ApiVersion(colaVersioningOption.MajorVersion, colaVersioningOption.MinorVersion);
                // 同时支持 请求头(HTTP Header)中使用版本控制 和 查询字符串中指定版本号 2种方式
                // 路由指定版本  [Route(“api/{v:apiVersion}/Values”)]
                // 查询字符串中指定版本号 /api/values?api-version=1.0
                // 请求头(HTTP Header)中使用版本控制  x-api-version = 1.0  x-api-version可自定义
                o.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(),
                    new HeaderApiVersionReader() { HeaderNames = { colaVersioningOption.HttpHeaderName } });
            })
            .AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });
    }
}