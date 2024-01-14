﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cola.ColaMiddleware.ColaSwagger;

/// <summary>
/// SetVersionInPathDocumentFilter
/// </summary>
public class SetVersionInPathDocumentFilter: IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var updatedPaths = new OpenApiPaths();
        foreach (var entry in swaggerDoc.Paths)
        {
            updatedPaths.Add(
                entry.Key.Replace("v{version}", swaggerDoc.Info.Version),
                entry.Value);
        }
        swaggerDoc.Paths = updatedPaths;
    }
}