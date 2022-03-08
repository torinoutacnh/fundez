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
using Hangfire;
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
using TIGE.Core.Share.Models.ProtectionFee;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;

namespace TIGE.Service
{
    [ScopedDependency(ServiceType = typeof(IProtectionFeeService))]
    public class ProtectionFeeService : Base.Service, IProtectionFeeService
    {
        private readonly IRepository<ProtectionFeeEntity> _protectionFeeRepo;

        public ProtectionFeeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _protectionFeeRepo = unitOfWork.GetRepository<ProtectionFeeEntity>();
        }


        public Task AddProtectionFee(CreateProtectionFeeModel model, CancellationToken cancellationToken = default)
        {
            var newFee = model.MapTo<ProtectionFeeEntity>();
            _protectionFeeRepo.Add(newFee);
            UnitOfWork.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<DetailProtectionFeeModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var protectionFeeRequest = _protectionFeeRepo.Get(x => x.Id == id).QueryTo<DetailProtectionFeeModel>().FirstOrDefault();
            return protectionFeeRequest;
        }

        public async Task UpdateAsync(DetailProtectionFeeModel model, CancellationToken cancellationToken = default)
        {
            var ProtectionFee = _protectionFeeRepo.Get(x => x.Id == model.Id).FirstOrDefault();

            if (ProtectionFee == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.ProtectionFee.DoesNotExist);
            }

            ProtectionFee.From = model.From;
            ProtectionFee.To = model.To;
            ProtectionFee.Fee = model.Fee;

            _protectionFeeRepo.Update(ProtectionFee, x => x.From, x => x.To, x => x.Fee);
            UnitOfWork.SaveChanges();
        }

        public Task<DataTableResponseModel<DetailProtectionFeeModel>> GetDataTableAsync(DataTableRequestModel model, CancellationToken cancellationToken = default)
        {
            var query = _protectionFeeRepo.Get();
            var listData = query.QueryTo<DetailProtectionFeeModel>();
            var result = listData.GetDataTableResponse(model);
            return Task.FromResult(result);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var protectionFee = _protectionFeeRepo.Get(x => x.Id == id).FirstOrDefault();
            if (protectionFee == null)
            {
                throw new CoreException(ErrorCode.BadRequest, Messages.ProtectionFee.DoesNotExist);
            }

            _protectionFeeRepo.DeleteWhere(x => x.Id == id);
            UnitOfWork.SaveChanges();
        }

        public List<DetailProtectionFeeModel> GetAll()
        {
            var result = _protectionFeeRepo.Get().QueryTo<DetailProtectionFeeModel>().OrderBy(x => x.From)
                .ToList();
            return result;
        }
    }
}