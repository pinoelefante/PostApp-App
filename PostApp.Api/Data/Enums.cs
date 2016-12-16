using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Api.Data
{
    public enum NewsType
    {
        EDITOR_NEWS = 0,
        SCUOLA_NEWS = 1,
        SCUOLA_CLASSE_NEWS = 2
    }
    public enum EditorType
    {
        GENERICO_EDITOR = 0,
        SCUOLA_EDITOR = 1,
        SCUOLA_CLASSE_EDITOR = 2
    }
    public enum StatusCodes
    {
        ERRORE_SERVER = -500, //errore presente solo sul client
        ERRORE_CONNESSIONE = -501, //errore presente solo sul client
        
        ENVELOP_UNSET = 0,

        FAIL = -1,
        RICHIESTA_MALFORMATA = -2,
        METODO_ASSENTE = -3,
        SQL_FAIL = -4,

        OK = 1,

        LOGIN_ERROR = 10,
        LOGIN_GIA_LOGGATO = 11,
        LOGIN_NON_LOGGATO = 12,

        REG_CODICE_IN_USO = 20, //registrazione utente ma codice già in uso

        EDITOR_IMPOSSIBILE_ASSEGNARE_AMMINISTRATORE = 50,
        EDITOR_ERRORE_CREAZIONE = 51,
        EDITOR_UTENTE_NON_AUTORIZZATO = 52,
        EDITOR_SEGUI_GIA = 53,
        NEWS_GIA_RINGRAZIATO = 54,
        EDITOR_NEWS_NON_TROVATA = 55,
        NEWS_GIA_LETTA = 56,
        EDITOR_NON_SEGUITO = 57,

        SCUOLA_IMPOSSIBILE_ASSEGNARE_PRESIDE = 60,
        SCUOLA_USERNAME_NON_VALIDO = 61,
        SCUOLA_PASSWORD_ERRATA = 62,
        SCUOLA_PERMESSI_INSUFFICIENTI = 63,
        SCUOLA_PLESSO_DUPLICATO = 64,
        SCUOLA_PLESSO_NON_PRESENTE = 65,
        SCUOLA_GRADO_DUPLICATO = 66,
        SCUOLA_ERRORE_INSERIMENTO_SEZIONE = 67,

        NEWS_COMMON_TIPO_NEWS_INVALIDO = 80,
    }
    public enum PushDevice
    {
        NOT_SET = 0,
        ANDROID = 1,
        IOS = 2,
        WINDOWS_UWP = 3
    }
}
