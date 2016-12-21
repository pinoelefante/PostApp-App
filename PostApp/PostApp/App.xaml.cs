using Plugin.DeviceInfo;
using Plugin.SecureStorage;
using PostApp.Api;
using PostApp.Api.Data;
using PostApp.Services;
using PostApp.ViewModels;
using PostApp.Views;
using PushNotification.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PostApp
{
    public partial class App : Application
    {
        public App(string parameter = null)
        {
            InitializeComponent();

            if (parameter != null)
            {
                Debug.WriteLine("AVVIO APP DA NOTIFICA");
                Locator.GetService<UserNotificationService>().ShowMessageDialog("Notifica", parameter.ToString());
            }
            Locator.RegisterPages();
            Page firstPage = null;
            if (CrossSecureStorage.Current.HasKey("AccessCode"))
                firstPage = new MyMasterDetail();
            else
                firstPage = new FirstAccessPage();
            MainPage = firstPage;
        }
        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator => _locator ?? (_locator = new ViewModelLocator());
        protected override void OnStart()
        {
            if (CrossSecureStorage.Current.HasKey("AccessCode"))
            {
                if (!CrossSecureStorage.Current.HasKey("PushTokenRegOK") || (Device.OS == TargetPlatform.Android && PushRegistrableTime()))
                {
                    if (CrossSecureStorage.Current.HasKey("PushToken"))
                        CrossPushNotification.Current.Unregister();
                    CrossPushNotification.Current.Register();
                }
            }
        }
        private bool PushRegistrableTime()
        {
            if (!CrossSecureStorage.Current.HasKey("PushRegistrationTime"))
                return true;
            DateTime timeReg = new DateTime(long.Parse(CrossSecureStorage.Current.GetValue("PushRegistrationTime")));
            var span = DateTime.Now.Subtract(timeReg);
            return span.Days > 2;
        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Debug.WriteLine("App sleep");
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            Debug.WriteLine("App resume");
        }
    }
}
