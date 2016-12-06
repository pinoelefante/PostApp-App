using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PostApp.Api;
using PostApp.Api.Data;
using PostApp.Services;
using System;
using System.Collections.Generic;
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
        public PostaNewsEditorPageViewModel(IPostAppApiService api, INavigationService nav, LocationService _loc)
        {
            postApp = api;
            navigation = nav;
            location = _loc;
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
                var pos = await location.GetLocation();
                if(pos!=null)
                {
                    PosizioneNews = $"{pos.Latitude.ToString("N6")};{pos.Longitude.ToString("N6")}";
                }
                else
                {
                    //TODO show error
                }
            }));
        public RelayCommand InviaPostCommand =>
            _inviaNewsCmd ??
            (_inviaNewsCmd = new RelayCommand(async () =>
            {
                if (VerificaCampi())
                {
                    var res = await postApp.PostEditor(ListaEditor[EditorSelezionato].id, TitoloNews, CorpoNews, Immagine, PosizioneNews);
                    if(res.response == StatusCodes.OK)
                    {

                        navigation.NavigateTo(ViewModelLocator.MainPage);
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }));
        private bool VerificaCampi()
        {
            return true;
        }
    }
}
