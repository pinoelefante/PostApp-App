using GalaSoft.MvvmLight.Command;
using Plugin.Share;
using PostApp.Api;
using PostApp.Api.Data;
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
        public ViewNewsPageViewModel(IPostAppApiService _api)
        {
            postApp = _api;
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
                    if(envelop?.response == StatusCodes.EDITOR_NEWS_GIA_RINGRAZIATO)
                    {

                    }
                    else
                    {
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
    }
}
