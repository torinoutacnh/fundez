using Elect.Data.EF.Services.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TIGE.Contract.Repository.Models.User;

namespace TIGE.Repository.Maps.User
{
    public class UserBusinessEntityMap : StringEntityTypeConfiguration<UserBusinessEntity>
    {
        public override void Map(EntityTypeBuilder<UserBusinessEntity> builder)
        {
            base.Map(builder);
           
            builder.ToTable(nameof(UserBusinessEntity));

            builder.HasIndex(x => x.ToUserId);
            builder.HasIndex(x => x.FromUserId);
            builder.HasIndex(x => x.AmountUSD);

            builder.HasOne(x => x.ToUser).WithMany(x => x.ToBusinessUsers).HasForeignKey(x => x.ToUserId);
            builder.HasOne(x => x.FromUser).WithMany(x => x.FromBusinessUsers).HasForeignKey(x => x.FromUserId);
            builder.HasOne(x => x.FromUserSlot).WithMany(x => x.UserBusiness).HasForeignKey(x => x.FromUserSlotId);

        }
    }
}