using Infrastructure.Configuration.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration
{
    public abstract class EntityConfiguration<T> : IEntityMappingConfiguration<T>
        where T : class
    {
        public abstract void Map(EntityTypeBuilder<T> builder);
        public  void Map(ModelBuilder builder)
        {
            Map(builder.Entity<T>());
        }
    }
}
