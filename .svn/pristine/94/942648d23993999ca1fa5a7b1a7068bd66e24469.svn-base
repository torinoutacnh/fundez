
using System.Linq;
using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using TIGE.Contract.Repository.Models;
using TIGE.Contract.Repository.Models.User;
using TIGE.Core.Models;
using TIGE.Core.Models.Configuration;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Models.ProtectionFee;
using TIGE.Core.Share.Models.Slot;

namespace TIGE.Mapper
{
    public class ProtectionFeeProfile : Profile
    {
        public ProtectionFeeProfile()
        {
            CreateMap<ProtectionFeeEntity, DetailProtectionFeeModel>().IgnoreAllNonExisting();
            CreateMap<CreateProtectionFeeModel, ProtectionFeeEntity>().IgnoreAllNonExisting();
            CreateMap<DetailProtectionFeeModel, ProtectionFeeEntity>().IgnoreAllNonExisting();
        }
    }
}