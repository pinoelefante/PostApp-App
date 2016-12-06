using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PostApp.Api;
using PostApp.Api.Data;
using PostApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.ViewModels
{
    public class PostaNewsEditorPageViewModel : MyViewModel
    {
        private IPostAppApiService postApp;
        private INavigationService navigation;
        private LocationService location;
        private UserNotificationService notification;
        public PostaNewsEditorPageViewModel(IPostAppApiService api, INavigationService nav, LocationService _loc, UserNotificationService _not)
        {
            postApp = api;
            navigation = nav;
            location = _loc;
            notification = _not;
        }
        public override void NavigatedTo(object parameter = null)
        {
            CaricaWriters();   
        }
        private async void CaricaWriters()
        {

            var response = await postApp.WriterEditors();
            if (response.response == StatusCodes.OK)
            {
                ListaEditor = response.content;
                RaisePropertyChanged(() => ListaEditor);
                if (ListaEditor.Count == 1)
                    EditorSelezionato = 0;
            }
        }
        private string _titolo = string.Empty, _corpo = string.Empty, _posizione = string.Empty, _immagine = string.Empty;
        private int _editorSelezionato = -1;
        public List<Editor> ListaEditor { get; set; }
        public int EditorSelezionato { get { return _editorSelezionato; } set { Set(ref _editorSelezionato, value); } }
        public string TitoloNews { get { return _titolo; } set { Set(ref _titolo, value); } }
        public string CorpoNews { get { return _corpo; } set { Set(ref _corpo, value); } }
        public string PosizioneNews { get { return _posizione; } set { Set(ref _posizione, value); } }
        public string Immagine { get { return _immagine; } set { Set(ref _immagine, value); } }
        private RelayCommand _inviaNewsCmd, _locationCmd;
        public RelayCommand LocationCommand =>
            _locationCmd ??
            (_locationCmd = new RelayCommand(async () =>
            {
                if (location.IsLocationEnabled)
                {
                    var pos = await location.GetLocation();
                    if (pos != null)
                        PosizioneNews = $"{pos.Latitude.ToString("N6")};{pos.Longitude.ToString("N6")}";
                    else
                        notification.ShowMessageDialog("Posizione", "Errore nella rilevazione della posizione");
                }
                else
                {
                    notification.ShowMessageDialog("Posizione", "Abilitare la rilevazione della posizione");
                }
            }));
        public RelayCommand InviaPostCommand =>
            _inviaNewsCmd ??
            (_inviaNewsCmd = new RelayCommand(async () =>
            {
                if (VerificaCampi(true))
                {
                    var res = await postApp.PostEditor(ListaEditor[EditorSelezionato].id, TitoloNews, CorpoNews, Immagine, PosizioneNews.Trim());
                    if(res.response == StatusCodes.OK)
                    {
                        notification.ShowMessageDialog("Invio notizia", "Notizia inviata con successo");
                        navigation.NavigateTo(ViewModelLocator.MainPage);
                    }
                    else
                        notification.ShowMessageDialog("Invio notizia", $"Errore durante l'invio della notizia.\nStatusCode: {res.response}");
                }
            }));
        private bool VerificaCampi(bool notify = false)
        {
            if (EditorSelezionato < 0)
            {
                if (notify)
                    notification.ShowMessageDialog("Verifica dati", "Seleziona l'editor con cui pubblicare la notizia");
                return false;
            }
            if (string.IsNullOrEmpty(TitoloNews) || string.IsNullOrEmpty(TitoloNews.Trim()))
            {
                if (notify)
                    notification.ShowMessageDialog("Verifica dati", "Inserire un titolo");
                return false;
            }
            if (string.IsNullOrEmpty(CorpoNews) || string.IsNullOrEmpty(CorpoNews.Trim()))
            {
                if (notify)
                    notification.ShowMessageDialog("Verifica dati", "Il messaggio della notizia non può essere vuoto");
                return false;
            }
            return true;
        }
    }
}
