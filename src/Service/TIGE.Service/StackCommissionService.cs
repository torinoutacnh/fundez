using Elect.DI.Attributes;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using Elect.Web.DataTable.Processing.Response;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Repository.Models.User;
using TIGE.Contract.Service;
using TIGE.Core.Share.Models.Stack;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IStackCommissionService))]
    public class StackCommissionService : Base.Service, IStackCommissionService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ICommonService _common;

        private readonly string _cacheKeyPrefix = "User_";
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IRepository<StackingWalletEntity> _stackingWalletRepository;
        private readonly IRepository<StackCommissionEntity> _stackingCommissionRepository;
        private readonly IRepository<StackCommissionRateEntity> _stackingCommissionRateRepository;

        public StackCommissionService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, ICommonService common) : base(unitOfWork)
        {
            _memoryCache = memoryCache;
            _common = common;

            _userRepository = unitOfWork.GetRepository<UserEntity>();
            _stackingWalletRepository = unitOfWork.GetRepository<StackingWalletEntity>();
            _stackingCommissionRepository = unitOfWork.GetRepository<StackCommissionEntity>();
            _stackingCommissionRateRepository = unitOfWork.GetRepository<StackCommissionRateEntity>();

        }

        public Task<StackCommissionEntity> CreateCommissionAsync(StackCommissionEntity model, CancellationToken cancellationToken = default)
        {
            var comm = _stackingCommissionRepository.Add(model);
            UnitOfWork.SaveChanges();

            var user = _stackingWalletRepository.Get(x=>x.Id==model.WalletId).FirstOrDefault();
            _common.UpdateStackWalletBalance(user.UserId);

            return Task.FromResult(model);
        }

        public Task<StackCommissionRateEntity> CreateCommissionRateAsync(StackCommissionRateEntity model, CancellationToken cancellationToken = default)
        {
            var comm = _stackingCommissionRateRepository.Add(model);
            UnitOfWork.SaveChanges();

            return Task.FromResult(model);
        }

        public Task DeleteCommissionAsync(string id, CancellationToken cancellationToken = default)
        {
            var comm = _stackingCommissionRepository.Get(x=>x.Id == id).FirstOrDefault();
            _stackingCommissionRepository.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            var user = _stackingWalletRepository.Get(x => x.Id == comm.WalletId).FirstOrDefault();
            _common.UpdateStackWalletBalance(user.UserId);

            return Task.CompletedTask;
        }

        public Task DeleteCommissionRateAsync(string id, CancellationToken cancellationToken = default)
        {
            _stackingCommissionRateRepository.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<StackCommissionEntity> GetCommissionAsync(string id, CancellationToken cancellationToken = default)
        {
            var value = _stackingCommissionRepository.Get(x => x.Id == id).FirstOrDefault();

            return Task.FromResult(value);
        }

        public Task<DataTableResponseModel<CommissionModel>> GetCommissionDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _stackingCommissionRepository.Get();
            var list = query.QueryTo<CommissionModel>();
            var result = list.GetDataTableResponse(model);

            var data = result.Data.Select(x => (CommissionModel)x);

            foreach (var item in data)
            {
                var wallet = _stackingWalletRepository.Get(x => x.Id == item.WalletId).FirstOrDefault();
                var user = _userRepository.Get(x => x.Id == wallet.UserId).FirstOrDefault();
                item.UserId = user.Code;
                item.Email = user.Email;
            }

            return Task.FromResult(result);
        }

        public Task<StackCommissionRateEntity> GetCommissionRateAsync(string id, CancellationToken cancellationToken = default)
        {
            var value = _stackingCommissionRateRepository.Get(x => x.Id == id).FirstOrDefault();

            return Task.FromResult(value);
        }

        public Task<DataTableResponseModel<CommissionRateModel>> GetCommissionRateDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _stackingCommissionRateRepository.Get();
            var list = query.QueryTo<CommissionRateModel>();
            var result = list.GetDataTableResponse(model);
            return Task.FromResult(result);
        }

        public Task<string> UpdateCommissionAsync(StackCommissionEntity model, CancellationToken cancellationToken = default)
        {
            var comm = _stackingCommissionRepository.Get(x => x.Id == model.Id).FirstOrDefault();

            comm.Level = model.Level;
            comm.Amount = model.Amount;
            comm.FromStack = model.FromStack;
            comm.FromReward = model.FromReward;
            comm.LastUpdatedTime = DateTimeOffset.UtcNow;

            _stackingCommissionRepository.Update(comm, x => x.Amount, x => x.LastUpdatedTime, x => x.FromStack, x => x.FromReward, x => x.Level);
            UnitOfWork.SaveChanges();

            var user = _stackingWalletRepository.Get(x => x.Id == model.WalletId).FirstOrDefault();
            _common.UpdateStackWalletBalance(user.UserId);

            return Task.FromResult(comm.Id);
        }

        public Task<string> UpdateCommissionRateAsync(StackCommissionRateEntity model, CancellationToken cancellationToken = default)
        {
            var comm = _stackingCommissionRateRepository.Get(x => x.Id == model.Id).FirstOrDefault();

            comm.Level = model.Level;
            comm.Condition = model.Condition;
            comm.Rate = model.Rate;
            comm.LastUpdatedTime = DateTimeOffset.UtcNow;

            _stackingCommissionRateRepository.Update(comm, x => x.Rate, x => x.LastUpdatedTime, x => x.Condition, x => x.Level);
            UnitOfWork.SaveChanges();


            return Task.FromResult(comm.Id);
        }
    }
}
