
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TIGE.Contract.Repository.Models.User;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using TIGE.Core.Models.Authentication;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.User;

namespace TIGE.Mapper.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // UserEntity

            CreateMap<UserEntity, UserModel>().IgnoreAllNonExisting()
                .ForMember(d => d.IsBanned, o => o.MapFrom(s => s.BannedTime != null))
                .ForMember(d => d.Permissions, o => o.MapFrom(s => s.Permission))
                .ForMember(d => d.ReferredID, o => o.MapFrom(s => s.Reference.Code))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Wallets.FirstOrDefault().AddressWallet))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email));

            CreateMap<CreateUserModel, UserEntity>().IgnoreAllNonExisting()
                .ForMember(d => d.Permission, o => o.MapFrom(s => s.ListPermission != null
                    ? string.Join(",", s.ListPermission.Select(x => (int) x))
                    : null));

            CreateMap<RegisterUserModel, UserEntity>().IgnoreAllNonExisting();

            CreateMap<UserModel, UserSignInTrackingModel>().IgnoreAllNonExisting()
                .ForMember(d => d.Permissions, o => o.MapFrom(s => s.ListPermission));
            
            CreateMap<UserModel, ProfileModel>().IgnoreAllNonExisting();
            CreateMap<UserModel, LoggedInUserModel>().IgnoreAllNonExisting();
            CreateMap<LoggedInUserModel, UserModel>().IgnoreAllNonExisting();
            CreateMap<ProfileModel, UserEntity>().IgnoreAllNonExisting();

            CreateMap<AuthorizeModel, AuthorizeWithCodeModel>().IgnoreAllNonExisting();

            CreateMap<UserBusinessEntity, BusinessDetailModel>().IgnoreAllNonExisting()
                .ForMember(d => d.Date, o => o.MapFrom(s => s.CreatedTime))
                .ForMember(d => d.From, o => o.MapFrom(s => s.FromUser.Code))
                .ForMember(d => d.To, o => o.MapFrom(s => s.ToUser.Code));

            CreateMap<UserEntity, StackUserModel>().IgnoreAllNonExisting()
                .ForMember(d => d.IsBanned, o => o.MapFrom(s => s.BannedTime != null))
                .ForMember(d => d.Permissions, o => o.MapFrom(s => s.Permission))
                .ForMember(d => d.ReferredID, o => o.MapFrom(s => s.Reference.Code))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email));
        }
    }
}