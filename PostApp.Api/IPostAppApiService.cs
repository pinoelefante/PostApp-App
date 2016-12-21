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
        string DestinatarioScuolaGenitore { get; }
        string DestinatarioScuolaStudente { get; }
        string DestinatarioScuolaPreside { get; }
        string DestinatarioScuolaAta { get; }
        string DestinatarioScuolaDocente { get; }
        //generici
        ObservableCollection<Comune> GetListaComuni();
        Dictionary<string, string> GetListaCategorie();
        Dictionary<string, string> GetListaGradiScuola();
        Dictionary<string, string> GetElencoRuoliScuola();
        Action OnAccessCodeError { get; set; }
        string AccessCode { get; set; }

        //authentication.php
        Task<Envelop<string>> RequestAccessCode();
        Task<Envelop<string>> RegisterAccessCode(string accessCode, string loc, string email = null);
        Task<Envelop<string>> Access(string accessCode);
        Task<Envelop<string>> CambiaLocalita(string localita);
        //"Ripristina utente"
        Task<Envelop<string>> RegistraPush(string token, PushDevice device, string deviceId);
        Task<Envelop<string>> UnRegistraPush(string token, PushDevice device, string deviceId);

        //editor.php
        Task<Envelop<string>> RegistraEditor(string nome, string categoria, string email, string telefono, string indirizzo, string localita);
        Task<Envelop<List<Editor>>> WriterEditors();
        Task<Envelop<List<Editor>>> WriterEditorsDaApprovare();
        Task<Envelop<List<Editor>>> ReaderEditors();
        Task<Envelop<string>> AddDescrizioneEditor(int idEditor, string descrizione);
        Task<Envelop<string>> AddEditorImage(int idEditor, byte[] immagineBytes);
        Task<Envelop<string>> PostEditor(int idEditor, string titolo, string corpo, byte[] img, string posizione);
        Task<Envelop<string>> FollowEditor(int idEditor);
        Task<Envelop<string>> UnfollowEditor(int idEditor);
        Task<Envelop<List<News>>> GetNotificationsEditor(DateTime from);
        Task<Envelop<string>> ThanksForNewsEditor(int idNews);
        Task<Envelop<News>> LeggiNewsEditor(int idNews);
        Task<Envelop<List<News>>> GetNewsEditor(int idEditor, int? lastId);
        Task<Envelop<List<News>>> GetNewsFromAllEditors(int idFrom);
        Task<Envelop<List<Editor>>> GetEditorsByLocation(string location);
        Task<Envelop<List<Comune>>> GetComuniConEditors();
        Task<Envelop<List<News>>> GetAllMyNewsFrom(int? fromId);
        Task<Envelop<List<News>>> GetAllMyNewsTo(int to);
        Task<Envelop<List<News>>> GetEditorNewsFromTo(int idEditor, int from, int to);
        Task<Envelop<Editor>> GetEditorInfo(int idEditor);
        Task<Envelop<List<Editor>>> CercaEditor(string query);

        //scuola.php
        Task<Envelop<string>> RegistraScuola(string nomeScuola, string localitaScuola, string emailScuola, string telScuola, string indirizzoScuola, string cognomePreside, string nomePreside, string usernamePreside, string passwordPreside);
        Task<Envelop<List<Scuola>>> GetMieScuoleWriter();
        Task<Envelop<List<Scuola>>> GetMieScuoleWriterDaApprovare();
        Task<Envelop<List<Scuola>>> GetMieScuoleReader();
        Task<Envelop<string>> VerificaAccessoScuola(int idScuola);
        Task<Envelop<string>> AccessoScuola(string username, string password);
        Task<Envelop<string>> AggiungiPlesso(int idScuola,string nomePlesso);
        Task<Envelop<string>> RimuoviPlesso(int idScuola,int idPlesso);
        Task<Envelop<string>> AggiungiSezione(int idScuola, int idPlesso, int idGrado, int classeInizio, int classeFine, string letteraSezione);
        Task<Envelop<string>> RimuoviSezione(int idScuola, int idPlesso, int idGrado, string letteraSezione);
        Task<Envelop<string>> AggiungiGrado(int idScuola, string gradoNome);
        Task<Envelop<string>> RimuoviGrado(int idScuola, int idGrado);
        Task<Envelop<string>> AggiungiClasse(int idScuola, int idPlesso, int idGrado, int classe, string letteraSezione);
        Task<Envelop<string>> RimuoviClasse(int idScuola, int idPlesso, int idGrado, int classe, string letteraSezione);
        Task<Envelop<string>> SbloccaCodiceFamigliaScuola(string codice, string nome, string cognome, string data);
        Task<Envelop<string>> PostaNewsScuola(int idScuola, string titolo, string corpoNews, byte[] immagine, IEnumerable<string> destinatati);
        //"PostaNewsClasse"
        //"GetNewsScuola"
        //"GetNewsClassi"
        Task<Envelop<List<News>>> GetNewsMyScuole();
        Task<Envelop<List<News>>> GetNewsMyClassi();
        //"GetNotificheScuola"
        //"GetNotificheClassi"
        Task<Envelop<string>> ThankYouNewsScuola(int idNews);
        Task<Envelop<string>> ThankYouNewsClasse(int idNews);
        Task<Envelop<News>> LeggiNewsScuola(int idNews);
        Task<Envelop<string>> NotificaLetturaScuola(int idNews);
        Task<Envelop<News>> LeggiNewsClasse(int idNews);
        Task<Envelop<string>> NotificaLetturaClasse(int idNews);
    }
}
