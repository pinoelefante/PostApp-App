using PostApp.ViewModels;
using PostApp.Views;
using System;
using System.Collections.Generic;
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
            if (Plugin.SecureStorage.CrossSecureStorage.Current.HasKey("AccessCode"))
            {
                firstPage = new MyMasterDetail();
            }
            else
            {
                firstPage = new NavigationPage(new FirstAccessPage());
                Locator.NavigationService.Initialize((NavigationPage)firstPage);
            }
            //Locator.NavigationService.NavigateTo(ViewModelLocator.FirstPage);
            
            MainPage = firstPage;
        }
        private static ViewModelLocator _locator;

        public static ViewModelLocator Locator
        {
            get
            {
                return _locator ?? (_locator = new ViewModelLocator());
            }
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
