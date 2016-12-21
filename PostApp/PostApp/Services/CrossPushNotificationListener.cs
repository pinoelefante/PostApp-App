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
using System.ComponentModel;
using Plugin.SecureStorage;
using Plugin.DeviceInfo;

namespace PostApp.Services
{
    public class CrossPushNotificationListener : IPushNotificationListener
    {
        //private IPostAppApiService postApp => App.Locator.GetService<IPostAppApiService>();

        private Action<string> OnErrorAction = (x) => App.Locator.GetService<UserNotificationService>().ShowMessageDialog("Registrazione notifiche fallito", x);
        public void OnError(string message, DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push notification error - {0}", message));
            OnErrorAction?.Invoke(message);
        }
        public Action<JObject> OnMessageAction = (message) =>
        {
            //TODO Mostrare notifica push
            Debug.WriteLine("è arrivata una notifica\n" + message.ToString());
        };
        public void OnMessage(JObject values, DeviceType deviceType)
        {
            Debug.WriteLine("Message Arrived");
            OnMessageAction?.Invoke(values);
        }
        public Action<string,PushDevice> OnRegisteredAction = async (token, device) =>
        {
            CrossSecureStorage.Current.SetValue("PushToken", token);
            CrossSecureStorage.Current.SetValue("PushTokenDevice", device.ToString());
            CrossSecureStorage.Current.SetValue("PushRegistrationTime", DateTime.Now.ToBinary().ToString())
            var postApp = App.Locator.GetService<IPostAppApiService>();
            var envelop = await postApp.RegistraPush(token, device, CrossDeviceInfo.Current.Id);
            if (envelop.response == StatusCodes.OK)
                CrossSecureStorage.Current.SetValue("PushTokenRegOK", "OK");
            else
                App.Locator.GetService<UserNotificationService>().ShowMessageDialog("Registrazione notifiche fallito", "La registrazione ai servizi di notifiche è fallito");
        };
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
            OnRegisteredAction?.Invoke(token, device);
        }
        public Action<PushDevice> OnUnregisteredAction = async (device) =>
        {
            var postApp = App.Locator.GetService<IPostAppApiService>();
            var envelop = await postApp.UnRegistraPush(CrossSecureStorage.Current.GetValue("PushToken"), device, CrossDeviceInfo.Current.Id);
            if (envelop.response == StatusCodes.OK)
            {
                CrossSecureStorage.Current.DeleteKey("PushTokenRegOK");
                CrossSecureStorage.Current.DeleteKey("PushToken");
            }
        };
        public void OnUnregistered(DeviceType deviceType)
        {
            Debug.WriteLine("Push Notification - Device Unnregistered");
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
            OnUnregisteredAction?.Invoke(device);
        }

        public bool ShouldShowNotification()
        {
            return true;
        }
    }
}
