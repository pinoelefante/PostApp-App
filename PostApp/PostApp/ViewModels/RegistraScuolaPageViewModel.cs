using GalaSoft.MvvmLight.Views;
using PostApp.Api;
using PostApp.Api.Data;
using PostApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PostApp.ViewModels
{
    public class RegistraScuolaPageViewModel : MyViewModel
    {
        private INavigationService navigation;
        private IPostAppApiService postApp;
        private UserNotificationService toast;
        private ValidationService validation;
        
        public RegistraScuolaPageViewModel(IPostAppApiService _postApp, INavigationService navigationService, UserNotificationService _toast, ValidationService _val)
        {
            navigation = navigationService;
            postApp = _postApp;
            toast = _toast;
            validation = _val;

            ElencoCitta = postApp.GetListaComuni();
        }
        public override async void NavigatedTo(object parameter = null)
        {
            ElencoScuoleDaApprovare?.Clear();
            var envelop = await postApp.GetMieScuoleWriterDaApprovare();
            if(envelop.response == StatusCodes.OK)
                foreach (var item in envelop.content)
                    ElencoScuoleDaApprovare.Add(item);
        }

        private string _cognomeDirigente = string.Empty, _nomeDirigente = string.Empty, _nomeScuola = string.Empty, _indirizzoEmail = string.Empty, _indirizzoScuola = string.Empty, _telefono = string.Empty, _usernameDirigente = string.Empty, _passwordDirigente = string.Empty;
        private Comune _cittaSelezionata;
        private ObservableCollection<Comune> _elencoCitta;
        private ObservableCollection<Scuola> _scuoleApprovare = new ObservableCollection<Scuola>();

        public ObservableCollection<Scuola> ElencoScuoleDaApprovare { get { return _scuoleApprovare; } set { Set(ref _scuoleApprovare, value); } }
        public ObservableCollection<Comune> ElencoCitta { get { return _elencoCitta; } set { Set(ref _elencoCitta, value); } }
        public string CognomeDirigente { get { return _cognomeDirigente; } set { Set(ref _cognomeDirigente, value); VerificaDati(); } }
        public string NomeDirigente { get { return _nomeDirigente; } set { Set(ref _nomeDirigente, value); VerificaDati(); } }
        public string NomeScuola { get { return _nomeScuola; } set { Set(ref _nomeScuola, value); VerificaDati(); } }
        public string IndirizzoScuola { get { return _indirizzoScuola; } set { Set(ref _indirizzoScuola, value); VerificaDati(); } }
        public string IndirizzoEmail { get { return _indirizzoEmail; } set { Set(ref _indirizzoEmail, value); VerificaDati(); } }
        public string Telefono { get { return _telefono; } set { Set(ref _telefono, value); VerificaDati(); } }
        public string UsernameDirigente { get { return _usernameDirigente; } set { Set(ref _usernameDirigente, value); VerificaDati(); } }
        public string PasswordDirigente { get { return _passwordDirigente; } set { Set(ref _passwordDirigente, value); VerificaDati(); } }
        public Comune CittaSelezionata { get { return _cittaSelezionata; } set { Set(ref _cittaSelezionata, value); VerificaDati(); } }
        private Command<Comune> _cellSelectedCommand;
        public Command<Comune> CellSelectedCommand => 
            _cellSelectedCommand ?? 
            (_cellSelectedCommand = new Command<Comune>(
                parameter => 
                {
                    CittaSelezionata = parameter;
                }
            ));
        private Command _inviaReg;
        public Command InviaRegistrazione =>
            _inviaReg ??
            (_inviaReg = new Command(async () =>
            {
                Debug.WriteLine($"{CognomeDirigente} {NomeDirigente}\n{NomeScuola}\n{CittaSelezionata?.comune}\n{IndirizzoEmail}\n{Telefono}\n{UsernameDirigente}\n{PasswordDirigente}");
                VerificaDati();
                if (VerificaDati(true))
                {
                    IsBusyActive = true;
                    var envelop = await postApp.RegistraScuola(NomeScuola, CittaSelezionata.istat, IndirizzoEmail, Telefono, IndirizzoScuola, CognomeDirigente, NomeDirigente, UsernameDirigente, PasswordDirigente);
                    IsBusyActive = false;
                    if(envelop.response == StatusCodes.OK)
                    {
                        toast.ShowMessageDialog("Registrazione scuola", "La registrazione è avvenuta con successo ed è in attesa di approvazione");
                        navigation.NavigateTo(ViewModelLocator.MainPage);
                    }
                    else
                    {
                        toast.ShowMessageDialog("Registrazione scuola", $"Errore {envelop.response}\nSi è verificato un errore durante la registrazione della scuola.\nRiprova più tardi o contatta l'assistenza");
                        Debug.WriteLine($"Errore {envelop.response}");
                    }
                }
            }));
        private bool _canRegister;
        public bool IsRegisterButtonEnable { get { return _canRegister; } set { Set(ref _canRegister, value); } }
        private bool VerificaDati(bool notify = false)
        {
            if (string.IsNullOrEmpty(NomeDirigente.Trim()))
            {
                if(notify)
                    toast.ShowMessageDialog("Registrazione scuola", "Inserisci il nome del dirigente scolastico");
                IsRegisterButtonEnable = false;
                return false;
            }
            if (string.IsNullOrEmpty(CognomeDirigente.Trim()))
            {
                if (notify)
                    toast.ShowMessageDialog("Registrazione scuola", "Inserisci il cognome del dirigente scolastico");
                IsRegisterButtonEnable = false;
                return false;
            }
            if (string.IsNullOrEmpty(UsernameDirigente.Trim()))
            {
                if (notify)
                    toast.ShowMessageDialog("Registrazione scuola", "Inserisci la username del dirigente scolastico");
                IsRegisterButtonEnable = false;
                return false;
            }
            if (string.IsNullOrEmpty(PasswordDirigente.Trim()))
            {
                if (notify)
                    toast.ShowMessageDialog("Registrazione scuola", "Inserisci la password del dirigente scolastico");
                IsRegisterButtonEnable = false;
                return false;
            }
            if (string.IsNullOrEmpty(NomeScuola.Trim()))
            {
                if (notify)
                    toast.ShowMessageDialog("Registrazione scuola", "Inserisci il nome della scuola");
                IsRegisterButtonEnable = false;
                return false;
            }
            if (CittaSelezionata == null)
            {
                if (notify)
                    toast.ShowMessageDialog("Registrazione scuola", "Inserisci la città della scuola");
                IsRegisterButtonEnable = false;
                return false;
            }
            if (string.IsNullOrEmpty(IndirizzoEmail.Trim()))
            {
                if (notify)
                    toast.ShowMessageDialog("Registrazione scuola", "Inserisci l'indirizzo email della scuola");
                IsRegisterButtonEnable = false;
                return false;
            }
            if (string.IsNullOrEmpty(Telefono.Trim()))
            {
                if (notify)
                    toast.ShowMessageDialog("Registrazione scuola", "Inserisci il numero di telefono della scuola");
                IsRegisterButtonEnable = false;
                return false;
            }
            IsRegisterButtonEnable = true;
            return true;
        }
    }
}
