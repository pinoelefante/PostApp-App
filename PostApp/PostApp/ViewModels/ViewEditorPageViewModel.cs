using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PostApp.Api;
using PostApp.Api.Data;
using PostApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.ViewModels
{
    public class ViewEditorPageViewModel : MyViewModel
    {
        private IPostAppApiService postApp;
        private UserNotificationService notification;
        private INavigationService navigation;
        public ViewEditorPageViewModel(IPostAppApiService _post, UserNotificationService _not, INavigationService _nav)
        {
            postApp = _post;
            notification = _not;
            navigation = _nav;
        }
        private int _currentEditorId;
        public int CurrentEditorId
        {
            get { return _currentEditorId; }
            set
            {
                var lastId = _currentEditorId;
                Set(ref _currentEditorId, value);
                if (value != lastId)
                {
                    ElencoNews?.Clear();
                    lastNewsId = null;
                    LoadEditor();
                }
            }
        }
        private Editor _editor;
        public Editor Editor { get { return _editor; } set { Set(ref _editor, value); } }
        private async void LoadEditor()
        {
            int retry = 0;
            IsBusyActive = true;
            bool responseOk = false;
            do
            {
                var envelop = await postApp.GetEditorInfo(CurrentEditorId);
                if (envelop.response == StatusCodes.OK)
                {
                    responseOk = true;
                    Editor = envelop.content;
                }
            }
            while (!responseOk && retry < 3);
            IsBusyActive = false;

            if (responseOk == false)
                notification.ShowMessageDialog("Errore di connessione", "Non è stato possibile caricare le informazioni dell'editor.\nControlla la connessione o contatta l'assistenza");
            else
                LoadNews();
        }
        private RelayCommand _followCmd, _unfollowCmd,_loadMoreNewsCmd;
        public RelayCommand FollowCommand =>
            _followCmd ??
            (_followCmd = new RelayCommand(async () =>
            {
                if (!Editor.following)
                {
                    var envelop = await postApp.FollowEditor(CurrentEditorId);
                    if (envelop.response == StatusCodes.OK)
                    {
                        Editor.followers++;
                        Editor.following = true;
                        App.Locator.MainPageViewModel.AggiungiEditor(CurrentEditorId);
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() => RaisePropertyChanged(() => Editor));
                    }
                }
            }));
        public RelayCommand UnfollowCommand =>
            _unfollowCmd ??
            (_unfollowCmd = new RelayCommand(async () =>
            {
                if (Editor.following)
                {
                    var envelop = await postApp.UnfollowEditor(CurrentEditorId);
                    if (envelop.response == StatusCodes.OK)
                    {
                        Editor.followers--;
                        Editor.following = false;
                        App.Locator.MainPageViewModel.RimuoviEditor(CurrentEditorId);
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() => RaisePropertyChanged(() => Editor));
                    }
                }
            }));
        public RelayCommand LoadMoreNewsCommand =>
            _loadMoreNewsCmd ??
            (_loadMoreNewsCmd = new RelayCommand(() =>
            {
                LoadNews();
            }));
        public ObservableCollection<News> ElencoNews { get; } = new ObservableCollection<News>();
        private int? lastNewsId;
        private async void LoadNews()
        {
            IsBusyActive = true;
            var envelop = await postApp.GetNewsEditor(Editor.id, lastNewsId);
            if(envelop.response == StatusCodes.OK)
            {
                if (envelop.content.Any())
                    lastNewsId = envelop.content.Last().id;
                foreach (var item in envelop.content)
                    ElencoNews.Add(item);
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
    }
}
