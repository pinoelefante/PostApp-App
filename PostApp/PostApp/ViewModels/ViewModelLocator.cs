using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using PostApp.Api;
using PostApp.Services;
using PostApp.Views;
using PushNotification.Plugin;
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
        public const string FirstAccessPage = "FirstAccessPage";
        public const string RegistraEditorPage = "RegistraEditorPage";
        public const string RegistraScuolaPage = "RegistraScuolaPage";
        public const string MainPage = "MainPage";
        public const string ViewNewsPage = "ViewNewsPage";
        public const string PostaNewsEditorPage = "PostaNewsEditorPage";
        public const string ViewEditorPage = "ViewEditorPage";
        public const string CercaEditorPage = "CercaEditorPage";
        public const string CittaPage = "CittaPage";
        public const string LoginPage = "LoginPage";

        private static NavigationService nav;
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            nav = new NavigationService();
            SimpleIoc.Default.Register<INavigationService>(()=>nav);
            var dbService = DependencyService.Get<ISQLite>();
            SimpleIoc.Default.Register<ISQLite>(() => dbService);
            var postApp = new PostAppAPI();
            /*TODO decommentare per email/password login
            postApp.OnAccessCodeError = () => { nav.NavigateTo(LoginPage); nav.ClearBackstack(); };
            */
            SimpleIoc.Default.Register<IPostAppApiService>(() => postApp);
            SimpleIoc.Default.Register<UserNotificationService>();
            SimpleIoc.Default.Register<ValidationService>();
            SimpleIoc.Default.Register<LocationService>();
            var killApp = DependencyService.Get<IClosingApp>();
            SimpleIoc.Default.Register<IClosingApp>(() => killApp);
            SimpleIoc.Default.Register<CrossPushNotificationListener>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<FirstAccessPageViewModel>();
            SimpleIoc.Default.Register<MyMasterDetailViewModel>();
            SimpleIoc.Default.Register<PostaNewsEditorPageViewModel>();
            SimpleIoc.Default.Register<RegistraEditorPageViewModel>();
            SimpleIoc.Default.Register<RegistraScuolaPageViewModel>();
            SimpleIoc.Default.Register<ViewEditorPageViewModel>();
            SimpleIoc.Default.Register<ViewNewsPageViewModel>();
            SimpleIoc.Default.Register<CercaEditorPageViewModel>();
            SimpleIoc.Default.Register<CittaPageViewModel>();
        }
        public void RegisterPages()
        {
            nav.Configure(ViewModelLocator.CercaEditorPage, typeof(CercaEditorPage));
            nav.Configure(ViewModelLocator.CittaPage, typeof(CittaPage));
            nav.Configure(ViewModelLocator.FirstAccessPage, typeof(FirstAccessPage));
            nav.Configure(ViewModelLocator.MainPage, typeof(MainPage));
            nav.Configure(ViewModelLocator.PostaNewsEditorPage, typeof(PostaNewsEditorPage));
            nav.Configure(ViewModelLocator.RegistraEditorPage, typeof(RegistraEditorPage));
            nav.Configure(ViewModelLocator.RegistraScuolaPage, typeof(RegistraScuolaPage));
            nav.Configure(ViewModelLocator.ViewEditorPage, typeof(ViewEditorPage));
            nav.Configure(ViewModelLocator.ViewNewsPage, typeof(ViewNewsPage));
        }
        public T GetService<T>() => ServiceLocator.Current.GetInstance<T>();
        public NavigationService NavigationService { get { return nav; } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel MainPageViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public FirstAccessPageViewModel FirstAccessVM => ServiceLocator.Current.GetInstance<FirstAccessPageViewModel>();
        public MyMasterDetailViewModel MyMasterDetailVM => ServiceLocator.Current.GetInstance<MyMasterDetailViewModel>();
        public RegistraEditorPageViewModel RegistraEditorPageVM => ServiceLocator.Current.GetInstance<RegistraEditorPageViewModel>();
        public RegistraScuolaPageViewModel RegistraScuolaPageVM => ServiceLocator.Current.GetInstance<RegistraScuolaPageViewModel>();
        public ViewNewsPageViewModel ViewNewsPageVM => ServiceLocator.Current.GetInstance<ViewNewsPageViewModel>();
        public PostaNewsEditorPageViewModel PostaNewsEditorPageVM => ServiceLocator.Current.GetInstance<PostaNewsEditorPageViewModel>();
        public ViewEditorPageViewModel ViewEditorPageVM => ServiceLocator.Current.GetInstance<ViewEditorPageViewModel>();
        public CercaEditorPageViewModel CercaEditorPageVM => ServiceLocator.Current.GetInstance<CercaEditorPageViewModel>();
        public CittaPageViewModel CittaPageVM => ServiceLocator.Current.GetInstance<CittaPageViewModel>();
    }
}
