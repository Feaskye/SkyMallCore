using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyMallCore.Data
{
    public class RespositoryBase<TEntity> : IRespositoryBase<TEntity> 
        where TEntity : class,new()
    {
        protected ISkyMallDbContext _SkyMallDBContext;
        protected DbSet<TEntity> _DbSet;


        public RespositoryBase(ISkyMallDbContext skyMallDbContext) {
            _SkyMallDBContext = skyMallDbContext;
            _DbSet = _SkyMallDBContext.Set<TEntity>();
        }

        public int Insert(TEntity entity)
        {
            _DbSet.Add(entity);
            _SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Added;
            return _SkyMallDBContext.SaveChanges();
        }
        public int Insert(List<TEntity> entitys)
        {
            foreach (var entity in entitys)
            {
                _DbSet.Add(entity);
                _SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Added;
            }
            return _SkyMallDBContext.SaveChanges();
        }

        public virtual int AddOneByOne(IList<TEntity> entitys)
        {
            //由于ef core批量写入数据时，采用了批处理方法，但这种方式会导致写入顺序错，暂时没有找到什么方法，所以改成一条条写。
            entitys.ToList().ForEach(t => {
                this._DbSet.Add(t);
                this._SkyMallDBContext.Entry<TEntity>(t).State = EntityState.Added;
                this._SkyMallDBContext.SaveChanges();
            });
            return entitys.Count;
        }

        public int Update(TEntity entity)
        {
            _DbSet.Attach(entity);
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                        _SkyMallDBContext.Entry(entity).Property(prop.Name).CurrentValue = null;
                    _SkyMallDBContext.Entry(entity).Property(prop.Name).IsModified = true;
                }
            }
            return _SkyMallDBContext.SaveChanges();
        }
        public int Delete(TEntity entity)
        {
            _DbSet.Remove(entity);
            _SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return _SkyMallDBContext.SaveChanges();
        }
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entitys = _DbSet.Where(predicate).ToList();
            entitys.ForEach(m =>{
                _DbSet.Remove(m);
                _SkyMallDBContext.Entry<TEntity>(m).State = EntityState.Deleted;
                });
            return _SkyMallDBContext.SaveChanges();
        }

        public virtual int Delete(int id)
        {
            TEntity entity = this.Get(id);
            _DbSet.Remove(entity);
            this._SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return this._SkyMallDBContext.SaveChanges();
        }

        public int Delete(List<int> ids)
        {
            ids.ForEach(t => {
                TEntity entity = this.Get(t);
                this._DbSet.Remove(entity);
                this._SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            });

            return this._SkyMallDBContext.SaveChanges();
        }

        public TEntity Get(object id)
        {
            return _DbSet.Find(id);
        }
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbSet.FirstOrDefault(predicate);
        }

        public virtual TResult Max<TResult>(Expression<Func<TEntity, TResult>> maxExpression, ISpecification<TEntity> specification)
        {
            TResult t = _DbSet.Where(specification.GetExpression()).Max(maxExpression);
            return t;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _DbSet;
        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbSet.Where(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbSet.Any(predicate);
        }

        public int Count(ISpecification<TEntity> specification)
        {
            return _DbSet.Where(specification.GetExpression()).Count();
        }


        public List<TEntity> GetPagedList(Pagination pagination)
        {
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = _DbSet.AsQueryable();
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }
        public List<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate, Pagination pagination)
        {
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = _DbSet.Where(predicate);
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }


        public List<TEntity> FromSql(string strSql, DbParameter[] dbParameter)
        {
            return _DbSet.FromSql<TEntity>(strSql, dbParameter).ToList();
        }

    }
}
