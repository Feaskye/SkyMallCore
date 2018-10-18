﻿using Microsoft.EntityFrameworkCore.Storage;
using SkyCoreApi.ViewModel;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SkyMallCore.WebApiData
{
    public interface IRespositoryBase<TEntity> where TEntity : KeyEntity, new()
    {

        IDbContextTransaction BeginTransaction();

        void Commit();

        void Rollback();

        bool Insert(TEntity entity);

        int Insert(List<TEntity> entitys);

        int AddOneByOne(IList<TEntity> entitys);

        bool Update(TEntity entity);

        bool UpdateFields(List<TEntity> entities, params string[] fields);

        bool UpdateFields(TEntity entity, params string[] fields);


        bool CreateOrUpdate(TEntity entity);


        bool Delete(TEntity entity);

        int Delete(Expression<Func<TEntity, bool>> predicate);

        bool Delete(object id);

        int Deletebatch(List<object> ids);

        TEntity Get(object id);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        TResult Max<TResult>(Expression<Func<TEntity, TResult>> maxExpression, Expression<Func<TEntity, bool>> predicate);

        int Count(Expression<Func<TEntity, bool>> predicate = null, string includeProperties = null);

        //TResult Sum<TResult>(Expression<Func<TEntity, TResult>> maxExpression, Expression<Func<TEntity, bool>> predicate = null);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "");

        IQueryable<TResult> GetFeilds<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "");

        TResult GetFeild<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
  string includeProperties = "");


        bool Any(Expression<Func<TEntity, bool>> predicate);

        //https://www.telerik.com/forums/pass-parameter-to-expression-func-tentity-bool-filter
        PagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> where,
            int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "");
        
        PagedList<TResult> GetPagedList<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> where,
            int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "");

        List<TEntity> FromSql(string strSql, DbParameter[] dbParameter = null);

        int ExecuteSql(string strSql, DbParameter[] dbParameters = null);
    }
}
