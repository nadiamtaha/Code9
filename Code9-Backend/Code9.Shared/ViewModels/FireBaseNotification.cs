using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
    public class FireBaseNotification
    {
        public FireBaseNotification()
        {

        }
        public FireBaseNotification(List<UserTokenViewModel> tokens, string title, string body, object payLoadData)
        {
            Tokens = tokens;
            Title = title;
            Body = body;
            PayLoadData = payLoadData;
        }



        public List<UserTokenViewModel> Tokens { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Id { get; set; }
        public object PayLoadData { get; set; }
    }
}
