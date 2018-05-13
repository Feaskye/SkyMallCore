using SkyMallCore.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SkyMallCore.Data
{
    public interface IRespositoryBase<TEntity> where TEntity : class,new()
    {
        int Insert(TEntity entity);
        int Insert(List<TEntity> entitys);
        int AddOneByOne(IList<TEntity> entitys);
        int Update(TEntity entity);
        int Delete(TEntity entity);
        int Delete(Expression<Func<TEntity, bool>> predicate);
        int Delete(int id);
        int Delete(List<int> ids);
        TEntity Get(object id);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        TResult Max<TResult>(Expression<Func<TEntity, TResult>> maxExpression, ISpecification<TEntity> specification);

        int Count(ISpecification<TEntity> specification);

        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetPagedList(Pagination pagination);
        List<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate, Pagination pagination);

        List<TEntity> FromSql(string strSql, DbParameter[] dbParameter = null);
    }
}
