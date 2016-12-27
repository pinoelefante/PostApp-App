using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Plugin.FilePicker;
using PostApp.Api;
using PostApp.Api.Data;
using PostApp.Controls;
using PostApp.Services;
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
        private UserNotificationService notification;
        private INavigationService navigation;
        public PostaNewsScuolaViewModel(IPostAppApiService _p, UserNotificationService notif, INavigationService nav)
        {
            api = _p;
            notification = notif;
            navigation = nav;
        }
        public override void NavigatedTo(object parameter = null)
        {
            Destinatari.Clear();
            CaricaElencoScuole();
        }
        public override void NavigatedFrom()
        {
            DestinatariAll = false;
            TitoloNews = string.Empty;
            CorpoNews = string.Empty;
            if (ElencoScuole.Any())
                ScuolaSelezionata = -1;
            ImmagineNews = string.Empty;
            _immagineByteArray = null;
            ClassiDisponibili?.Clear();
            ClassiSelezionate?.Clear();
        }
        public ObservableCollection<Scuola> ElencoScuole { get; } = new ObservableCollection<Scuola>();
        private async void CaricaElencoScuole()
        {
            IsBusyActive = true;
            ElencoScuole?.Clear();
            var response = await api.GetMieScuoleWriter();
            if(response.response == StatusCodes.OK)
            {
                foreach (var item in response.content)
                    ElencoScuole.Add(item);
                RaisePropertyChanged(() => ElencoScuole);
                if (!ElencoScuole.Any())
                {
                    notification.ShowMessageDialog("Scuole disponibili", "Non sei autorizzato a postare per nessuna scuola.\nSe questo è un errore, riprova più tardi o contatta l'assistenza.", () => navigation.NavigateTo(ViewModelLocator.MainPage));
                    return;
                }
                else if (ElencoScuole.Count == 1)
                    ScuolaSelezionata = -1;
            }
            else
            {
                notification.ConfirmDialog("Scuole disponibili", $"Si è verificato un errore di comunicazione.\nErrore: {response.response}", (action) =>
                {
                    if (action)
                        CaricaElencoScuole();
                    else
                        navigation.NavigateTo(ViewModelLocator.MainPage);
                }, "Riprova", "Chiudi");
            }
            IsBusyActive = false;
        }
        private bool _dstAll, _dstGenitori, _dstStudenti, _dstDocenti, _dstAta, _dstPreside;
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
                    if (!Destinatari.Contains(api.DestinatarioScuolaGenitore))
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
                    if (!Destinatari.Contains(api.DestinatarioScuolaStudente))
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
                    if (!Destinatari.Contains(api.DestinatarioScuolaDocente))
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
                    if (!Destinatari.Contains(api.DestinatarioScuolaAta))
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
                    if (!Destinatari.Contains(api.DestinatarioScuolaPreside))
                        Destinatari.Add(api.DestinatarioScuolaPreside);
                }
                else
                    Destinatari.Remove(api.DestinatarioScuolaPreside);
            }
        }
        private List<string> Destinatari { get; } = new List<string>();
        private string _titolo = string.Empty, _corpo = string.Empty, _immagine = string.Empty, _sezione = string.Empty;
        private byte[] _immagineByteArray;
        private int _scuolaSelezionata = -1;
        public int ScuolaSelezionata
        {
            get { return _scuolaSelezionata; }
            set
            {
                Set(ref _scuolaSelezionata, value);
                if(value >= 0)
                    CaricaClassi();
            }
        }
        public string TitoloNews { get { return _titolo; } set { Set(ref _titolo, value); } }
        public string CorpoNews { get { return _corpo; } set { Set(ref _corpo, value); } }
        public string ImmagineNews { get { return _immagine; } set { Set(ref _immagine, value); } }
        private RelayCommand _immagineCmd, _postaNewsScuolaCmd, _postaNewsClasseCmd;
        public RelayCommand ImmagineCommand =>
            _immagineCmd ??
            (_immagineCmd = new RelayCommand(async () =>
            {
                var file = await CrossFilePicker.Current.PickFile();
                if (file != null)
                {
                    ImmagineNews = file.FileName;
                    _immagineByteArray = file.DataArray;
                }
            }));
        public RelayCommand PostaNewsScuola =>
            _postaNewsScuolaCmd ??
            (_postaNewsScuolaCmd = new RelayCommand(async () =>
            {
                if (VerificaCampiScuola(true))
                {
                    var res = await api.PostaNewsScuola(ElencoScuole[ScuolaSelezionata].id, TitoloNews.Trim(), CorpoNews.Trim(), _immagineByteArray, Destinatari);
                    if (res.response == StatusCodes.OK)
                    {
                        notification.ShowMessageDialog("Invio notizia", "Notizia inviata con successo");
                        navigation.NavigateTo(ViewModelLocator.MainPage);
                    }
                    else
                        notification.ShowMessageDialog("Invio notizia", $"Errore durante l'invio della notizia.\nStatusCode: {res.response}");
                }
            }));
        public RelayCommand PostaNewsClasse =>
            _postaNewsClasseCmd ??
            (_postaNewsClasseCmd = new RelayCommand(async () =>
            {
                if (VerificaCampiClasse(true))
                {
                    DestinatariAta = false;
                    DestinatariDocenti = false;
                    DestinatariPreside = false;
                    var res = await api.PostaNewsClasse(ElencoScuole[ScuolaSelezionata].id, TitoloNews.Trim(), CorpoNews.Trim(), _immagineByteArray, Destinatari, ClassiSelezionate.Select(x => x.Id));
                    if (res.response == StatusCodes.OK)
                    {
                        notification.ShowMessageDialog("Invio notizia", "Notizia inviata con successo");
                        navigation.NavigateTo(ViewModelLocator.MainPage);
                    }
                    else
                        notification.ShowMessageDialog("Invio notizia", $"Errore durante l'invio della notizia.\nStatusCode: {res.response}");
                }
            }));
        private bool VerificaCampiScuola(bool notify = false)
        {
            if (ScuolaSelezionata < 0)
            {
                if (notify)
                    notification.ShowMessageDialog("Verifica dati", "Seleziona la scuola per cui pubblicare la notizia");
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
            if(Destinatari==null || !Destinatari.Any())
            {
                if (notify)
                    notification.ShowMessageDialog("Verifica dati", "Devi selezionare i destinatari della notizia");
                return false;
            }
            return true;
        }
        private bool VerificaCampiClasse(bool notify = false)
        {
            if (VerificaCampiScuola(true))
            {
                var found = ClassiDisponibili.Where(x => x.IsSelected).Select(x => x.Item).ToList();
                if(found == null || !found.Any())
                {
                    if (notify)
                        notification.ShowMessageDialog("Verifica dati", "Devi selezionare almeno una classe");
                    return false;
                }
                ClassiSelezionate = found;
                return true;
            }
            return false;
        }
        public ObservableCollection<SelectableItemWrapper<Classe>> ClassiDisponibili { get; } = new ObservableCollection<SelectableItemWrapper<Classe>>();
        private List<Classe> ClassiSelezionate { get; set; }
        private RelayCommand<SelectableItemWrapper<Classe>> _classeSelectCmd;
        public RelayCommand<SelectableItemWrapper<Classe>> ClasseTapped =>
            _classeSelectCmd ??
            (_classeSelectCmd = new RelayCommand<SelectableItemWrapper<Classe>>((item) =>
            {
                item.IsSelected = !item.IsSelected;
            }));
        private bool loadingClassi = false;
        private async void CaricaClassi()
        {
            if (loadingClassi)
                return;
            loadingClassi = true;
            if (ScuolaSelezionata < 0 || ScuolaSelezionata >= ElencoScuole.Count)
                return;

            ClassiDisponibili?.Clear();
            int prove = 0;
            bool ok = false;
            do
            {
                var envelop = await api.ElencoClassiScuola(ElencoScuole[ScuolaSelezionata].id);
                if(envelop.response == StatusCodes.OK)
                {
                    ok = true;
                    foreach (var classe in envelop.content)
                        ClassiDisponibili.Add
                        (
                            new SelectableItemWrapper<Classe>()
                            {
                                IsSelected = false,
                                Item = classe
                            }
                        );
                    break;
                }
                prove++;
            } while (prove < 3);
            if (ok && !ClassiDisponibili.Any())
                notification.ShowMessageDialog("Classi della scuola", "Non sono presenti classi per cui postare news");
            if (!ok)
                notification.ShowMessageDialog("Errore", "Si è verificato un errore durante il caricamento delle classi.\nRiprova più tardi");
            loadingClassi = false;
        }
    }
}
