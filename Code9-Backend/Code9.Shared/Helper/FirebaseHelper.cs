using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Code9.Shared.ViewModels;
using System.Threading;
using Microsoft.Extensions.Options;

namespace Code9.Shared.Helper
{
    public static class FirebaseHelper
    {
        public static void SendNotification(List<UserTokenViewModel> tokens, string title, string body, object payLoadData)
        {
            var fireBaseThread = new Thread(new ParameterizedThreadStart(SendNotificationThreaded));
            fireBaseThread.Start(new FireBaseNotification(tokens, title, body, payLoadData));



        }



        private static void SendNotificationThreaded(object obj)
        {
            var notificationInfo = obj as FireBaseNotification;
            if (notificationInfo == null)
            {
                return;
            }
            try
            {

                string FirebaseApiKey = "AAAAKvRJhS4:APA91bHqG8cDYt-sz8meR2dtBhSwPwF-7zF2zUw3RaW3vNedQ3JNsE3zFjJZWTwKC00RyrerrTQoVp0bYQucypzFZImsF5B3hKfgrVd0LuAPkVnJp-sxyq6GeUACVAgh4iEv9vLlk5O1";
                string FirebaseSenderId = "184487085358";
                string FirebaseUrl = "https://fcm.googleapis.com/fcm/send";

                foreach (var token in notificationInfo.Tokens)
                {
                    WebRequest tRequest = WebRequest.Create(FirebaseUrl);
                    tRequest.Method = "post";
                    //serverKey - Key from Firebase cloud messaging server  
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", FirebaseApiKey));
                    //Sender Id - From firebase project setting  
                    tRequest.Headers.Add(string.Format("Sender: id={0}", FirebaseSenderId));
                    tRequest.ContentType = "application/json";
                    object payload = null;
                    if (token.Type == DeviceType.Ios)
                    {
                        payload = new
                        {
                            to = token.Token,
                            priority = "High",
                            content_available = true,
                            notification = new
                            {
                                body = notificationInfo.Body,
                                title = notificationInfo.Title,
                                badge = token.UnReadMessagesCount,
                                sound = token.IsMuted ? "" : "Enabled",
                                // click_action = "OPEN_ACTIVITY_1"
                            }
                            ,
                            data = notificationInfo.PayLoadData
                        };
                    }
                    else
                    {
                        var dictionary = GetDictionary(notificationInfo.PayLoadData);
                        dictionary.Add("IsMuted", token.IsMuted);
                        payload = new
                        {
                            to = token.Token,
                            priority = "High",
                            content_available = true,
                            data = dictionary



                        };
                    }




                    string postbody = JsonConvert.SerializeObject(payload).ToString();
                    Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                    tRequest.ContentLength = byteArray.Length;
                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                    {
                                        String sResponseFromServer = tReader.ReadToEnd();
                                    }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }



        public static IDictionary<string, object> GetDictionary(object o)
        {
            var json = JsonConvert.SerializeObject(o);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
    }
}
