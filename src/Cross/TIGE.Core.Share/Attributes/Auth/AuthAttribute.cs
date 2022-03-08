using System;
using TIGE.Core.Share.Constants;

namespace TIGE.Core.Share.Attributes.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthAttribute : Attribute
    {
        public Permission[] Permissions { get; }

        public AuthAttribute(params Permission[] permission)
        {
            Permissions = permission;
        }
    }
}