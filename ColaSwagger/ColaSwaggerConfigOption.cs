using Microsoft.OpenApi.Models;

namespace Cola.ColaMiddleware.ColaSwagger;

/// <summary>
/// ColaSwaggerConfigOption - cola swagger model
/// </summary>
public class ColaSwaggerConfigOption
{
    public ColaSwaggerConfigOption()
    {
        
    }
    
    public ColaSwaggerConfigOption(List<ColaSwaggerConfigModel>? colaSwaggerConfigModels)
    {
        ColaSwaggerConfigModels = colaSwaggerConfigModels;
    }

    public bool EnableJwt { get; set; } = true;
    /// <summary>
    /// ColaSwaggerConfigModels
    /// </summary>
    public List<ColaSwaggerConfigModel>? ColaSwaggerConfigModels { get; set; }
}

