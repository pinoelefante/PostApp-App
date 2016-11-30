using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using PostApp.Api;
using PostApp.Services;
using PostApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PostApp.ViewModels
{
    public class ViewModelLocator
    {
        public const string MyMasterDetailPage = "MyMasterDetail";
        public const string FirstAccessPage = "FirstAccessPage";
        public const string RegistraEditorPage = "RegistraEditorPage";
        public const string RegistraScuolaPage = "RegistraScuolaPage";
        public const string MainPage = "MainPage";

        private static NavigationService nav;
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            nav = new NavigationService();
            SimpleIoc.Default.Register<INavigationService>(()=>nav);
            var dbService = DependencyService.Get<ISQLite>();
            SimpleIoc.Default.Register<ISQLite>(() => dbService);
            SimpleIoc.Default.Register<IPostAppApiService, PostAppAPI>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<FirstAccessPageViewModel>();
            SimpleIoc.Default.Register<MyMasterDetailViewModel>();
            SimpleIoc.Default.Register<RegistraEditorPageViewModel>();
            SimpleIoc.Default.Register<RegistraScuolaPageViewModel>();
        }
        public void RegisterPages()
        {
            nav.Configure(ViewModelLocator.MyMasterDetailPage, typeof(MyMasterDetail));
            nav.Configure(ViewModelLocator.FirstAccessPage, typeof(FirstAccessPage));
            nav.Configure(ViewModelLocator.MainPage, typeof(MainPage));
            nav.Configure(ViewModelLocator.RegistraEditorPage, typeof(RegistraEditorPage));
            nav.Configure(ViewModelLocator.RegistraScuolaPage, typeof(RegistraScuolaPage));
        }
        public NavigationService NavigationService { get { return nav; } }
        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public FirstAccessPageViewModel FirstAccessVM => ServiceLocator.Current.GetInstance<FirstAccessPageViewModel>();
        public MyMasterDetailViewModel MyMasterDetailVM => ServiceLocator.Current.GetInstance<MyMasterDetailViewModel>();
        public RegistraEditorPageViewModel RegistraEditorPageVM => ServiceLocator.Current.GetInstance<RegistraEditorPageViewModel>();
        public RegistraScuolaPageViewModel RegistraScuolaPageVM => ServiceLocator.Current.GetInstance<RegistraScuolaPageViewModel>();
        
    }
}
