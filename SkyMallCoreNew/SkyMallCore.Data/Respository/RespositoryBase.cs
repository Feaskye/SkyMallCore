using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SkyCoreLib.Utils;
using SkyMallCore.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

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
            if (entity.Id.IsEmpty())
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

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="groupNum">默认50个批量提交一次</param>
        public void AddBetch(List<TEntity> entities,int groupNum = 50)
        {
            if (_SkyMallDBContext.Database.GetDbConnection().State != ConnectionState.Open)
            {
                _SkyMallDBContext.Database.OpenConnection(); //打开Connection连接
            }

            //调用BulkInsert方法,将entitys集合数据批量插入到数据库的tolocation表中
            using (var bulkCopy = new SqlBulkCopy((SqlConnection)_SkyMallDBContext.Database.GetDbConnection()))
            {
                //table 表
                var tableAttribute = (TableAttribute)typeof(TEntity).GetCustomAttribute(typeof(TableAttribute));
                bulkCopy.DestinationTableName = tableAttribute.Name;

                var table = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(TEntity))
                    .Cast<PropertyDescriptor>()
                    .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                    .ToArray();

                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                }


                bulkCopy.BatchSize = groupNum;

                var values = new object[props.Length];
                foreach (var item in entities)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }

                bulkCopy.WriteToServer(table);
            }

            if (_SkyMallDBContext.Database.GetDbConnection().State != ConnectionState.Closed)
            {
                _SkyMallDBContext.Database.CloseConnection(); //关闭Connection连接
            }

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
