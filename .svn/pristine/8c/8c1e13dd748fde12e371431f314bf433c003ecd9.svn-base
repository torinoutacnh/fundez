
using System;
using TIGE.Contract.Repository.Models.User;
using Elect.Data.EF.Services.Map;
using TIGE.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TIGE.Repository.Maps.User
{
    public class UserTokenRequestEntityMap : StringEntityTypeConfiguration<UserTokensEntity>
    {
        public override void Map(EntityTypeBuilder<UserTokensEntity> builder)
        {
            base.Map(builder);
           
            builder.ToTable(nameof(UserWithDrawRequestEntity));

            builder.HasIndex(x => x.Quantity);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.TotalAmount);
            builder.HasIndex(x => x.Rate);

            builder.HasOne(x=>x.User).WithMany(x=>x.TokensHistories).HasForeignKey(x=>x.UserId);
            builder.HasOne(x=>x.Slot).WithMany(x=>x.Tokens).HasForeignKey(x=>x.SlotId);
        }
    }
}