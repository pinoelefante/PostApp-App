using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PostApp.Api;
using PostApp.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.ViewModels
{
    public class CercaEditorPageViewModel : MyViewModel
    {
        private IPostAppApiService postApp;
        private INavigationService navigation;
        public CercaEditorPageViewModel(IPostAppApiService _p, INavigationService nav)
        {
            postApp = _p;
            navigation = nav;
        }
        private string _testoCercato = string.Empty;
        public string SearchText { get { return _testoCercato; } set { Set(ref _testoCercato, value); } }
        private RelayCommand _cercaCmd;
        public RelayCommand CercaCommand =>
            _cercaCmd ??
            (_cercaCmd = new RelayCommand(async () =>
            {
                RisultatiRicerca?.Clear();
                if (!string.IsNullOrEmpty(SearchText.Trim()))
                {
                    IsBusyActive = true;
                    var envelop = await postApp.CercaEditor(SearchText.Trim());
                    if(envelop.response == StatusCodes.OK)
                        RisultatiRicerca = envelop.content;
                    IsBusyActive = false;
                }
            }));
        private List<Editor> _found;
        public List<Editor> RisultatiRicerca { get { return _found; } set { Set(ref _found, value); } }
        private RelayCommand<Editor> _apriEditorCmd;
        public RelayCommand<Editor> ApriEditor =>
            _apriEditorCmd ??
            (_apriEditorCmd = new RelayCommand<Editor>((x) =>
            {
                navigation.NavigateTo(ViewModelLocator.ViewEditorPage, x.id);
            }));
    }
}
