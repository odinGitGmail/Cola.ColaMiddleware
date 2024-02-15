using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cola.ColaMiddleware.ColaSwagger.Filters;

/// <summary>
/// HttpMethodAttribute 或 RouteAttribute 重新定义可选 RouteParameter 参数
/// </summary>
/// <typeparam name="T">HttpMethodAttribute 或者 RouteParameter</typeparam>
public class ReApplyOptionalRouteParameterOperationFilter<T> : IOperationFilter where T : IRouteTemplateProvider
{
    private const string CaptureName = "routeParameter";
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var httpMethodAttributes = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<T>();
        var httpMethodWithOptional = httpMethodAttributes.FirstOrDefault(m => m.Template != null && m.Template.Contains("?"));
        if (httpMethodWithOptional == null)
            return;
        string regex = $"{{(?<{CaptureName}>\\w+)\\?}}";
        var matches = Regex.Matches(httpMethodWithOptional.Template!, regex);
        foreach (Match match in matches)
        {
            var name = match.Groups[CaptureName].Value;
            var parameter = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Path && p.Name == name);
            if (parameter != null)
            {
                parameter.AllowEmptyValue = true;
                parameter.Description =
                    "选择 \"Send empty value\" 可以发送空值，否则发送逗号 ,";
                parameter.Required = false;
                parameter.Schema.Nullable = true;
            }
        }
    }
}