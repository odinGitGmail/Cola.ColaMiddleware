using Microsoft.OpenApi.Models;

namespace Cola.ColaMiddleware.ColaSwagger;

/// <summary>
/// ColaSwaggerConfigOption - cola swagger model
/// </summary>
public class ColaSwaggerConfigOption
{
    public bool EnableJwt { get; set; } = true;
    /// <summary>
    /// ColaSwaggerConfigModels
    /// </summary>
    public List<ColaSwaggerConfigModel> ColaSwaggerConfigModels { get; set; }
}

/// <summary>
/// ColaSwaggerConfigModel
/// </summary>
public class ColaSwaggerConfigModel
{
    public string Name { get; set; } = "V1";
    public bool EnableXmlComment { get; set; } = true;
    public string ViewModelNamespaceFullPath { get; set; } = "";
    public string Version { get; set; } = "Ver: 1.0.0";
    public string Title { get; set; } = "Cola Swagger";
    public string Description { get; set; } = "Cola Swagger Description";
    public OpenApiContact OpenApiContact { get; set; } = new OpenApiContact();
    public OpenApiLicense OpenApiLicense { get; set; } = new OpenApiLicense();
}