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
        public DbSet<SysArea> SysAreas { get; set; }
        public DbSet<SysItems> SysItems { get; set; }
        public DbSet<SysItemsDetail> SysItemsDetails { get; set; }
        public DbSet<SysModule> SysModules { get; set; }
        public DbSet<SysModuleButton> SysModuleButtons { get; set; }
        public DbSet<SysOrganize> SysOrganizes { get; set; }
        public DbSet<SysRole> SysRoles { get; set; }
        public DbSet<SysRoleAuthorize> SysRoleAuthorizes { get; set; }
        public DbSet<SysUserLogOn> SysUserLogOns { get; set; }





    }

    
}
