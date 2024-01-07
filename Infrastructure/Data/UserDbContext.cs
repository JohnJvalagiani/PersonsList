using IG.Core.Data.Entities;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Infrastructure.Data
{
    public class UserDbContext:DbContext
    {

        public DbSet<Person> Persons { get; set; }


        public UserDbContext(DbContextOptions<UserDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            


            builder.Entity<ConnectedPerson>()
             .HasOne<Person>()
             .WithMany()
            .HasForeignKey(p => p.PersonId);

            //.OnDelete(DeleteBehavior.Cascade);

        }
    }
}
