using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elect.Core.DateTimeUtils;
using Elect.Core.SecurityUtils;
using Elect.Core.StringUtils;
using Elect.Data.IO;
using Elect.Data.IO.FileUtils;
using Elect.Data.IO.ImageUtils;
using Elect.Data.IO.ImageUtils.CompressUtils;
using Elect.Data.IO.ImageUtils.ResizeUtils;
using Elect.DI.Attributes;
using Elect.Mapper.AutoMapper.IQueryableUtils;
using Elect.Mapper.AutoMapper.ObjUtils;
using Elect.Web.Api.Models;
using Elect.Web.DataTable.Models;
using Elect.Web.DataTable.Models.Request;
using Elect.Web.DataTable.Models.Response;
using Elect.Web.DataTable.Processing.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using TIGE.Contract.Repository.Interfaces;
using TIGE.Contract.Repository.Models;
using TIGE.Contract.Repository.Models.User;
using TIGE.Contract.Service;
using TIGE.Core.Constants;
using TIGE.Core.Models;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Exceptions;
using TIGE.Core.Share.Models;
using TIGE.Core.Share.Models.Crypto;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using PagedRequestModel = TIGE.Core.Models.PagedRequestModel;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IBusinessService))]
    public class BusinessService : Base.Service, IBusinessService
    {

        private readonly IRepository<UserBusinessEntity> _businessRepository;
        private readonly ICommonService _commonService;
        public BusinessService(IUnitOfWork unitOfWork, ICommonService commonService) : base(unitOfWork)
        {
            _commonService = commonService;
            _businessRepository = UnitOfWork.GetRepository<UserBusinessEntity>();
        }


        public async Task<List<BusinessDetailModel>> GetMyBusinessAsync(string userId)
        {
            var results = _businessRepository.Get(x => x.ToUserId == userId).QueryTo<BusinessDetailModel>().ToList();
            return results;
        }

        public Task<double> GetTotalCommission(string userId)
        {
            var results = _businessRepository.Get(x => x.ToUserId == userId).QueryTo<BusinessDetailModel>().ToList();
            var total = results.Sum(x => x.Commission);
            return Task.FromResult(total);
        }

        public Task<DataTableResponseModel<BusinessDetailModel>> GetDataTableAsync(DataTableRequestModel model)
        {
            var query = _businessRepository.Get();
            var listData = query.QueryTo<BusinessDetailModel>();
            var result = listData.GetDataTableResponse(model);
            return Task.FromResult(result);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var business = _businessRepository.Get(x => x.Id == id).FirstOrDefault();
            if (business == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.Business.DoesNotExist);
            }

            _businessRepository.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();

            // load balance
            await _commonService.UpdateWalletBalance(business.ToUserId, cancellationToken);
        }

        public async Task<BusinessDetailModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var query = _businessRepository.Get();
            var listData = query.QueryTo<BusinessDetailModel>().FirstOrDefault();

            return listData;
        }

        public Task UpdateAsync(BusinessDetailModel model, CancellationToken cancellationToken = default)
        {
            var business = _businessRepository.Get(x => x.Id == model.Id).FirstOrDefault();
            if (business == null)
            {
                throw new CoreException(ErrorCode.BadRequest, "Not exist");
            }

            business.AmountSlot = model.AmountSlot;
            business.AmountUSD = model.AmountUSD;
            business.Commission = model.Commission;

            _businessRepository.Update(business, x=>x.AmountSlot, x=>x.AmountUSD, x=>x.Commission);
            UnitOfWork.SaveChanges();
            return Task.CompletedTask;
        }
    }
}