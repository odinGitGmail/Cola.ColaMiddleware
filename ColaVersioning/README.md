# Cola.ColaVersioning 中间件框架

#### config 配置
```json
{
  "ColaVersioning":{
    "ReportApiVersions": true,
    "AssumeDefaultVersionWhenUnspecified": true,
    "MajorVersion": 1,
    "MinorVersion": 0,
    "HttpHeaderName": "x-api-version"
  }
}
```
#### 使用 ColaVersioning 中间件
```csharp
builder.Configuration.AddJsonFile("appsettings.json");
var config = builder.Configuration;

// 注入ColaVersioning
builder.Services.AddColaVersioning(config);
```

#### 路由标致版本号
```csharp 
[ApiVersion("1.0")]
[Route("api/values")]
[ApiController]
public class ValuesV1Controller : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "Value1 from Version 1", "value2 from Version 1" };
    }
}   
```

#### 查询字符串标致版本号
```csharp 
https://localhost:44319/api/values?api-version=2.0 
```

#### 使用路由约束中指定请求Api的版本
```csharp 
[Route(“api/{v:apiVersion}/Values”)]
```

#### 请求头(HTTP Header)中使用版本控制
```csharp headers 添加 
key:x-api-version  value:1.0
```

#### 弃用Api(Deprecated)特性
```csharp headers 添加 
[ApiVersion("1.0", Deprecated = true)]
[Route("api/values")]
[ApiController]
public class ValuesV1Controller : ControllerBase
{
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "Value1 from Version 1", "value2 from Version 1" };
    }
}
```
此时，在请求是，请求头如下
```text
Response Header
api-deprecated-versions: 1.0    // 1.0版本过期
api-supported-versions: 2.0     // 2.0 版本支持
content-type: application/json; charset=utf-8
date: Sat, 06 Oct 2018 06:32:18 GMT
server: Kestrel
status: 200
x-powered-by: ASP.NET
```

#### 指定不需要版本的api
```csharp
[ApiVersionNeutral]
[Route("api/[controller]")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    public string Get()
    {
        return "Good";
    }
}
```
