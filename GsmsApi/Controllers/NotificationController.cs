using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using PushNotification.Models;
using PushNotification.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GsmsApi.Controllers
{
    //PhucVVT
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1.0/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private IHostingEnvironment _env;
        public NotificationController(INotificationService notificationService, IHostingEnvironment env)
        {
            _notificationService = notificationService;
            _env = env;
        }

        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendNotification(NotificationModel notificationModel)
        {
            var result = await _notificationService.SendNotification(notificationModel, _env);
            return Ok(result);
        }
    }
}
