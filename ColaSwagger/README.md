# Cola.ColaMiddleware 中间件框架

[![Version](https://flat.badgen.net/nuget/v/Cola.ColaMiddleware?label=version)](https://github.com/odinGitGmail/Cola.ColaMiddleware) [![download](https://flat.badgen.net/nuget/dt/Cola.ColaMiddleware)](https://www.nuget.org/packages/Cola.ColaMiddleware) [![commit](https://flat.badgen.net/github/last-commit/odinGitGmail/Cola.ColaMiddleware)](https://flat.badgen.net/github/last-commit/odinGitGmail/Cola.ColaMiddleware) [![Blog](https://flat.badgen.net/static/blog/odinsam.com)](https://odinsam.com)

#### config 配置
```json
{
  "ColaSwagger": {
    // 启用xml注释
    "EnableXmlComment": true,
    // 启用jwt
    "EnableJWT": true,
    "Version": "Ver:1.0.0",
    "Title": "Cola Swagger",
    "Description": "Cola Swagger Description",
    "OpenApiContact": {
      "Name": "OdinSam",
      "Url": "https://odinsam.com",
      "Email": "odinsam.cn@gmail.com"
    },
    "OpenApiLicense": {
      "Name": "License",
      "Url": "https://odinsam.com"
    }
  }
}
```
#### 使用 ColaMiddleware 中间件
```csharp
builder.Services.AddColaSwagger(config);
app.UseColaSwagger("/swagger/v1/swagger.json", "WebApi V1",null);
```


#### 注意: 如果使用xml注释需要修改 csproj 配置，添加允许自动生成xml
```xml
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

#### 普通注释
```csharp
/// <summary>
/// 这是一个例子
/// </summary>
/// <remarks>
/// 描述：这是一个带参数的GET请求
/// Web API
/// </remarks>
/// <param name="id">主键ID</param>
/// <returns>测试字符串</returns>
[HttpGet("{id}")]
public ActionResult<string> Get(int id) {
     return $"你请求的ID是：{id}";
}
```

#### 当入参/出参返回object或者匿名类时
```csharp
/// <summary>
/// 这是一个例子
/// </summary>
/// <remarks>
/// 描述：这是一个带参数的GET请求
/// Web API
/// </remarks>
/// <param name="id">主键ID</param>
/// <returns>测试字符串</returns>
[HttpGet("{id}")]
[ProducesResponseType(typeof(xxx),200)]
public Object[] Get(int id) {
     return $"你请求的ID是：{id}";
}
```

#### 隐藏接口, 有接口但是不想让别人看到
```csharp
/// <summary>
/// 这是一个例子
/// </summary>
/// <remarks>
/// 描述：这是一个带参数的GET请求
/// Web API
/// </remarks>
/// <param name="id">主键ID</param>
/// <returns>测试字符串</returns>
[HttpGet("{id}")]
[ApiExplorerSettings(IgnoreApi =true)]
public ActionResult<string> Get(int id) {
     return $"你请求的ID是：{id}";
}
```