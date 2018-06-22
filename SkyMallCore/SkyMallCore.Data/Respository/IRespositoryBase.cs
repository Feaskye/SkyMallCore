using Microsoft.EntityFrameworkCore.Storage;
using SkyMallCore.Core;
using SkyMallCore.Models;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SkyMallCore.Data
{
    public interface IRespositoryBase<TEntity> where TEntity : KeyEntity, new()
    {

        IDbContextTransaction BeginTransaction();

        void Commit();

        void Rollback();

        int Insert(TEntity entity);

        int Insert(List<TEntity> entitys);

        int AddOneByOne(IList<TEntity> entitys);

        int Update(TEntity entity);

        int UpdateFields(TEntity entity, params string[] fields);

        int Delete(TEntity entity);

        int Delete(Expression<Func<TEntity, bool>> predicate);

        int Delete(object id);

        int Delete(List<int> ids);

        TEntity Get(object id);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        TResult Max<TResult>(Expression<Func<TEntity, TResult>> maxExpression, ISpecification<TEntity> specification);

        int Count(Expression<Func<TEntity, bool>> predicate = null);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        List<TEntity> GetPagList(Pagination pagination);

        List<TEntity> GetPagList(Expression<Func<TEntity, bool>> predicate, Pagination pagination);

        PagedList<TEntity> GetPagedList<Tkey>(Expression<Func<TEntity, bool>> where,
            int pageIndex, int pageSize, Expression<Func<TEntity, Tkey>> order = null);

        PagedList<TResult> GetPagedList<TResult, Tkey>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> where,
            int pageIndex, int pageSize, Expression<Func<TEntity, Tkey>> order = null) where TResult : class;

        List<TEntity> FromSql(string strSql, DbParameter[] dbParameter = null);

        int ExecuteSql(string strSql, DbParameter[] dbParameters = null);
    }
}
