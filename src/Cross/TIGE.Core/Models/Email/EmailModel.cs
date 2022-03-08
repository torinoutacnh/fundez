#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> EmailModel.cs </Name>
//         <Created> 21/04/2018 10:25:53 PM </Created>
//         <Key> 81e1bf91-27fb-4640-ba02-37a922eabb48 </Key>
//     </File>
//     <Summary>
//         EmailModel.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using TIGE.Core.Constants;

namespace TIGE.Core.Models.Email
{
    public class EmailModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EmailTemplate Template { get; set; }

        public string ApplicationName { get; set; }
        public string UserId { get; set; }

        public string Url { get; set; }
        public string Code { get; set; }

        public DateTimeOffset? ExpireTime { get; set; }

        public Object AdditionData { get; set; }
    }
}