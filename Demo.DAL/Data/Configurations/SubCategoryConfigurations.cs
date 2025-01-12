using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Configurations
{
    public class SubCategoryConfigurations : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.HasOne(sc => sc.Category)
                 .WithMany(c => c.SubCategories)
                 .HasForeignKey(sc => sc.CategoryId);
            builder.HasIndex(sc => new { sc.CategoryId, sc.Name }).IsUnique();
        }
      
    }
}
