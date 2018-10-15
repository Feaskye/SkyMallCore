using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace SkyMallCore.WebApiData
{
    public interface IMysqlDbContext : IDbContext
    {

    }
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>()
             where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        int SaveChanges();

        DatabaseFacade Database { get; }
    }

}
