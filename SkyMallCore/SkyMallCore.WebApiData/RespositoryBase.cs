using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkyCoreApi.ViewModel;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkyMallCore.WebApiData
{
    public class RespositoryBase<TEntity> : IRespositoryBase<TEntity>
        where TEntity : KeyEntity, new()
    {
        protected IMysqlDbContext _SkyMallDBContext;
        protected DbSet<TEntity> _DbSet;


        public RespositoryBase(IMysqlDbContext skyMallDbContext) {
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



        public virtual bool Insert(TEntity entity)
        {
            _SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Added;
            return _SkyMallDBContext.SaveChanges() > 0;
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

        public virtual bool Update(TEntity entity)
        {
            _SkyMallDBContext.Entry(entity).State = EntityState.Modified;
            return _SkyMallDBContext.SaveChanges() > 0;
        }

        public virtual bool CreateOrUpdate(TEntity entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
                return Insert(entity);
            }
            return Update(entity);
        }


        public bool UpdateFields(TEntity entity, params string[] fields)
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
            return _SkyMallDBContext.SaveChanges() > 0;
        }


        public bool UpdateFields(List<TEntity> entities, params string[] fields)
        {
            entities.ForEach(entity =>
            {
                PropertyInfo[] props = entity.GetType().GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (prop.GetValue(entity, null) != null && prop.GetCustomAttribute(typeof(KeyAttribute)) == null)
                    {
                        //fields
                        if (fields != null && fields.Length > 0 && !fields.Any(w => w == prop.Name))
                            continue;

                        if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                            _SkyMallDBContext.Entry(entity).Property(prop.Name).CurrentValue = null;
                        _SkyMallDBContext.Entry(entity).Property(prop.Name).IsModified = true;
                    }
                }
            });
            return _SkyMallDBContext.SaveChanges() > 0;
        }

        public virtual bool Delete(TEntity entity)
        {
            _SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return _SkyMallDBContext.SaveChanges() > 0;
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

        public virtual bool Delete(object id)
        {
            TEntity entity = this.Get(id);
            //_DbSet.Remove(entity);
            this._SkyMallDBContext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return this._SkyMallDBContext.SaveChanges() > 0;
        }

        public virtual int Deletebatch(List<object> ids)
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

        public virtual TResult Max<TResult>(Expression<Func<TEntity, TResult>> maxExpression, Expression<Func<TEntity, bool>> predicate)
        {
            TResult t = _DbSet.Where(predicate).Max(maxExpression);
            return t;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _DbSet;
        }
        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "")
        {
            var query = _DbSet.AsTracking();
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.Where(predicate);
        }

        public IQueryable<TResult> GetFeilds<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "")
        {
            var query = _DbSet.AsTracking();
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.Where(predicate).Select(select);
        }

        public TResult GetFeild<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
  string includeProperties = "")
        {
            return GetFeilds<TResult>(select,predicate,orderBy,includeProperties).FirstOrDefault();
        }


        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbSet.Any(predicate);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null,string includeProperties = null)
        {
            var query = _DbSet.AsTracking();
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            return predicate ==null? query.Count() : query.Where(predicate).Count();
        }

        //public virtual TResult Sum<TResult>(Expression<Func<TEntity, TResult>> maxExpression, Expression<Func<TEntity, bool>> predicate = null)
        //{
        //    return predicate == null ? _DbSet.Sum(maxExpression) : _DbSet.Where(predicate).Sum(maxExpression);
        //}

            
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public PagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> where,
            int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "") 
        {
            var query = _DbSet.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return PagedList<TEntity>.GetPagedList(query.Where(where), pageIndex, pageSize);
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
        public PagedList<TResult> GetPagedList<TResult>(Expression<Func<TEntity, TResult>> select, Expression<Func<TEntity, bool>> where, 
            int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "")
        {
            var query = _DbSet.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            query = query.Where(where);
            return PagedList<TResult>.GetPagedList(query.Select(select), pageIndex, pageSize);
        }
        


        public List<TEntity> FromSql(string strSql, DbParameter[] dbParameter)
        {
            return _DbSet.FromSql(strSql, dbParameter).ToList();
        }

        public int ExecuteSql(string strSql,DbParameter[] dbParameters = null)
        {
            return _SkyMallDBContext.Database.ExecuteSqlCommand(strSql, dbParameters);
        }

    }
}
