using PostApp.Api.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Api
{
    public interface IPostAppApiService
    {
        //generici
        ObservableCollection<Comune> GetListaComuni();
        Dictionary<string, string> GetListaCategorie();
        void SetAccessCode(string code);

        //authentication.php
        Task<Envelop<string>> RequestAccessCode();
        Task<Envelop<string>> Access(string accessCode);
        Task<Envelop<string>> RegisterAccessCode(string accessCode, string loc, string email = null);

        //editor.php
        Task<Envelop<string>> RegistraEditor(string nome, string categoria, string email, string telefono, string indirizzo, string localita);
        Task<Envelop<List<Editor>>> WriterEditors();
        Task<Envelop<List<Editor>>> WriterEditorsDaApprovare();
        Task<Envelop<List<Editor>>> ReaderEditors();
        Task<Envelop<string>> AddDescrizioneEditor(int idEditor, string descrizione);
        Task<Envelop<string>> AddEditorImage(int idEditor, string immagineBase64);
        Task<Envelop<string>> PostEditor(int idEditor, string titolo, string corpo, string img, string posizione);
        Task<Envelop<string>> FollowEditor(int idEditor);
        Task<Envelop<string>> UnfollowEditor(int idEditor);
        Task<Envelop<List<News>>> GetNotificationsEditor(DateTime from);
        Task<Envelop<string>> ThanksForNewsEditor(int idNews);
        Task<Envelop<News>> LeggiNewsEditor(int idNews);
        Task<Envelop<List<News>>> GetNewsEditor(int idEditor);
        Task<Envelop<List<Editor>>> GetEditorsByLocation(string location);
        Task<Envelop<List<Comune>>> GetComuniConEditors();
        Task<Envelop<List<News>>> GetAllMyNewsFrom(int? fromId);

        //scuola.php
        Task<Envelop<string>> RegistraScuola(string nomeScuola, string localitaScuola, string emailScuola, string telScuola, string indirizzoScuola, string cognomePreside, string nomePreside, string usernamePreside, string passwordPreside);
        Task<Envelop<List<Scuola>>> GetMieScuoleWriter();
        Task<Envelop<List<Scuola>>> GetMieScuoleWriterDaApprovare();
        Task<Envelop<List<Scuola>>> GetMieScuoleReader();
        Task<Envelop<string>> VerificaAccessoScuola(int idScuola);
        Task<Envelop<string>> AccessoScuola(string username, string password);

    }
}
