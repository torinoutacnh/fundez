#region	License

//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> Formatting.cs </Name>
//         <Created> 18/04/2018 4:49:30 PM </Created>
//         <Key> f2c1e405-fcce-4ccf-9c35-60218b24109c </Key>
//     </File>
//     <Summary>
//         Formatting.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------

#endregion License

using System.Globalization;
using Newtonsoft.Json;

namespace TIGE.Core.Share.Constants
{
    public static class Formattings
    {
        /// <summary>
        ///     Config use datetime with TimeZone. Default is "UTC", See more: https://msdn.microsoft.com/en-us/library/gg154758.aspx 
        /// </summary>
        public const string TimeZone = "SE Asia Standard Time"; // "UTC"

        public const string DateFormat = "dd/MM/yyyy";

        public const string TimeFormat  = "hh:mm:ss tt";

        public const string DateTimeFormat  = "dd/MM/yyyy hh:mm:ss tt";
        
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Elect.Core.Constants.Formatting.JsonSerializerSettings.Formatting,
            NullValueHandling = Elect.Core.Constants.Formatting.JsonSerializerSettings.NullValueHandling,
            MissingMemberHandling = Elect.Core.Constants.Formatting.JsonSerializerSettings.MissingMemberHandling,
            DateFormatHandling = Elect.Core.Constants.Formatting.JsonSerializerSettings.DateFormatHandling,
            ReferenceLoopHandling = Elect.Core.Constants.Formatting.JsonSerializerSettings.ReferenceLoopHandling,
            ContractResolver = Elect.Core.Constants.Formatting.JsonSerializerSettings.ContractResolver,
            DateFormatString = Elect.Core.Constants.Formatting.JsonSerializerSettings.DateFormatString,
            Culture = CultureInfo.CurrentCulture,
        };
    }
}