using Elect.Data.EF.Services.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIGE.Contract.Repository.Models;

namespace TIGE.Repository.Maps
{
    public class ImageEntityMap : StringEntityTypeConfiguration<ImageEntity>
    {
        public override void Map(EntityTypeBuilder<ImageEntity> builder)
        {
            base.Map(builder);

            builder.ToTable(nameof(ImageEntity));
        }
    }
}