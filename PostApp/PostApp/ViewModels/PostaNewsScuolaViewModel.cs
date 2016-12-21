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
    public class PostaNewsScuolaViewModel : MyViewModel
    {
        private IPostAppApiService api;
        public PostaNewsScuolaViewModel(IPostAppApiService _p)
        {
            api = _p;
        }
        public override void NavigatedTo(object parameter = null)
        {
            Destinatari.Clear();
            CaricaElencoScuole();
        }
        ObservableCollection<Scuola> ElencoScuole { get; } = new ObservableCollection<Scuola>();
        private async void CaricaElencoScuole()
        {
            IsBusyActive = true;
            ElencoScuole?.Clear();
            var response = await api.GetMieScuoleWriter();
            if(response.response == StatusCodes.OK)
            {
                foreach (var item in response.content)
                    ElencoScuole.Add(item);

                if (!ElencoScuole.Any())
                {
                    //L'utente non ha scuole per cui pubblicare
                    //TODO mostrare un dialog e tornare alla home
                }
            }
            else
            {
                //TODO mostra dialogo per ricaricare
            }
            IsBusyActive = false;
        }
        private bool _postScuola, _postClasse, _dstAll, _dstGenitori, _dstStudenti, _dstDocenti, _dstAta, _dstPreside;
        public bool IsPostScuola
        {
            get { return _postScuola; }
            set
            {
                Set(ref _postScuola, value);
                if (_postScuola == true)
                    IsPostClasse = false;
            }
        }
        public bool IsPostClasse
        {
            get { return _postClasse; }
            set
            {
                Set(ref _postClasse, value);
                if (IsPostClasse)
                    IsPostScuola = false;
            }
        }
        public bool DestinatariAll
        {
            get { return _dstAll; }
            set
            {
                Set(ref _dstAll, value);
                DestinatariGenitori = value;
                DestinatariStudenti = value;
                DestinatariDocenti = value;
                DestinatariAta = value;
                DestinatariPreside = value;
            }
        }
        public bool DestinatariGenitori
        {
            get { return _dstGenitori; }
            set
            {
                Set(ref _dstGenitori, value);
                if (value)
                {
                    if (Destinatari.Contains(api.DestinatarioScuolaGenitore))
                        Destinatari.Add(api.DestinatarioScuolaGenitore);
                }
                else
                    Destinatari.Remove(api.DestinatarioScuolaGenitore);
            }
        }
        public bool DestinatariStudenti
        {
            get { return _dstStudenti; }
            set
            {
                Set(ref _dstStudenti, value);
                if (value)
                {
                    if (Destinatari.Contains(api.DestinatarioScuolaStudente))
                        Destinatari.Add(api.DestinatarioScuolaStudente);
                }
                else
                    Destinatari.Remove(api.DestinatarioScuolaStudente);
            }
        }
        public bool DestinatariDocenti
        {
            get { return _dstDocenti; }
            set
            {
                Set(ref _dstDocenti, value);
                if (value)
                {
                    if (Destinatari.Contains(api.DestinatarioScuolaDocente))
                        Destinatari.Add(api.DestinatarioScuolaDocente);
                }
                else
                    Destinatari.Remove(api.DestinatarioScuolaDocente);
            }
        }
        public bool DestinatariAta
        {
            get { return _dstAta; }
            set
            {
                Set(ref _dstAta, value);
                if (value)
                {
                    if (Destinatari.Contains(api.DestinatarioScuolaAta))
                        Destinatari.Add(api.DestinatarioScuolaAta);
                }
                else
                    Destinatari.Remove(api.DestinatarioScuolaAta);
            }
        }
        public bool DestinatariPreside
        {
            get { return _dstPreside; }
            set
            {
                Set(ref _dstPreside, value);
                if (value)
                {
                    if (Destinatari.Contains(api.DestinatarioScuolaPreside))
                        Destinatari.Add(api.DestinatarioScuolaPreside);
                }
                else
                    Destinatari.Remove(api.DestinatarioScuolaPreside);
            }
        }
        private List<string> Destinatari { get; } = new List<string>();
        
    }
}
