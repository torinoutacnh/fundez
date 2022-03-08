
using System.Linq;
using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using TIGE.Contract.Repository.Models;
using TIGE.Contract.Repository.Models.User;
using TIGE.Core.Models;
using TIGE.Core.Models.Configuration;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Models.Token;
using TIGE.Core.Share.Models.Slot;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Core.Share.Models.Stack;

namespace TIGE.Mapper
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<UserSellTokenEntity, DetailSellTokenModel>().IgnoreAllNonExisting()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.Code))
                .ForMember(d => d.ApproveName, o => o.MapFrom(s => s.User.FullName))
                .ForMember(d => d.UserEmail, o => o.MapFrom(s => s.User.Email));

            CreateMap<DetailSellTokenModel, UserSellTokenEntity>().IgnoreAllNonExisting();
            CreateMap<SubmitTokenModel, UserSellTokenEntity>().IgnoreAllNonExisting();

            CreateMap<StackHistoryEntity, StackHistoryModel>().IgnoreAllNonExisting();
            CreateMap<SubscriptionEntity, SubscriptionModel>().IgnoreAllNonExisting();

            CreateMap<HistoryRefundEntity, RewardModel>().IgnoreAllNonExisting();
        }
    }
}