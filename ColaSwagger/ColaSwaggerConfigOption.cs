using Microsoft.OpenApi.Models;

namespace Cola.ColaMiddleware.ColaSwagger;

public class ColaSwaggerConfigOption
{
    public bool EnableXmlComment { get; set; } = true;
    public bool EnableJwt { get; set; } = true;
    public string ViewModelNamespaceFullPath { get; set; } = "";
    public string Version { get; set; } = "Ver: 1.0.0";
    public string Title { get; set; } = "Cola Swagger";
    public string Description { get; set; } = "Cola Swagger Description";
    public OpenApiContact OpenApiContact { get; set; } = new OpenApiContact();
    public OpenApiLicense OpenApiLicense { get; set; } = new OpenApiLicense();
}