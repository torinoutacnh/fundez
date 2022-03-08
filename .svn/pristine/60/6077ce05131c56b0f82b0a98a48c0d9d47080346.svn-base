using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TIGE.Contract.Repository.Models.Stacking;
using TIGE.Contract.Service.Base;
using TIGE.Core.Models.Configuration;

namespace TIGE.Contract.Service
{
    public interface IStackConfigService : IUpdateable<StackConfigurationModel>
    {
        StackingConfigEntity GetConfig(CancellationToken getRequestCancellationToken = new CancellationToken());
        StackConfigurationModel GetEditConfig(CancellationToken getRequestCancellationToken = default);
    }
}
