
using System.Collections.Generic;
using System.Threading;
using TIGE.Contract.Service.Base;
using TIGE.Core.Models.Configuration;

namespace TIGE.Contract.Service
{
    public interface IConfigurationService :
        IUpdateable<ConfigurationModel>
    {
        ConfigurationModel GetConfig (CancellationToken getRequestCancellationToken = new CancellationToken());
        List<TigeHistoryModel> GetTigeHistory();
    }
}