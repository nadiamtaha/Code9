using Code9.Entities.Common;
using Code9.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Data
{
    public class Code9Context : IdentityDbContext<User, Role, string,
       IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
       IdentityRoleClaim<string>, IdentityUserToken<string>>
        {
            public Code9Context(DbContextOptions<Code9Context> options) : base(options)
            {

            }
      
            public DbSet<ExceptionLog> ExceptionLogs { get; set; }
            public DbSet<Category> Category { get; set; }
            public DbSet<CheckInOut> CheckInOut { get; set; }
            public DbSet<Shop> Shop { get; set; }
            public DbSet<ShopStatus> ShopStatus { get; set; }
            public DbSet<UserStatus> UserStatus { get; set; }
            public DbSet<UserDevice> UserDevices { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);


            }
        }
}
