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
    public class CittaPageViewModel : MyViewModel
    {
        private IPostAppApiService postApp;
        INavigationService navigation;
        UserNotificationService notification;
        public CittaPageViewModel(IPostAppApiService _p, INavigationService _n, UserNotificationService _not)
        {
            postApp = _p;
            navigation = _n;
            notification = _not;
        }
        public Dictionary<Comune, ObservableCollection<Editor>> Editors { get; } = new Dictionary<Comune, ObservableCollection<Editor>>();
        public override void NavigatedTo(object parameter = null)
        {
            if (!Editors.Keys.Any())
                CaricaComuni();
            else
                RaisePropertyChanged(() => Editors);
        }
        private async void CaricaComuni()
        {
            IsBusyActive = true;
            var envelop = await postApp.GetComuniConEditors();
            if(envelop.response == StatusCodes.OK)
            {
                foreach (var item in envelop.content)
                    Editors.Add(item, new ObservableCollection<Editor>());
                RaisePropertyChanged(() => Editors);
            }
            else
            {
                notification.ShowMessageDialog("Errore", "Si è verificato un errore durante il caricamento dei comuni");
            }
            IsBusyActive = false;
        }
        public async void CaricaEditors(string istat, Action atEnd = null)
        {
            var keyFound = Editors.Keys.Where(x => x.istat.Equals(istat));
            if (!keyFound.Any())
            {
                atEnd?.Invoke();
                return;
            }
            ObservableCollection<Editor> editors = Editors[keyFound.ElementAt(0)];
            if (editors.Any())
            {
                atEnd?.Invoke();
                return;
            }
            IsBusyActive = true;
            var envelop = await postApp.GetEditorsByLocation(istat);
            if (envelop.response == StatusCodes.OK)
            {
                var comuni = envelop.content.Where(x => x.categoria.Equals("Comune")).ToList();
                if (comuni.Any()) //inserisce i comuni all'inizio della lista
                {
                    foreach (var item in comuni)
                    {
                        editors.Add(item);
                        envelop.content.Remove(item);
                    }
                }
                foreach (var item in envelop.content)
                    editors.Add(item);
            }
            else
            {
                notification.ShowMessageDialog("Errore", "Si è verificato un errore durante il caricamento degli editors del comune");
            }
            IsBusyActive = false;
            atEnd?.Invoke();
        }
        public void ApriEditor(Editor editor)
        {
            navigation.NavigateTo(ViewModelLocator.ViewEditorPage, editor.id);
        }
    }
}
