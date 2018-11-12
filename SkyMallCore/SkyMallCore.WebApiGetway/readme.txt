Ocelot框架：https://github.com/ThreeMammals/Ocelot

Ocelot的目标是使用.NET运行微服务/面向服务架构，我们需要一个统一的入口进入我们的服务，提供监控、鉴权、负载均衡（提供轮询、最少访问）等机制，
也可以通过编写中间件的形式，来扩展Ocelot的功能。  Ocelot是一堆特定顺序的中间件。

 Ocelot框架内部集成了IdentityServer和Consul（服务注册发现），还引入了Polly来处理进行故障处理





configuration.json 的Api网关配置如下：
{
  "ReRoutes": [
    {
      "DownstreamPathTemplate": "/api/token",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 63656
        }
      ],
      "UpstreamPathTemplate": "/api/token",
      "UpstreamHttpMethod": [ "Get" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "TestKey",
        "AllowScopes": []
      }
    },
    {
      "DownstreamPathTemplate": "/api/todo/members",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 63656
        }
      ],
      "UpstreamPathTemplate": "/api/todo/members",
      "UpstreamHttpMethod": [ "Get" ]
    },
    {
      "DownstreamPathTemplate": "/api/todo/get",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 63656
        }
      ],
      "UpstreamPathTemplate": "/api/todo/get",
      "UpstreamHttpMethod": [ "Get" ]
    }
  ],
  "GlobalConfiguration": {
    "ServiceDiscoveryProvider": {
      "Host": "localhost",
      "Port": 63656
    }
  }
}

1）：以下两者要对应相关Api网址，所属要一致，比如以上例子
DownstreamPathTemplate
DownstreamHostAndPorts
2）：配置中.AddAdministration("/admin", ooptions);的用法待研究















附：博客园介绍
http://www.cnblogs.com/ibeisha/p/ocelot.html
















