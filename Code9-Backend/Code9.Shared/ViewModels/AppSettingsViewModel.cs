using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
    public class AppSettingsViewModel
    {
        public string Secret { get; set; }
        public string BaseUrl { get; set; }
        public int TokenExpirationDays { get; set; }
        public string Environment { get; set; }
        public string FirebaseApiKey { get; set; }
        public string FirebaseSenderId { get; set; }


    }
}
