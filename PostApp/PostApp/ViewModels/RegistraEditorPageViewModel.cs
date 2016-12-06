using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using PostApp.Api;
using PostApp.Api.Data;
using PostApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace PostApp.ViewModels
{
    public class RegistraEditorPageViewModel : MyViewModel
    {
        private IPostAppApiService postApp;
        private INavigationService navigation;
        private UserNotificationService toast;
        private ValidationService validation;
        public Dictionary<string, string> Categorie { get; set; }
        public RegistraEditorPageViewModel(IPostAppApiService api, INavigationService nav, UserNotificationService _t, ValidationService _validation)
        {
            postApp = api;
            navigation = nav;
            toast = _t;
            validation = _validation;
            Categorie = postApp.GetListaCategorie();
            ElencoCitta = postApp.GetListaComuni();
        }
        private bool _regButtonEnable, _nomeEnabled = true;
        private int _categoriaIndex = -1;
        private Comune _cittaSelezionata;
        private string _nome = string.Empty, _indirizzo = string.Empty, _email = string.Empty, _telefono = string.Empty;
        public ObservableCollection<Comune> ElencoCitta { get; private set; }
        public int CategoriaIndexSelected
        {
            get { return _categoriaIndex; }
            set
            {
                Set(ref _categoriaIndex, value);
                if (_categoriaIndex >=0 && Categorie[Categorie.Keys.ElementAt(_categoriaIndex)].CompareTo("Comune") == 0)
                {
                    IsNomeEnabled = false;
                    ComponiNomeComune();
                }
                else
                    IsNomeEnabled = true;
            }
        }
        public Comune CittaSelezionata { get { return _cittaSelezionata; } set { Set(ref _cittaSelezionata, value); } }
        public string NomeEditor { get { return _nome; } set { Set(ref _nome, value); } }
        public string IndirizzoEditor { get { return _indirizzo; } set { Set(ref _indirizzo, value); } }
        public string IndirizzoEmail { get { return _email; } set { Set(ref _email, value); } }
        public string Telefono { get { return _telefono; } set { Set(ref _telefono, value); } }
        public bool IsRegisterButtonEnable { get { return _regButtonEnable; } set { Set(ref _regButtonEnable, value); } }
        public bool IsNomeEnabled { get { return _nomeEnabled; } set { Set(ref _nomeEnabled, value); } }

        private RelayCommand<Comune> _cellSelectedCommand;
        public RelayCommand<Comune> CellSelectedCommand =>
            _cellSelectedCommand ??
            (_cellSelectedCommand = new RelayCommand<Comune>(
                parameter =>
                {
                    CittaSelezionata = parameter;
                    if (_categoriaIndex >= 0 && Categorie[Categorie.Keys.ElementAt(_categoriaIndex)].CompareTo("Comune") == 0)
                    {
                        IsNomeEnabled = false;
                        ComponiNomeComune();
                    }
                }
            ));
        private RelayCommand _regCmd;
        public RelayCommand InviaRegistrazione =>
            _regCmd ??
            (_regCmd = new RelayCommand(async () =>
            {
                if (VerificaDati(true))
                {
                    var envelop = await postApp.RegistraEditor(NomeEditor, Categorie[Categorie.Keys.ElementAt(_categoriaIndex)], IndirizzoEmail, Telefono, IndirizzoEditor, CittaSelezionata.istat);
                    if(envelop.response == StatusCodes.OK)
                    {
                        toast.ShowMessageDialog("Registrazione editor", "La registrazione è avvenuta con successo ed è in attesa di approvazione");
                        navigation.NavigateTo(ViewModelLocator.MainPage);
                    }
                    else
                    {
                        toast.ShowMessageDialog("Registrazione editor", $"Errore {envelop.response}\nSi è verificato un errore durante la registrazione dell'editor. Riprova più tardi o contatta l'assistenza");
                        Debug.WriteLine($"Errore {envelop.response}");
                    }
                }
            }));
        private bool VerificaDati(bool notifyOn = false)
        {
            if (this.CategoriaIndexSelected < 0)
            {   if(notifyOn)
                    toast.ShowMessageDialog("Registrazione editor", "Devi selezionare una categoria");
                IsRegisterButtonEnable = false;
                return false;
            }
            if (this.CittaSelezionata == null)
            {
                if (notifyOn)
                    toast.ShowMessageDialog("Registrazione editor", "Devi selezionare la città dell'editor");
                IsRegisterButtonEnable = false;
                return false;
            }
            if (string.IsNullOrEmpty(this.IndirizzoEditor.Trim()))
            {
                if (notifyOn)
                    toast.ShowMessageDialog("Registrazione editor", "Devi inserire l'indirizzo dell'editor");
                IsRegisterButtonEnable = false;
                return true;
            }
            if (!validation.EmailValidation(IndirizzoEmail))
            {
                if (notifyOn)
                    toast.ShowMessageDialog("Registrazione editor", "Devi inserire un indirizzo email valido");
                IsRegisterButtonEnable = false;
                return false;
            }
            if (string.IsNullOrEmpty(Telefono.Trim()))
            {
                if (notifyOn)
                    toast.ShowMessageDialog("Registrazione editor", "Devi inserire un numero di telefono");
                IsRegisterButtonEnable = false;
                return false;
            }
            return true;
        }
        private void ComponiNomeComune()
        {
            NomeEditor = $"Comune di {(CittaSelezionata!=null ? CittaSelezionata.comune : "")}";
        }
    }
}
