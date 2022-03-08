using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Elect.Core.DictionaryUtils;
using Elect.Core.ObjUtils;
using Elect.Web.Middlewares.HttpContextMiddleware;
using TIGE.Core.Share.Models.User;

namespace TIGE.Core.Share.Utils
{
    public static class LoggedInUser
    {
        private static string LoggedInUserKey => typeof(UserModel).GetTypeInfo().Assembly.FullName;
        
        public static LoggedInUserModel Current
        {
            get
            {
                if (HttpContext.Current?.Items != null)
                {
                    return
                        HttpContext.Current.Items.TryGetValue(LoggedInUserKey, out var value)
                            ? value?.ConvertTo<LoggedInUserModel>()
                            : null;
                }

                return null;
            }
            set
            {
                if (HttpContext.Current.Items?.Any() != null)
                {
                    HttpContext.Current.Items = new Dictionary<object, object>();
                }

                if (value == null)
                {
                    if (HttpContext.Current.Items?.ContainsKey(LoggedInUserKey) == true)
                    {
                        HttpContext.Current.Items.Remove(LoggedInUserKey);
                    }

                    return;
                }

                // Add to Current of LoggerInUser
                HttpContext.Current.Items.AddOrUpdate(LoggedInUserKey, value);

                // Add to User in Http Context Too
                var claims = CoreHelper.GetClaimsIdentity(value);
                
                HttpContext.Current.User = new ClaimsPrincipal(claims);
            }
        }  
    }
}