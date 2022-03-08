using Elect.DI.Attributes;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Service;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IStackingUserService))]
    public class StackingUserService : Base.Service, IStackingUserService
    {
        // Cache

        private readonly IMemoryCache _memoryCache;

        private readonly string _cacheKeyPrefix = "User_";

        private readonly TimeSpan _cacheSliding = TimeSpan.FromHours(4);

        private readonly IServiceProvider _serviceProvider;
        private readonly IEmailService _emailService;

        private readonly IRepository<HistoryDepositEntity> _stackingDepositRepository;
        private readonly IRepository<HistoryWithdrawTokenEntity> _stackingWithdrawRepository;
        private readonly IRepository<SubscriptionEntity> _stackingSubscriptionRepository;

        public StackingUserService(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, IMemoryCache memoryCache, IEmailService emailService) : base(unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _emailService= emailService;
            _memoryCache = memoryCache;

            _stackingDepositRepository = unitOfWork.GetRepository<HistoryDepositEntity>();
            _stackingWithdrawRepository = unitOfWork.GetRepository<HistoryWithdrawTokenEntity>();
            _stackingSubscriptionRepository = unitOfWork.GetRepository<SubscriptionEntity>();
        }
    }
}
