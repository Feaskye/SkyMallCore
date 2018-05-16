using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Data
{
    public interface ISkyMallDbContext : IDbContext
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
