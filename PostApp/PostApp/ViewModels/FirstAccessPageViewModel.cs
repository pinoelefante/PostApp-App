using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Plugin.SecureStorage;
using PostApp.Api;
using PostApp.Api.Data;
using PostApp.Views;
using PushNotification.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace PostApp.ViewModels
{
    public class FirstAccessPageViewModel : MyViewModel
    {
        private IPostAppApiService postApp;
        private INavigationService navigation;
        public FirstAccessPageViewModel(INavigationService navigationService,IPostAppApiService _postApp)
        {
            navigation = navigationService;
            postApp = _postApp;
            Items = postApp.GetListaComuni();
        }
        public override async void NavigatedTo(object parameter = null)
        {
            RequestedAccessCode = await postApp.RequestAccessCode();
        }

        private Envelop<string> RequestedAccessCode = null;

        private ObservableCollection<Comune> _items;
        public ObservableCollection<Comune> Items
        {
            get
            {
                return _items;
            }
            set
            {
                Set(ref _items, value);
            }
        }
        private Command _accessCommand;
        private Command<Comune> _cellSelectedCommand;
        private Comune _selectedItem;
        public Comune SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                Set(ref _selectedItem, value);
            }
        }
        public Command<Comune> CellSelectedCommand =>
            _cellSelectedCommand ?? 
            (_cellSelectedCommand = new Command<Comune>(parameter => { SelectedItem = parameter; AccediEnabled = true; } ));
            
        
        public Command AccessCommand =>
            _accessCommand ??
            (_accessCommand = new Command(async () =>
            {
                if (SelectedItem != null)
                {
                    IsBusyActive = true;
                    AccediEnabled = false;

                    int retry = 0;
                    while (retry++ < 5 && RequestedAccessCode?.response != StatusCodes.OK)
                        RequestedAccessCode = await postApp.RequestAccessCode();

                    if (RequestedAccessCode.response != StatusCodes.OK)
                    {
                        Debug.WriteLine("Request access code fail");
                        AccediEnabled = true;
                        IsBusyActive = false;
                        return;
                    }

                    Debug.WriteLine($"Comune selezionato: {SelectedItem.comune}\nAccessCode: {RequestedAccessCode.content}");

                    var responseRegister = await postApp.RegisterAccessCode(RequestedAccessCode.content, SelectedItem.istat);
                    if(responseRegister.response!=StatusCodes.OK)
                    {
                        Debug.WriteLine($"Si è verificato un errore durante la registrazione. Riprova più tardi\nErrore: {responseRegister.response}");
                    }
                    else
                    {
                        CrossSecureStorage.Current.SetValue("AccessCode", RequestedAccessCode.content);
                        postApp.SetAccessCode(RequestedAccessCode.content);
                        CrossPushNotification.Current.Register();
                        App.Current.MainPage = new MyMasterDetail();
                    }
                    AccediEnabled = true;
                    IsBusyActive = false;
                }
            }));
        private bool _accediEnabled = false;
        public bool AccediEnabled
        {
            get { return _accediEnabled; }
            set { Set(ref _accediEnabled, value); }
        }
    }
}
