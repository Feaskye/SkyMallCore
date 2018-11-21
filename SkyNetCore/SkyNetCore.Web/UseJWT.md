#### ��JWT���������ǵ�ASP.NET Core Web API����Api��Ȩ��
��  վ����ͨ��RPC�ķ�ʽ������apiȡ����Դ�ģ���վ����ֱ�ӷ���api��û���õ��з���Ȩ�޵����ƣ���ôվ�����ò�����ص�������Դ�ġ�
������ͼչʾ���������������������ò�����Ҫ�Ľ������վ����ȥ��Ȩ�������õ��˿��Է���api��access_token(����)����ͨ�����
access_tokenȥ����api��api�Ż᷵���ܱ�����������Դ��

![image](https://images2015.cnblogs.com/blog/558945/201611/558945-20161112200303639-504860167.png)

######  TokenProviderMiddleware ����Access_Token <a href='http://www.cnblogs.com/catcher1994/p/6057484.html' target='_blank'>���</a>�ο�
    //��Token��������װΪ�м������
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
    //У��Url��GenericAuthorizedResult��Ӧ��У�鲢����Token

######    ConfigureJwtAuthServices��д���JwtBearer������ <a href='https://www.c-sharpcorner.com/article/handle-refresh-token-using-asp-net-core-2-0-and-json-web-token/' target='_blank'>���</a>�ο�
      services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });
