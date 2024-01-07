# Cola.ColaIpRateLimit 中间件框架

[![Version](https://flat.badgen.net/nuget/v/Cola.ColaIpRateLimit?label=version)](https://github.com/odinGitGmail/Cola.ColaIpRateLimit) [![download](https://flat.badgen.net/nuget/dt/Cola.ColaIpRateLimit)](https://www.nuget.org/packages/Cola.ColaIpRateLimit) [![commit](https://flat.badgen.net/github/last-commit/odinGitGmail/Cola.ColaIpRateLimit)](https://flat.badgen.net/github/last-commit/odinGitGmail/Cola.ColaIpRateLimit) [![Blog](https://flat.badgen.net/static/blog/odinsam.com)](https://odinsam.com)

#### config 配置
```json
{
  "IpRateLimiting": {
    // false则全局将应用限制，并且仅应用具有作为端点的规则* 。 true则限制将应用于每个端点，如{HTTP_Verb}{PATH}
    "EnableEndpointRateLimiting": true,
    // false则拒绝的API调用不会添加到调用次数计数器上
    "StackBlockedRequests": false,
　　 // 注意这个配置，表示获取用户端的真实IP，我们的线上经过负载后是 X-Forwarded-For，而测试服务器没有，所以是X-Real-IP
    "RealIpHeader": "X-Real-IP",
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 200,
    // 自定义返回的内容，所以必须设置HttpStatusCode和StatusCode为200
    "QuotaExceededResponse": {
      "Content": "{{\"code\":429,\"msg\":\"访问过于频繁，请稍后重试\",\"data\":null}}",
      "ContentType": "application/json",
      "StatusCode": 200
    },
    // IP白名单，本地调试或者UAT环境，可以加入相应的IP，略过策略的限制
    "IpWhitelist": [ ],
    // 端点白名单，如果全局配置了访问策略，设置端点白名单相当于IP白名单一样，略过策略的限制
    "EndpointWhitelist": [],
    "ClientWhitelist": [],
    // 具体的策略，根据不同需求配置不同端点即可， Period的单位可以是s, m, h, d，Limint是单位时间内的允许访问的次数
    "GeneralRules": [
      {
        "Endpoint": "*:/api/values/test",
        "Period": "5s",
        "Limit": 3
      }
    ]
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