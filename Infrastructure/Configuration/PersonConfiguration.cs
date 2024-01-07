using IG.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration
{
    public class PersonConfiguration : EntityConfiguration<Person>
    {
        public override void Map(EntityTypeBuilder<Person> builder)
        {

            builder.ToTable("Person");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstNameENG).HasMaxLength(50).IsRequired();
            builder.Property(u => u.FirstNameGEO).IsRequired();
            builder.Property(u => u.Sex).IsRequired();

        }
    
    }
}
