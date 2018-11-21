###	Net Core相关知识点：

	深耕Core相关知识
		1:底层理解（重要）
		2:第三方库（按需）
		3:新知识如：SingleR、IHostedService、BackgroundService（按需）


#####	WebSocket
	     app.Map("/ws", Services.SocketHandler.Map);

#####	SingleR
	 //services.AddSignalR();


#####	Web & Console通用的后台任务类
>		IHostedService（源）	根据需要使用此类；

>		BackgroundService（半成品源） 已做过处理，比较常用；

>		继承此类后都可以使用如下方式调用：
>		services.AddHostedService<PrinterHostedService>();

>	注意：Web & Console程序停止，后台任务Service进程并未停止，需要引用一个变量来记录要运行的任务，将其从StartAsync方法中解放出来。
>	比如：bool _stopping ，在StartAsync或ExecuteAsync中循环时做是否停止判断，在StopAsync停止时设置为false 






#####






