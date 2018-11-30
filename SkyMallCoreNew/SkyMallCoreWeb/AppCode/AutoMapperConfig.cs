using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using SkyMallCore.ViewModel.Business;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SkyMallCoreWeb
{
    public static class AutoMapperConfig
    {
        /// <summary>
        /// 注册数据类型映射
        /// </summary>
        /// <param name="services"></param>
        public static void AddMapConfig(this IServiceCollection services)
        {
            Mapper.Initialize(ops=> {
                ops.CreateMap<Article, ArticleDetailView>();
                ops.CreateMap<PagedList<Article>, PagedList<ArticleDetailView>>();
                ops.CreateMap<Member, MemberDetailView>();


                var memMap = ops.CreateMap<MemberDetailView, Member>();
                memMap.ForMember(a=>a.CreatorTime,b=>b.Ignore());
                memMap.ForMember(a => a.CreatorUserId, b => b.Ignore());


                ops.CreateMap<ArticleDetailView,Article>();
                ops.CreateMap<ArticleCateDetailView, ArticleCategory>();
                ops.CreateMap<ArticleCategory, ArticleCateDetailView>();
                ops.CreateMap<News, NewsDetailView>();
                ops.CreateMap<Help, HelpDetailView>();
                ops.CreateMap<ArticleCateDetailView, ArticleCategory>();
                ops.CreateMap<TopicDetailView, ArticleTopic>();
                ops.CreateMap<ArticleTopic, TopicDetailView>();





                //ops.CreateMap<SysUser, SysUser>().ForAllMembers(w=>w.Condition(srs=>srs.nu));

            }) ;
            

        }

    }


    /// <summary>
    /// The collection extensions.
    /// </summary>
    public static class ObjectExtensions
    {
        private static readonly MethodInfo mapMethod;
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, Tuple<MethodInfo, Type>> methodsMapper = new ConcurrentDictionary<Tuple<Type, Type>, Tuple<MethodInfo, Type>>();

        static ObjectExtensions()
        {
            mapMethod = (typeof(Mapper)).GetMethods().FirstOrDefault(_ => _.Name == "Map" && _.GetParameters().Length == 1);
        }

        public static T MapTo<T>(this IPagedList tList) where T : class
        {
            var totalCount = tList.TotalCount;
            var pageIndex = tList.PageIndex;
            var pageSize = tList.PageSize;
            //var groupSize = tList.GroupSize;


            var t = methodsMapper.GetOrAdd(new Tuple<Type, Type>(tList.GetType(), typeof(T)), _ =>
            {
                var targetGenericArguments = typeof(T).GenericTypeArguments[0];
                var targetGenericArgumentsIEnumerableType = typeof(IEnumerable<>).MakeGenericType(targetGenericArguments);
                return new Tuple<MethodInfo, Type>(mapMethod.MakeGenericMethod(targetGenericArgumentsIEnumerableType),
                    typeof(PagedList<>).MakeGenericType(targetGenericArguments));
            });
            var rtn2 = t.Item1.Invoke(null, new object[] { tList });
            var o2 = Activator.CreateInstance(t.Item2, rtn2, totalCount, pageIndex, pageSize) as T;
            return o2;
        }
        
    }



}
