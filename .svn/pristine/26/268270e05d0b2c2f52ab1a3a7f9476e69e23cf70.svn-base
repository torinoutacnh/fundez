#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> ErrorModel.cs </Name>
//         <Created> 18/04/2018 7:52:43 PM </Created>
//         <Key> 8d322cca-34af-4f41-b2da-6f40669d9026 </Key>
//     </File>
//     <Summary>
//         ErrorModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TIGE.Core.Share.Exceptions
{
    public class ErrorModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        ///     Unique error code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Message/Description of error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Http response code
        /// </summary>
        public int StatusCode { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();

        public ErrorModel()
        {
            
        }
        
        public ErrorModel(string code)
        {
            Code = code;
        }

        public ErrorModel(string code, string message) : this(code)
        {
            Message = message;
        }

        public ErrorModel(string code, string message, int statusCode) : this(code, message)
        {
            StatusCode = statusCode;
        }

        public ErrorModel(CoreException exception)
        {
            Code = exception.Code;
            Message = exception.Message;
            StatusCode = exception.StatusCode;
            AdditionalData = exception.AdditionalData;
        }
    }
}