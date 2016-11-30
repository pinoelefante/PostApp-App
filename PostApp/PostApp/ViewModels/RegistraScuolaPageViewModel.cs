using GalaSoft.MvvmLight.Views;
using PostApp.Api;
using PostApp.Api.Data;
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
        
        public RegistraScuolaPageViewModel(IPostAppApiService _postApp, INavigationService navigationService)
        {
            navigation = navigationService;
            postApp = _postApp;
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
                if (IsRegisterButtonEnable)
                {
                    IsBusyActive = true;
                    var envelop = await postApp.RegistraScuola(NomeScuola, CittaSelezionata.istat, IndirizzoEmail, Telefono, IndirizzoScuola, CognomeDirigente, NomeDirigente, UsernameDirigente, PasswordDirigente);
                    IsBusyActive = false;
                    if(envelop.response == StatusCodes.OK)
                    {
                        //TODO comunicare il successo dell'operazione
                        navigation.NavigateTo(ViewModelLocator.MyMasterDetailPage);
                    }
                    else
                    {
                        Debug.WriteLine($"Errore {envelop.response}");
                    }
                }
            }));
        private bool _canRegister;
        public bool IsRegisterButtonEnable { get { return _canRegister; } set { Set(ref _canRegister, value); } }
        private void VerificaDati()
        {
            if (string.IsNullOrEmpty(NomeDirigente.Trim()))
            {
                IsRegisterButtonEnable = false;
                return;
            }
            if (string.IsNullOrEmpty(CognomeDirigente.Trim()))
            {
                IsRegisterButtonEnable = false;
                return;
            }
            if (string.IsNullOrEmpty(NomeScuola.Trim()))
            {
                IsRegisterButtonEnable = false;
                return;
            }
            if (CittaSelezionata == null)
            {
                IsRegisterButtonEnable = false;
                return;
            }
            if (string.IsNullOrEmpty(IndirizzoEmail.Trim()))
            {
                IsRegisterButtonEnable = false;
                return;
            }
            if (string.IsNullOrEmpty(Telefono.Trim()))
            {
                IsRegisterButtonEnable = false;
                return;
            }
            if (string.IsNullOrEmpty(UsernameDirigente.Trim()))
            {
                IsRegisterButtonEnable = false;
                return;
            }
            if (string.IsNullOrEmpty(PasswordDirigente.Trim()))
            {
                IsRegisterButtonEnable = false;
                return;
            }
            IsRegisterButtonEnable = true;
        }
    }
}
