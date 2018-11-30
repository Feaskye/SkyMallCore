using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyMallCore.ViewModel
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int PageCount { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }

        int GroupSize { get; set; }
    }

    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList<T> : IList<T>, IPagedList
    {
    }

    public class PagedList<T> : List<T>, IPagedList
    {
        public int PageIndex { get; private set; }
        public int PageCount { get; private set; }
        public int TotalCount { get; private set; }
        public int PageSize { get; private set; }

        /// <summary>
        /// 界面一组显示5个按钮
        /// </summary>
        public int GroupSize { get; set; }

        public PagedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageCount = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            PageSize = pageSize;
            GroupSize = 5;
            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < PageCount);
            }
        }

        public static PagedList<T> GetPagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageIndex, pageSize);
        }

    }
}
