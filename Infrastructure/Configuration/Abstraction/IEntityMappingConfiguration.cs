using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration.Abstraction
{
    public interface IEntityMappingConfiguration<T>
    {
        public void Map(ModelBuilder builder);
    }
}
