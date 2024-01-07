using System.Reflection;
using Cola.Core.Utils.Constants;
using Cola.CoreUtils.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Cola.ColaMiddleware.ColaSwagger;

public static class ColaSwaggerInject
{
    public static IServiceCollection AddSingletonColaCache(
        this IServiceCollection services,
        Action<ColaSwaggerConfigOption> action)
    {
        var colaSwaggerConfig = new ColaSwaggerConfigOption();
        action(colaSwaggerConfig);
        return InjectColaSwagger(services, colaSwaggerConfig);
        
    }
    
    public static IServiceCollection AddColaSwagger(this IServiceCollection services, IConfiguration config)
    {
        var colaSwaggerConfig = config.GetSection(SystemConstant.CONSTANT_COLASWAGGER_SECTION).Get<ColaSwaggerConfigOption>();
        colaSwaggerConfig = colaSwaggerConfig ?? new ColaSwaggerConfigOption();
        return InjectColaSwagger(services, colaSwaggerConfig);
    }

    private static IServiceCollection InjectColaSwagger(IServiceCollection services, ColaSwaggerConfigOption colaSwaggerConfigOption)
    {
        //注册Swagger
        return services.AddSwaggerGen(u =>
        {
            u.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Version = colaSwaggerConfigOption.Version,
                Title = colaSwaggerConfigOption.Title,
                Description = colaSwaggerConfigOption.Description,
                Contact = new OpenApiContact
                {
                    Name = colaSwaggerConfigOption.OpenApiContact.Name,
                    Url = colaSwaggerConfigOption.OpenApiContact.Url,
                    Email = colaSwaggerConfigOption.OpenApiContact.Email
                },
                License = new OpenApiLicense()
                {
                    Name = colaSwaggerConfigOption.OpenApiLicense.Name,
                    Url = colaSwaggerConfigOption.OpenApiLicense.Url,
                }
            });
            
            #region 读取xml信息

            if (colaSwaggerConfigOption.EnableXmlComment)
            {
                // 使用反射获取xml文件，并构造出文件的路径
                var xmlFile = $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    // 启用xml注释，该方法第二个参数启用控制器的注释，默认为false.
                    u.IncludeXmlComments(xmlPath, true);
                }

                if (colaSwaggerConfigOption.ViewModelNamespaceFullPath.StringIsNotNullOrEmpty())
                {
                    // 使用反射获取xml文件，并构造出文件的路径
                    var modelXmlFile = $"{colaSwaggerConfigOption.ViewModelNamespaceFullPath}.xml";
                    var modelXmlPath = Path.Combine(AppContext.BaseDirectory, modelXmlFile);
                    if (File.Exists(modelXmlPath))
                    {
                        // 启用xml注释，该方法第二个参数启用控制器的注释，默认为false.
                        u.IncludeXmlComments(modelXmlPath, true);
                    }
                }
            }
            
            #endregion
            
            #region 开启JWT

            if (colaSwaggerConfigOption.EnableJwt)
            {
                u.OperationFilter<SecurityRequirementsOperationFilter>();
                u.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
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
}