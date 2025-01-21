using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Data.Configurations
{
    public class ImageConfigurations : IEntityTypeConfiguration<Image>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Image> builder)
        {

            builder.HasOne(image => image.Product )
               .WithMany(p => p.Images)
               .HasForeignKey(image=> image.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
