using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
           
            builder.HasOne(p => p.SubCategory)
               .WithMany(sc => sc.Products)
               .HasForeignKey(p => p.SubCategoryId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
