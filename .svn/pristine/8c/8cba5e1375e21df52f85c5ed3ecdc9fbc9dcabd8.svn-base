
using System;
using TIGE.Contract.Repository.Models.User;
using Elect.Data.EF.Services.Map;
using TIGE.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TIGE.Repository.Maps.User
{
    public class UserSellTokenRequestEntityMap : StringEntityTypeConfiguration<UserSellTokenEntity>
    {
        public override void Map(EntityTypeBuilder<UserSellTokenEntity> builder)
        {
            base.Map(builder);
           
            builder.ToTable(nameof(UserWithDrawRequestEntity));

            builder.HasIndex(x => x.ApproveBy);
            builder.HasIndex(x => x.UserId);

            builder.HasOne(x=>x.User).WithMany(x=>x.UserSellTokens).HasForeignKey(x=>x.UserId);
        }
    }
}