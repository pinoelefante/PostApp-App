using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Plugin.SecureStorage;
using PostApp.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.ViewModels
{
    public class MyMasterDetailViewModel : MyViewModel
    {
        INavigationService _navigationService;
        IPostAppApiService postApp;

        public RelayCommand<string> NavigateCommand { get; set; }
        
        public MyMasterDetailViewModel(INavigationService navigationService, IPostAppApiService pa)
        {
            _navigationService = navigationService;
            postApp = pa;
            NavigateCommand = new RelayCommand<string>(Navigate);
            pa.SetAccessCode(CrossSecureStorage.Current.GetValue("AccessCode"));
        }
        public void Navigate(string name)
        {
            _navigationService.NavigateTo(name);
        }
    }
}
