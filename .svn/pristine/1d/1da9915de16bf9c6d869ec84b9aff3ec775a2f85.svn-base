#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> ProjectService.cs </Name>
//         <Created> 20/04/2018 10:02:54 AM </Created>
//         <Key> 1b04f0c4-f737-4407-98d0-5d6ea6c49a7a </Key>
//     </File>
//     <Summary>
//         ProjectService.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using System;
using System.Collections.Generic;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Service;
using Elect.DI.Attributes;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Web.Api.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using Elect.Web.DataTable.Processing.Response;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.StringUtils;
using TIGE.Contract.Repository.Models;
using TIGE.Core.Constants;
using TIGE.Core.Models;
using TIGE.Core.Models.Configuration;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IConfigurationService))]
    public class ConfigurationService : Base.Service, IConfigurationService
    {
        private readonly IRepository<ConfigurationEntity> _configurationRepo;
        private readonly IRepository<TigeHistoryEntity> _tigeHistoryRepo;

        public ConfigurationService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _configurationRepo = unitOfWork.GetRepository<ConfigurationEntity>();
            _tigeHistoryRepo = unitOfWork.GetRepository<TigeHistoryEntity>();
        }

        public Task UpdateAsync(ConfigurationModel model, CancellationToken cancellationToken = default)
        {
            var oldConfig = _configurationRepo.Get().FirstOrDefault();

            if (oldConfig != null && Math.Abs(model.TokenPrice - oldConfig.TokenPrice) > 0)
            {
                _tigeHistoryRepo.Add(new TigeHistoryEntity()
                {
                    Price = model.TokenPrice
                });
                UnitOfWork.SaveChanges();
            }

            var entity = model.MapTo<ConfigurationEntity>();

            entity.TokenPrice = Math.Round(entity.TokenPrice, 4);

            _configurationRepo.Update(entity, 
                x=>x.AboutPage,
                x=>x.ContactAddress,
                x=>x.HelloPage,
                x=>x.SalePhone1,
                x=>x.SalePhone2,
                x=>x.SlotPrice,
                x=>x.SlotToToken,
                x=>x.TokenPrice,
                x=>x.MinWithdraw,
                x=>x.TotalDepositWallet,
                x=>x.Level1,
                x=>x.Level2,
                x=>x.Level3,
                x=>x.Level4,
                x=>x.Level5,
                x=>x.Level6,
                x=>x.Level7,
                x=>x.Level8,
                x=>x.Level9,
                x=>x.Level10,
                x=>x.ConditionLevel1,
                x=>x.ConditionLevel2,
                x=>x.ConditionLevel3,
                x=>x.ConditionLevel4,
                x=>x.ConditionLevel5,
                x=>x.ConditionLevel6,
                x=>x.ConditionLevel7,
                x=>x.ConditionLevel8,
                x=>x.ConditionLevel9,
                x=>x.ConditionLevel10,
                x=>x.SupportEmail,
                x=>x.TelegramLink,
                x=>x.TIGEChartRange
                );

            UnitOfWork.SaveChanges();
            return Task.CompletedTask;
        }
      
        public ConfigurationModel GetConfig(CancellationToken getRequestCancellationToken = new CancellationToken())
        {
            return _configurationRepo.Get().QueryTo<ConfigurationModel>().ToList().FirstOrDefault();
        }

        public List<TigeHistoryModel> GetTigeHistory()
        {
            var result = _tigeHistoryRepo.Get().Select(x => new TigeHistoryModel()
            {
                CreatedTime = x.CreatedTime,
                Price = x.Price
            }).OrderBy(x=>x.CreatedTime).ToList();

            return result;
        }
    }
}