using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SkyMallCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCore.Data
{
    public class SkyMallDBContext:DbContext, ISkyMallDbContext
    {
        public SkyMallDBContext(DbContextOptions<SkyMallDBContext> options) 
            : base(options)
        {
        }

        public DbSet<SysUser> SysUsers { get; set; }
        




    }

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
