﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Core;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyMallCore.Data
{
    public class RespositoryBase<TEntity> : IRespositoryBase<TEntity>
        where TEntity : Models.KeyEntity, new()
    {
        protected ISkyMallDbContext _SkyMallDBContext;
        protected DbSet<TEntity> _DbSet;


        public RespositoryBase(ISkyMallDbContext skyMallDbContext) {
            _SkyMallDBContext = skyMallDbContext;
            _DbSet = _SkyMallDBContext.Set<TEntity>();
        }

        private IDbContextTransaction _dbTransaction { get; set; }
        public IDbContextTransaction BeginTransaction()
        {
            _dbTransaction = _SkyMallDBContext.Database.BeginTransaction();
            return _dbTransaction;
        }
        public void Commit()
        {
            if (_dbTransaction != null)
            {
                _dbTransaction.Commit();
            }
        }

        public void Rollback()
        {
            if (_dbTransaction != null)
            {
                this._dbTransaction.Rollback();
            }
        }

        //public void Dispose()
        //{
        //    if (dbTransaction != null)
        //    {
        //        this.dbTransaction.Dispose();
        //    }
        //    //todo dispose()
        //    //this._SkyMallDBContext.Database.Dispose();
        //}



        public virtual int Insert(TEntity entity)
        {
            _SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Added;
            return _SkyMallDBContext.SaveChanges();
        }
        public virtual int Insert(List<TEntity> entitys)
        {
            foreach (var entity in entitys)
            {
                _SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Added;
            }
            return _SkyMallDBContext.SaveChanges();
        }

        public virtual int AddOneByOne(IList<TEntity> entitys)
        {
            entitys.ToList().ForEach(t => {
                this._SkyMallDBContext.Entry<TEntity>(t).State = EntityState.Added;
                this._SkyMallDBContext.SaveChanges();
            });
            return entitys.Count;
        }

        public virtual int Update(TEntity entity)
        {
            _SkyMallDBContext.Entry(entity).State = EntityState.Modified;
            return _SkyMallDBContext.SaveChanges();
        }

        public int UpdateFields(TEntity entity, params string[] fields)
        {
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null && prop.GetCustomAttribute(typeof(KeyAttribute))==null)
                {
                    //fields
                    if (fields != null&& fields.Length>0 && !fields.Any(w => w == prop.Name))
                        continue;

                    if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                        _SkyMallDBContext.Entry(entity).Property(prop.Name).CurrentValue = null;
                    _SkyMallDBContext.Entry(entity).Property(prop.Name).IsModified = true;
                }
            }
            return _SkyMallDBContext.SaveChanges();
        }
        public virtual int Delete(TEntity entity)
        {
            _SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return _SkyMallDBContext.SaveChanges();
        }
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entitys = _DbSet.Where(predicate).ToList();
            entitys.ForEach(m =>{
                _DbSet.Remove(m);
                _SkyMallDBContext.Entry<TEntity>(m).State = EntityState.Deleted;
                });
            return _SkyMallDBContext.SaveChanges();
        }

        public virtual int Delete(object id)
        {
            TEntity entity = this.Get(id);
            //_DbSet.Remove(entity);
            this._SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return this._SkyMallDBContext.SaveChanges();
        }

        public virtual int Delete(List<int> ids)
        {
            ids.ForEach(t => {
                TEntity entity = this.Get(t);
                //this._DbSet.Remove(entity);
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

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate ==null? _DbSet.Count() : _DbSet.Where(predicate).Count();
        }


        public List<TEntity> GetPagList(Pagination pagination)
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
        public List<TEntity> GetPagList(Expression<Func<TEntity, bool>> predicate, Pagination pagination)
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

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public PagedList<TEntity> GetPagedList<Tkey>(Expression<Func<TEntity, bool>> where,
            int pageIndex, int pageSize, Expression<Func<TEntity, Tkey>> order = null) 
        {
            var list = _DbSet.AsNoTracking();
            if (order != null)
            {
                list = list.OrderBy(order);
            }
            return PagedList<TEntity>.GetPagedList(list, pageIndex, pageSize);
        }


        /// <summary>
        /// 分页，指定查询类型
        /// </summary>
        /// <param name="select"></param>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public PagedList<TResult> GetPagedList<TResult, Tkey>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> where, 
            int pageIndex, int pageSize, Expression<Func<TEntity, Tkey>> order = null) where TResult : class
        {
            var list = _DbSet.AsNoTracking();
            if (order != null)
            {
                list = list.OrderBy(order);
            }
            return PagedList<TResult>.GetPagedList(list.Select(select), pageIndex, pageSize);
        }


        public List<TEntity> FromSql(string strSql, DbParameter[] dbParameter)
        {
            return _DbSet.FromSql<TEntity>(strSql, dbParameter).ToList();
        }

        public int ExecuteSql(string strSql,DbParameter[] dbParameters = null)
        {
            return _SkyMallDBContext.Database.ExecuteSqlCommand(strSql, dbParameters);
        }

    }
}
