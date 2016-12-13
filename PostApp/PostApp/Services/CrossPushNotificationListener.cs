using PushNotification.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PushNotification.Plugin.Abstractions;
using System.Diagnostics;
using PostApp.Api;
using PostApp.Api.Data;
using Microsoft.Practices.ServiceLocation;

namespace PostApp.Services
{
    public class CrossPushNotificationListener : IPushNotificationListener
    {
        public IPostAppApiService postApp { get; set; }
        
        public CrossPushNotificationListener()
        {
            //postApp = ServiceLocator.Current.GetInstance<IPostAppApiService>();
        }
        
        public void OnError(string message, DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push notification error - {0}", message));
        }

        public void OnMessage(JObject values, DeviceType deviceType)
        {
            Debug.WriteLine("Message Arrived");
        }

        public void OnRegistered(string token, DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push Notification - Device Registered - Token : {0}", token));
            PushDevice device = PushDevice.NOT_SET;
            switch (deviceType)
            {
                case DeviceType.Android:
                    device = PushDevice.ANDROID;
                    break;
                case DeviceType.iOS:
                    device = PushDevice.IOS;
                    break;
                case DeviceType.Windows:
                    device = PushDevice.WINDOWS_UWP;
                    break;
            }
            postApp.RegistraPush(token, device);
        }

        public void OnUnregistered(DeviceType deviceType)
        {
            Debug.WriteLine("Push Notification - Device Unnregistered");
        }

        public bool ShouldShowNotification()
        {
            return true;
        }
    }
}
