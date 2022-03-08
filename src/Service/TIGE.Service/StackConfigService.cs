using Elect.DI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Service;
using TIGE.Core.Models.Configuration;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Mapper.AutoMapper.ObjUtils;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IStackConfigService))]
    public class StackConfigService : Base.Service, IStackConfigService
    {
        private readonly IRepository<StackingConfigEntity> _configurationRepo;

        public StackConfigService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _configurationRepo = unitOfWork.GetRepository<StackingConfigEntity>();
        }

        public StackingConfigEntity GetConfig(CancellationToken getRequestCancellationToken = default)
        {
            return _configurationRepo.Get().FirstOrDefault();
        }

        public Task UpdateAsync(StackConfigurationModel model, CancellationToken cancellationToken = default)
        {
            var oldConfig = _configurationRepo.Get().FirstOrDefault();

            var entity = model.MapTo<StackingConfigEntity>();

            _configurationRepo.Update(entity,
                x => x.WalletAddress,
                x => x.AboutPage,
                x => x.ContactAddress,
                x => x.MinStacking,
                x => x.MinWithDraw,
                x => x.MinWithDrawUSD,
                x => x.MinTransfer,
                x => x.DepositAmount,
                x => x.SupportEmail,
                x => x.TelegramLink,
                x => x.TIGEChartRange
                );

            UnitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public StackConfigurationModel GetEditConfig(CancellationToken getRequestCancellationToken = default)
        {
            return _configurationRepo.Get().QueryTo<StackConfigurationModel>().FirstOrDefault();
        }
    }
}
