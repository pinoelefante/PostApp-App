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
        public App()
        {
            InitializeComponent();
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
            if (CrossSecureStorage.Current.HasKey("AccessCode") && !CrossSecureStorage.Current.HasKey("PushTokenRegOK"))
            {
                if (CrossSecureStorage.Current.HasKey("PushToken")) //il token è stato già registrato nel server delle notifiche ma non in quello dell'app
                {
                    var token = CrossSecureStorage.Current.GetValue("PushToken");
                    var deviceOs = (PushDevice)Enum.Parse(typeof(PushDevice), CrossSecureStorage.Current.GetValue("PushTokenDevice"));
                    PostAppTokenReg(token, deviceOs);
                }
                else
                    CrossPushNotification.Current.Register();
            }
        }
        private async void PostAppTokenReg(string token, PushDevice device)
        {
            var postApp = Locator.GetService<IPostAppApiService>();
            var envelop = await postApp.RegistraPush(token, device);
            if (envelop.response == StatusCodes.OK)
                CrossSecureStorage.Current.SetValue("PushTokenRegOK", "OK");
            else
                Locator.GetService<UserNotificationService>().ShowMessageDialog("Registrazione notifiche fallito", "La registrazione ai servizi di notifiche è fallito");
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
