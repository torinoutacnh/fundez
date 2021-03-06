using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TIGE.Core.Share.Constants
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Permission
    {
        [Display(Name = "Admin")] 
        Admin,

        [Display(Name = "Manager")] 
        Manager,

        [Display(Name = "Member")] 
        Member,
    }
}