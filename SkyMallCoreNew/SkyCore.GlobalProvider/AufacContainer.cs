using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyCore.GlobalProvider
{
    public class AufacContainer
    {

        private IContainer _container;


        /// <summary>
        /// 创建集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">服务名称</param>
        public List<T> CreateList<T>(string name = null)
        {
            var result = CreateList(typeof(T), name);
            if (result == null)
                return new List<T>();
            return ((IEnumerable<T>)result).ToList();
        }

        /// <summary>
        /// 创建集合
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="name">服务名称</param>
        public object CreateList(Type type, string name = null)
        {
            Type serviceType = typeof(IEnumerable<>).MakeGenericType(type);
            return Create(serviceType, name);
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="name">服务名称</param>
        public T Create<T>(string name = null)
        {
            return (T)Create(typeof(T), name);
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="name">服务名称</param>
        public object Create(Type type, string name = null)
        {
            return CoreContextProvider.HttpContext?.RequestServices != null ? GetServiceFromHttpContext(type, name) : GetService(type, name);
        }


        /// <summary>
        /// 从HttpContext获取服务
        /// </summary>
        private object GetServiceFromHttpContext(Type type, string name)
        {
            var serviceProvider = CoreContextProvider.HttpContext.RequestServices;
            if (name == null)
                return serviceProvider.GetService(type);
            var context = serviceProvider.GetService<IComponentContext>();
            return context.ResolveNamed(name, type);
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        private object GetService(Type type, string name)
        {
            if (name == null)
                return _container.Resolve(type);
            return _container.ResolveNamed(name, type);
        }


        /// <summary>
        /// 注册依赖
        /// </summary>
        /// <param name="configs">依赖配置</param>
        public void Register(params IModule[] configs)
        {
            Register(null, configs);
        }
        
        /// <summary>
        /// 创建容器生成器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="actionBefore"></param>
        /// <param name="modules"></param>
        /// <returns></returns>
        public ContainerBuilder CreateBuilder(IServiceCollection services,params IModule[] modules)
        {
            var builder = new ContainerBuilder();
            if (modules != null)//新模块组件注册
            {
                foreach (var module in modules)
                {
                    builder.RegisterModule(module);
                }
            }

            if (services != null)
                builder.Populate(services);//将services中的服务填充到Autofac中

            return builder;
        }


        public IServiceProvider Register(IServiceCollection services,params IModule[] modules)
        {
            var builder = CreateBuilder(services,modules);
            //创建容器.
            _container = builder.Build();
            //使用容器创建 AutofacServiceProvider 
            return new AutofacServiceProvider(_container);
        }


        /// <summary>
        /// 释放容器
        /// </summary>
        public void Dispose()
        {
            _container.Dispose();
        }


    }
}
