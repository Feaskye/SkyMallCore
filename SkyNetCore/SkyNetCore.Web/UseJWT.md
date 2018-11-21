#### 用JWT来保护我们的ASP.NET Core Web API（即Api授权）
　  站点是通过RPC的方式来访问api取得资源的，当站点是直接访问api，没有拿到有访问权限的令牌，那么站点是拿不到相关的数据资源的。
就像左图展示的那样，发起了请求但是拿不到想要的结果；当站点先去授权服务器拿到了可以访问api的access_token(令牌)后，再通过这个
access_token去访问api，api才会返回受保护的数据资源。

![image](https://images2015.cnblogs.com/blog/558945/201611/558945-20161112200303639-504860167.png)

参照项目中的：TokenProviderMiddleware 生成Access_Token
