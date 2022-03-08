
using System.Linq;
using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using TIGE.Contract.Repository.Models;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Repository.Models.User;
using TIGE.Core.Models;
using TIGE.Core.Models.Configuration;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Models.Slot;
using TIGE.Core.Share.Models.Stack;

namespace TIGE.Mapper
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<UserWalletEntity, WalletModel>().IgnoreAllNonExisting()
                .ForMember(d => d.Address, o => o.MapFrom(s => s.AddressWallet))
                .ForMember(d => d.Balance, o => o.MapFrom(s => s.AmountUSD))
                .ForMember(d => d.PublicKey, o => o.MapFrom(s => s.PublicKey))
                .ForMember(d => d.PrivateKey, o => o.MapFrom(s => s.PrivateKey));

            CreateMap<UserDepositRequestEntity, WalletDepositModel>().IgnoreAllNonExisting()
                .ForMember(d => d.AmountETH, o => o.MapFrom(s => s.AmountETH))
                .ForMember(d => d.AmountUSD, o => o.MapFrom(s => s.AmountUSD))
                .ForMember(d => d.Rate, o => o.MapFrom(s => s.Rate))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.TxHash, o => o.MapFrom(s => s.TxHash))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Wallet.User.Code))
                .ForMember(d => d.UserEmail, o => o.MapFrom(s => s.Wallet.User.Email))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
                .ForMember(d => d.Time, o => o.MapFrom(s => s.CreatedTime));

            CreateMap<CreateWithdrawRequestModel, UserWithDrawRequestEntity>().IgnoreAllNonExisting()
                .ForMember(d => d.AmountUSD, o => o.MapFrom(s => s.Amount));

            CreateMap<UserWithDrawRequestEntity, WithdrawRequestModel>().IgnoreAllNonExisting()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Wallet.User.Code))
                .ForMember(d => d.UserEmail, o => o.MapFrom(s => s.Wallet.User.Email))
                .ForMember(d => d.ToAddressWallet, o => o.MapFrom(s => s.Wallet.AddressWallet))
                .ForMember(d => d.AmountUSD, o => o.MapFrom(s => s.AmountUSD));

            CreateMap<UserSlotsEntity, DetailSlotRequestModel>().IgnoreAllNonExisting()
                .ForMember(d => d.ApproveName, o => o.MapFrom(s => s.Approve.FullName))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Code))
                .ForMember(d => d.UserEmail, o => o.MapFrom(s => s.User.Email))
                .ForMember(d => d.Time, o => o.MapFrom(s => s.CreatedTime));

            CreateMap<HistoryDepositEntity, StackDepositModel>().IgnoreAllNonExisting();
            CreateMap<HistoryWithdrawTokenEntity, StackWithdrawModel>().IgnoreAllNonExisting();
            CreateMap<UserWithDrawRequestEntity, TransferRequestModel>().IgnoreAllNonExisting()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Wallet.User.Code))
                .ForMember(d => d.UserEmail, o => o.MapFrom(s => s.Wallet.User.Email))
                .ForMember(d => d.AmountTige, o => o.MapFrom(s => s.AmountUSD));
            CreateMap<HistoryWithdrawUSDEntity, StackWithdrawUSDModel>().IgnoreAllNonExisting();
            CreateMap<StackCommissionEntity, CommissionModel>().IgnoreAllNonExisting();
            CreateMap<StackCommissionRateEntity, CommissionRateModel>().IgnoreAllNonExisting();
        }
    }
}