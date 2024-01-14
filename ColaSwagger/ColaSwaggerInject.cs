using System.Reflection;
using Cola.Core.ColaConsole;
using Cola.Core.Utils.Constants;
using Cola.CoreUtils.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cola.ColaMiddleware.ColaSwagger;
/// <summary>
/// ColaSwaggerInject - cola swagger inject
/// </summary>
public static class ColaSwaggerInject
{
    public static IServiceCollection AddColaSwagger(
        this IServiceCollection services,
        Action<ColaSwaggerConfigOption> action)
    {
        var colaSwaggerConfig = new ColaSwaggerConfigOption();
        action(colaSwaggerConfig);
        services = InjectColaSwagger(services, colaSwaggerConfig);
        ConsoleHelper.WriteInfo("ColaSwagger 注入");
        return services;
    }
    
    public static IServiceCollection AddColaSwagger(this IServiceCollection services, IConfiguration config)
    {
        var colaSwaggerConfig = config.GetColaSection<ColaSwaggerConfigOption>(SystemConstant.CONSTANT_COLASWAGGER_SECTION);
        services = InjectColaSwagger(services, colaSwaggerConfig);
        ConsoleHelper.WriteInfo("ColaSwagger 注入");
        return services;
    }

    private static IServiceCollection InjectColaSwagger(IServiceCollection services,
        ColaSwaggerConfigOption colaSwaggerConfigOptions)
    {
        var provider = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();
        if (provider != null)
        {
            // swagger
            services.AddSwaggerGen(c =>
            {
                // 排序方式
                c.OrderActionsBy(o => o.HttpMethod);
                foreach (var item in provider.ApiVersionDescriptions)
                {
                    var option =
                        colaSwaggerConfigOptions.ColaSwaggerConfigModels.Single(c => c.Version == item.GroupName);
                    c.SwaggerDoc(item.GroupName, new OpenApiInfo
                    {
                        Title = $"{option.Title} {item.GroupName}",
                        Description = option.Description,
                        Version = item.ApiVersion.MajorVersion + "." + item.ApiVersion.MinorVersion,
                        Contact = new OpenApiContact()
                        {
                            Name = option.OpenApiContact.Name,
                            Url = option.OpenApiContact.Url,
                            Email = option.OpenApiContact.Email
                        },
                        License = new OpenApiLicense
                        {
                            Name = option.OpenApiLicense.Name,
                            Url = option.OpenApiLicense.Url
                        }
                    });

                }

                // 重载方式
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                foreach (var name in Directory.GetFiles(AppContext.BaseDirectory, "*.*",
                             SearchOption.AllDirectories).Where(f => Path.GetExtension(f).ToLower() == ".xml"))
                {
                    c.IncludeXmlComments(name, includeControllerXmlComments: true);
                }

                //添加2个过滤器
                c.DocumentFilter<SetVersionInPathDocumentFilter>();

                #region 开启JWT

                if (colaSwaggerConfigOptions.EnableJwt)
                {
                    c.OperationFilter<SecurityRequirementsOperationFilter>();
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "JWT授权(数据将在请求头中进行传输)直接在下框中输入Bearer token (注意 Bearer 和 token 之间有一个空格)",
                        Name = "Authorization",
                        //jwt默认存放 Authorization 信息的位置（请求头中）
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                }

                #endregion
            });
        }
        else
        {
            // swagger
            services.AddSwaggerGen(c =>
            {
                var swaggerModel = colaSwaggerConfigOptions.ColaSwaggerConfigModels[0];
                // 排序方式
                c.SwaggerDoc(
                    swaggerModel.Version,
                    new OpenApiInfo
                    {
                        Title = $"{swaggerModel.Title}",
                        Description = swaggerModel.Description,
                        Version = swaggerModel.Version,
                        Contact = new OpenApiContact()
                        {
                            Name = swaggerModel.OpenApiContact.Name,
                            Url = swaggerModel.OpenApiContact.Url,
                            Email = swaggerModel.OpenApiContact.Email
                        },
                        License = new OpenApiLicense
                        {
                            Name = swaggerModel.OpenApiLicense.Name,
                            Url = swaggerModel.OpenApiLicense.Url
                        }
                    });

                // 重载方式
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                foreach (var name in Directory.GetFiles(AppContext.BaseDirectory, "*.*",
                             SearchOption.AllDirectories).Where(f => Path.GetExtension(f).ToLower() == ".xml"))
                {
                    c.IncludeXmlComments(name, includeControllerXmlComments: true);
                }

                //添加2个过滤器
                c.DocumentFilter<SetVersionInPathDocumentFilter>();

                #region 开启JWT

                if (colaSwaggerConfigOptions.EnableJwt)
                {
                    c.OperationFilter<SecurityRequirementsOperationFilter>();
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "JWT授权(数据将在请求头中进行传输)直接在下框中输入Bearer token (注意 Bearer 和 token 之间有一个空格)",
                        Name = "Authorization",
                        //jwt默认存放 Authorization 信息的位置（请求头中）
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                }

                #endregion
            });
        }

        return services;
    }
}