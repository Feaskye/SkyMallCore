#### 用JWT来保护我们的ASP.NET Core Web API（即Api授权）
　  站点是通过RPC的方式来访问api取得资源的，当站点是直接访问api，没有拿到有访问权限的令牌，那么站点是拿不到相关的数据资源的。
就像左图展示的那样，发起了请求但是拿不到想要的结果；当站点先去授权服务器拿到了可以访问api的access_token(令牌)后，再通过这个
access_token去访问api，api才会返回受保护的数据资源。

![image](https://images2015.cnblogs.com/blog/558945/201611/558945-20161112200303639-504860167.png)

######  TokenProviderMiddleware 生成Access_Token <a href='http://www.cnblogs.com/catcher1994/p/6057484.html' target='_blank'>点此</a>参考
    //将Token相关请求封装为中间件处理
    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
        {
            await _next(context);
        }
        // Request must be POST with Content-Type: application/x-www-form-urlencoded
        if (!context.Request.Method.Equals("POST")
            || !context.Request.HasFormContentType)
        {
            await ReturnBadRequest(context);
        }
        await GenericAuthorizedResult(context);
    }
    //校验Url后GenericAuthorizedResult做应用校验并生成Token

######    ConfigureJwtAuthServices中写入对JwtBearer的配置 <a href='https://www.c-sharpcorner.com/article/handle-refresh-token-using-asp-net-core-2-0-and-json-web-token/' target='_blank'>点此</a>参考
      services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });
