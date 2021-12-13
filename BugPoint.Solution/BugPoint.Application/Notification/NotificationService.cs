using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace BugPoint.Application.Notification
{
    public class NotificationService : INotificationService
    {
        public static string NotificationListKey => "NotificationToast";

        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NotificationService(ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContextAccessor)
        {
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public void InformationNotification(string alertTitle, NotificationType type, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.Info, message, encode);
        }

        public void SuccessNotification(string alertTitle, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.Success, message, encode);
        }

        public void WarningNotification(string alertTitle, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.Warning, message, encode);
        }

        public void DangerNotification(string alertTitle, string message, bool encode = true)
        {
            PrepareTempData(alertTitle, NotificationType.Danger, message, encode);
        }

        protected void PrepareTempData(string alertTitle, NotificationType type, string message, bool encode = true)
        {
            var context = _httpContextAccessor.HttpContext;
            var tempData = _tempDataDictionaryFactory.GetTempData(context);

            //Messages have stored in a serialized list
            var messages = tempData.ContainsKey(NotificationListKey)
                ? JsonConvert.DeserializeObject<IList<NotificationData>>(tempData[NotificationListKey].ToString() ?? string.Empty)
                : new List<NotificationData>();

            messages.Add(new NotificationData
            {
                Message = message,
                Type = type,
                Encode = encode,
                AlertTitle = alertTitle
            });

            tempData[NotificationListKey] = JsonConvert.SerializeObject(messages);
        }


    }
}