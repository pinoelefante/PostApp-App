using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Plugin.Share;
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
    public class ViewNewsPageViewModel : MyViewModel
    {
        private IPostAppApiService postApp;
        private LocationService location;
        private INavigationService navigation;
        public ViewNewsPageViewModel(IPostAppApiService _api, LocationService _loc, INavigationService _nav)
        {
            postApp = _api;
            location = _loc;
            navigation = _nav;
        }
        public override void NavigatedTo(object parameter = null)
        {
            if(parameter != null && parameter is News)
            {
                NewsSelezionata = parameter as News;
                LoadNews();
            }
            else
            {
                Debug.WriteLine("Non so perché tu sia qui");
            }
        }
        private News _newsSelezionata;
        public News NewsSelezionata { get { return _newsSelezionata; } set { Set(ref _newsSelezionata, value); } }
        private async void LoadNews()
        {
            var envelop = await postApp.LeggiNewsEditor(NewsSelezionata.id);
            NewsSelezionata.letta = 1;
            if (envelop.response == StatusCodes.OK)
            {
                NewsSelezionata.thankyou = envelop.content.thankyou;
                NewsSelezionata.testo = envelop.content.testo;
                NewsSelezionata.posizione = envelop.content.posizione;
                NewsSelezionata.letta = 1;
            }
            else
            {

            }
        }
        private RelayCommand _thankYouCmd, _shareCmd, _positionCmd;
        public RelayCommand ThankYouCommand =>
            _thankYouCmd ??
            (_thankYouCmd = new RelayCommand(async () =>
            {
                Envelop<string> envelop = null;
                if (NewsSelezionata.tipoNews == NewsType.EDITOR_NEWS)
                    envelop = await postApp.ThanksForNewsEditor(NewsSelezionata.id);
                else if(NewsSelezionata.tipoNews == NewsType.SCUOLA_NEWS)
                {

                }
                else if (NewsSelezionata.tipoNews == NewsType.SCUOLA_CLASSE_NEWS)
                {

                }
                if(envelop != null && envelop.response==StatusCodes.OK)
                    NewsSelezionata.thankyou++;
                else
                {
                    if(envelop?.response == StatusCodes.NEWS_GIA_RINGRAZIATO)
                    {

                    }
                    else
                    {
                        Debug.WriteLine("Errore: "+envelop.response);
                        //errore di connessione
                    }
                }
            }));
        public RelayCommand Share =>
            _shareCmd ??
            (_shareCmd = new RelayCommand(async () =>
            {
                await CrossShare.Current.ShareLink("https://www.facebook.com", NewsSelezionata.testoAnteprima, NewsSelezionata.titolo);
            }));
        public RelayCommand LocationCommand =>
            _positionCmd ??
            (_positionCmd = new RelayCommand(async () =>
            {
                var pos = NewsSelezionata.posizione.Split(new char[] { ';' });
                var latitude = Double.Parse(pos[0]);
                var longitude = Double.Parse(pos[1]);
                var res = await location.NavigateTo(latitude, longitude, NewsSelezionata.titolo);
                if (res)
                {
                    Debug.WriteLine("Maps OK");
                }
                else
                {
                    Debug.WriteLine("Maps error");
                }
            }));
        public void ApriPaginaEditor()
        {
            navigation.NavigateTo(ViewModelLocator.ViewEditorPage, NewsSelezionata.publisherId);
        }
    }
}
