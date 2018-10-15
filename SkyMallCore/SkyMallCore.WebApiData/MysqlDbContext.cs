using MySql.Data.MySqlClient;
using SkyMallCore.WebApiData.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SkyMallCore.WebApiData
{
    public class MysqlDbContext: DbContext, IMysqlDbContext
    {

        public MysqlDbContext(DbContextOptions options) : base(options)
        {
        }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //}


        public DbSet<Member> Members { get; set; }

    }



    
}
