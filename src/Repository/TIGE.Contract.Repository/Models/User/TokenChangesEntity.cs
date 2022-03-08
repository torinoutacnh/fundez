using TIGE.Contract.Repository.Models.Application;
using System;
using System.Collections.Generic;
using Elect.Core.StringUtils;
using TIGE.Core.Constants;
using TIGE.Core.Share.Constants;

namespace TIGE.Contract.Repository.Models.User
{
    public class TokenChangesEntity : Entity
    {
        public double Rate { get; set; }
    }
}