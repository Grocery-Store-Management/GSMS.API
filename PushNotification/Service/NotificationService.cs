using CorePush.Google;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using PushNotification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static PushNotification.Models.GoogleNotification;

namespace PushNotification.Service
{
    public interface INotificationService
    {
        Task<string> SendNotification(NotificationModel notificationModel, IHostingEnvironment env);
    }

    public class NotificationService : INotificationService
    {
        public async Task<string> SendNotification(NotificationModel notificationModel, IHostingEnvironment env)
        {
            FirebaseApp app = null;
            var path = env.ContentRootPath + "\\private-key.json";
            try
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                }, "myApp");
            }
            catch (Exception ex)
            {
                app = FirebaseApp.GetInstance("myApp");
            }

            var token = notificationModel.Token;
            var fcm = FirebaseAdmin.Messaging.FirebaseMessaging.GetMessaging(app);

            Message message = new Message()
            {
                Notification = new Notification
                {
                    Title = notificationModel.Title,
                    Body = notificationModel.Body
                },
                Token = token
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            var response = await fcm.SendAsync(message);
            
            // Response is a message ID string.
            return response;
        }
    
    }
}
