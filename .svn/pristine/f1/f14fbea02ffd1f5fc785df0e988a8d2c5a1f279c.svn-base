
using System;
using TIGE.Contract.Repository.Models.User;
using Elect.Data.EF.Services.Map;
using TIGE.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TIGE.Repository.Maps.User
{
    public class UserWithDrawRequestEntityMap : StringEntityTypeConfiguration<UserWithDrawRequestEntity>
    {
        public override void Map(EntityTypeBuilder<UserWithDrawRequestEntity> builder)
        {
            base.Map(builder);
           
            builder.ToTable(nameof(UserWithDrawRequestEntity));

            builder.HasIndex(x => x.AmountETH);
            builder.HasIndex(x => x.AmountUSD);
            builder.HasIndex(x => x.WalletId);
            builder.HasIndex(x => x.FromWalletId);
            builder.HasIndex(x => x.Rate);

            builder.HasOne(x=>x.ApproveBy).WithMany(x=>x.ApproveRequest).HasForeignKey(x=>x.ApproveById);
            builder.HasOne(x=>x.RejectBy).WithMany(x=>x.RejectRequest).HasForeignKey(x=>x.RejectById);
            builder.HasOne(x=>x.Wallet).WithMany(x=>x.WithdrawRequests).HasForeignKey(x=>x.WalletId);

        }
    }
}