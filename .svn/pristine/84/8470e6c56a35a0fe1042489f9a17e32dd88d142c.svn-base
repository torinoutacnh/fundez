
using System.Linq;
using AutoMapper;
using Elect.Mapper.AutoMapper.IMappingExpressionUtils;
using TIGE.Contract.Repository.Models;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Core.Models;
using TIGE.Core.Models.Configuration;

namespace TIGE.Mapper
{
    public class ConfigProfile : Profile
    {
        public ConfigProfile()
        {
            CreateMap<ConfigurationEntity, ConfigurationModel>().IgnoreAllNonExisting();
            CreateMap<ConfigurationModel, ConfigurationEntity>().IgnoreAllNonExisting();

            CreateMap<StackingConfigEntity, StackConfigurationModel>().IgnoreAllNonExisting();
            CreateMap<StackConfigurationModel, StackingConfigEntity>().IgnoreAllNonExisting();
        }
    }
}