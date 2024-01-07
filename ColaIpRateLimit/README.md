# Cola.ColaIpRateLimit 中间件框架

#### config 配置 更详细配置请参看 [详细配置](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#setup) 包括了
IP速率限制 - IpRateLimiting  
IP速率限制策略 -  IpRateLimitPolicies
Client速率限制 -  ClientRateLimiting
Client速率限制策略 - ClientRateLimitPolicies
```json IP速率限制
{
  // IP速率限制
  "ColaIpRateLimit": {
    // 这里暂时只支持 Memory 和 Redis 两种方式(默认使用 Memory).  如果使用 redis缓存，则需要再注入IpRateLimit之前优先注入redis
    "IpRateLimitCache": "Memory",
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
        // 5秒 3次
        "Endpoint": "*:/api/values/test",
        "Period": "5s",
        "Limit": 3
      }
    ]
  }，
  // IP速率限制策略
  "IpRateLimitPolicies": {
    "IpRules": [
      {
        "Ip": "XXX.XXX.XXX.XXX",
        "Rules": [
          {
            "Endpoint": "*",
            "Period": "1s",
            "Limit": 10
          },
          {
            "Endpoint": "*",
            "Period": "1m",
            "Limit": 1000
          }
        ]
      }
    ]
  },
  // Client速率限制
  "ClientRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "ClientIdHeader": "X-ClientId",
    "EndpointWhitelist": [],
    "ClientWhitelist": [],
    "QuotaExceededResponse": {
      "Content": "{{\"code\":429,\"type\":\"error\",\"message\":\"访问人数过多,请稍后重试!\",\"result\":null,\"extras\":null}}",
      "ContentType": "application/json",
      "StatusCode": 429
    },
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1s",
        "Limit": 10
      },
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 1000
      }
    ]
  },
  // Client速率限制策略
  "ClientRateLimitPolicies": {
    "ClientRules": [
      {
        "ClientId": "xxx-xxx",
        "Rules": [
          {
            "Endpoint": "*",
            "Period": "1s",
            "Limit": 10
          },
          {
            "Endpoint": "*",
            "Period": "1m",
            "Limit": 1000
          }
        ]
      }
    ]
  }
}
```
#### 使用 ColaMiddleware 中间件
```csharp
builder.Configuration.AddJsonFile("appsettings.json");
var config = builder.Configuration;
// 添加 IpRateLimit 注入 默认使用内存缓存，如果使用redis缓存
builder.Services.AddSingletonColaIpRateLimit(config);

// 添加 IpRateLimit 中间件，尽量添加在管道到外层
app.UseIpRateLimiting();
```