using System;
using System.Collections.Generic;
using AngleSharp.Text;

namespace TIGE.Core.Share.Models
{
    public class AuthyModel
    {
  
       public List<WebHookAuthyItem> events { get; set; }
       public string webhook_id { get; set; }

    }

    public class WebHookAuthyItem
    {
        public string Event { get; set; }
        public string time { get; set; }
        public WebHookAuthyObject objects { get; set; }
        public Object request { get; set; }
        public bool @public { get; set; }
    }

    public class WebHookAuthyObject
    {
        public Object app { get; set; }
        public WebHookAuthyRegistration registration { get; set; }
        public Object user { get; set; }
    }

    public class WebHookAuthyRegistration
    {
        public int s_app_id { get; set; }
        public int s_authy_id { get; set; }
        public string s_custom_id { get; set; }
    }


    public class AuthyRegistrationResponseModel
    {
        public RegistrationInfoModel registration { get; set; }
        public bool success { get; set; }
    }
    public class RegistrationInfoModel
    {
        public string status { get; set; }
        public int authy_id { get; set; }
    }



    public class VerifyAuthyResponseModel
    {
        public string message { get; set; }
        public bool success { get; set; }
    }
}