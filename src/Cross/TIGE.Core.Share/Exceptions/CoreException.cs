#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> CoreException.cs </Name>
//         <Created> 18/04/2018 8:13:29 PM </Created>
//         <Key> 85cf22c1-0858-43f3-9b87-bfbf92cdedc4 </Key>
//     </File>
//     <Summary>
//         CoreException.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TIGE.Core.Share.Exceptions
{
    public class CoreException : Exception
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        
        public string Code { get; }

        public int StatusCode { get; set; }

        [JsonExtensionData] public Dictionary<string, object> AdditionalData { get; set; }

        public CoreException(string code, string message = "", int statusCode = StatusCodes.Status400BadRequest)
            : base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }
    }
}