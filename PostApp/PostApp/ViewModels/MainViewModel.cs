using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using PostApp.Api;
using PostApp.Api.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.ViewModels
{
    public class MainViewModel : MyViewModel
    {
        private IPostAppApiService postApp;
        private INavigationService navigation;
        public MainViewModel(IPostAppApiService _api, INavigationService _nav)
        {
            postApp = _api;
            navigation = _nav;
        }
        public ObservableCollection<News> ElencoNews { get; } = new ObservableCollection<News>();
        private int? editorLastId = null;

        public override async void NavigatedTo(object parameter = null)
        {
            if (!ElencoNews.Any())
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
        }
    }
}
