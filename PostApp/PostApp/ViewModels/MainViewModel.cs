using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PostApp.Api;
using PostApp.Api.Data;
using PostApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PostApp.ViewModels
{
    public class MainViewModel : MyViewModel
    {
        private IPostAppApiService postApp;
        private INavigationService navigation;
        private UserNotificationService notification;
        private IClosingApp closeApp;
        public MainViewModel(IPostAppApiService _api, INavigationService _nav, UserNotificationService _not, IClosingApp _ca)
        {
            postApp = _api;
            navigation = _nav;
            notification = _not;
            closeApp = _ca;
        }
        public ObservableCollection<News> ElencoNews { get; } = new ObservableCollection<News>();
        private int? editorLastId = null;

        public override void NavigatedTo(object parameter = null)
        {
            if (!ElencoNews.Any())
                GetNewsFrom();
            else
                GetNewsTo();
        }
        private async void GetNewsFrom()
        {
            IsBusyActive = true;
            var envelop = await postApp.GetAllMyNewsFrom(editorLastId);
            if (envelop.response == StatusCodes.OK)
            {
                foreach (var item in envelop.content)
                    ElencoNews.Add(item);
                if (ElencoNews.Any())
                    editorLastId = ElencoNews.Last().id;
                RaisePropertyChanged(() => ElencoNews);
            }
            IsBusyActive = false;
        }
        private async void GetNewsTo()
        {
            IsBusyActive = true;
            int id = ElencoNews.First().id;
            var envelop = await postApp.GetAllMyNewsTo(id);
            if(envelop.response == StatusCodes.OK)
            {
                for (int i = envelop.content.Count - 1; i >= 0; i--)
                    ElencoNews.Insert(0, envelop.content[i]);
                RaisePropertyChanged(() => ElencoNews);
            }
            else
            {
                Debug.WriteLine("Errore: " + envelop.response);
            }
            IsBusyActive = false;
        }
        private RelayCommand<News> _apriNewsCmd;
        public RelayCommand<News> ApriNews =>
            _apriNewsCmd ??
            (_apriNewsCmd = new RelayCommand<News>((x) =>
            {
                navigation.NavigateTo(ViewModelLocator.ViewNewsPage, x);
            }));
        public void AskClose()
        {
            notification.ConfirmDialog("Conferma chiusura", "Sei sicuro di voler chiudere l'applicazione?", (confirmValue) =>
            {
                if (confirmValue)
                    closeApp.CloseApp();
            });
        }
        private RelayCommand _loadMoreNewsCmd;
        public RelayCommand CaricaAltreNewsCommand =>
            _loadMoreNewsCmd ??
            (_loadMoreNewsCmd = new RelayCommand(() =>
            {
                GetNewsFrom();
            }));
    }
}
