
using System;
using TIGE.Contract.Repository.Models.User;
using Elect.Data.EF.Services.Map;
using TIGE.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TIGE.Repository.Maps.User
{
    public class UserSlotRequestEntityMap : StringEntityTypeConfiguration<UserSlotsEntity>
    {
        public override void Map(EntityTypeBuilder<UserSlotsEntity> builder)
        {
            base.Map(builder);
           
            builder.ToTable(nameof(UserWithDrawRequestEntity));

            builder.HasIndex(x => x.ApproveBy);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.CurrentBalance);
            builder.HasIndex(x => x.Quantity);

            builder.HasOne(x=>x.Approve).WithMany(x=>x.ApproveSlots).HasForeignKey(x=>x.ApproveBy);
            builder.HasOne(x=>x.User).WithMany(x=>x.SlotHistories).HasForeignKey(x=>x.UserId);
        }
    }
}