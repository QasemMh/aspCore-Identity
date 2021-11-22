using _1_app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace _1_app.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users", "security");
            //.Ignore(e=>e.EmailConfirmed);

            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken", "security");


        }

        public DbSet<_1_app.Models.UserProfileViewModel> UserProfileViewModel { get; set; }
    }
}
