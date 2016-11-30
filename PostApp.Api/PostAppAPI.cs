using Newtonsoft.Json;
using PostApp.Api.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Api
{
    public class PostAppAPI : IPostAppApiService
    {
        private HttpClient http;
        private readonly static string SERVER_ADDRESS = "http://gestioneserietv.altervista.org/postAppTest";
        
        public PostAppAPI()
        {
            //inject DataService
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true, AllowAutoRedirect = false };
            http = new HttpClient(handler);
            http.BaseAddress = new Uri(SERVER_ADDRESS);
            http.DefaultRequestHeaders.Add("User-Agent", "PostAppClient");
        }
        #region authentication.php
        public async Task<Envelop<string>> RequestAccessCode()
        {
            Envelop<string> response = await sendRequest<string>($"{SERVER_ADDRESS}/authentication.php?action=RequestAccessCode",null, false);
            return response;
        }
        public async Task<Envelop<string>> Access(string accessCode)
        {
            FormUrlEncodedContent postContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("code",accessCode)
            });
            Envelop<string> response = await sendRequest<string>($"{SERVER_ADDRESS}/authentication.php?action=Access", postContent, false);
            return response;
        }
        public async Task<Envelop<string>> RegisterAccessCode(string accessCode, string loc, string email = null)
        {
            FormUrlEncodedContent postContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("code",accessCode),
                new KeyValuePair<string, string>("loc",loc),
                new KeyValuePair<string, string>("mail",email)
            });
            Envelop<string> response = await sendRequest<string>($"{SERVER_ADDRESS}/authentication.php?action=RegisterAccessCode", postContent, false);
            return response;
        }
        #endregion

        #region editor.php
        public async Task<Envelop<string>> RegistraEditor(string nome, string categoria, string email, string telefono, string indirizzo, string localita)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("nome",nome),
                new KeyValuePair<string, string>("categoria",categoria),
                new KeyValuePair<string, string>("email",email),
                new KeyValuePair<string, string>("tel",telefono),
                new KeyValuePair<string, string>("indirizzo",indirizzo),
                new KeyValuePair<string, string>("localita",localita)
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/editor.php?action=RegistraEditor", content);
        }
        public async Task<Envelop<List<Editor>>> WriterEditors()
        {
            return await sendRequest<List<Editor>>($"{SERVER_ADDRESS}/editor.php?action=WriterEditors");
        }
        public async Task<Envelop<List<Editor>>> WriterEditorsDaApprovare()
        {
            return await sendRequest<List<Editor>>($"{SERVER_ADDRESS}/editor.php?action=WriterEditorsDaApprovare");
        }
        public async Task<Envelop<List<Editor>>> ReaderEditors()
        {
            return await sendRequest<List<Editor>>($"{SERVER_ADDRESS}/editor.php?action=ReaderEditors");
        }
        public async Task<Envelop<string>> AddDescrizioneEditor(int idEditor, string descrizione)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("descrizione",descrizione),
                new KeyValuePair<string, string>("idEditor",idEditor.ToString())
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/editor.php?action=AddDescrizione", content);
        }
        public async Task<Envelop<string>> AddEditorImage(int idEditor, string immagineBase64)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("immagine",immagineBase64), //TODO verificare correttezza
                new KeyValuePair<string, string>("idEditor",idEditor.ToString())
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/editor.php?action=AddEditorImage", content);
        }
        public async Task<Envelop<string>> PostEditor(int idEditor, string titolo, string corpo, string img, string posizione)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("editor",idEditor.ToString()),
                new KeyValuePair<string, string>("titolo",titolo),
                new KeyValuePair<string, string>("corpo",corpo),
                new KeyValuePair<string, string>("img",img),
                new KeyValuePair<string, string>("posizione",posizione),
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/editor.php?action=Post", content);
        }
        public async Task<Envelop<string>> FollowEditor(int idEditor)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idEditor",idEditor.ToString())
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/editor.php?action=FollowEditor", content);
        }
        public async Task<Envelop<string>> UnfollowEditor(int idEditor)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idEditor",idEditor.ToString())
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/editor.php?action=UnfollowEditor", content);
        }
        public async Task<Envelop<List<News>>> GetNotificationsEditor(DateTime from)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("from",from.ToString("yyyy-MM-dd HH:mm:ss"))
            });
            return await sendRequest<List<News>>($"{SERVER_ADDRESS}/editor.php?action=GetNotifications", content);
        }
        public async Task<Envelop<List<News>>> ThanksForNewsEditor(int idNews)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idNews",idNews.ToString())
            });
            return await sendRequest<List<News>>($"{SERVER_ADDRESS}/editor.php?action=ThanksForNews", content);
        }
        public async Task<Envelop<News>> LeggiNewsEditor(int idNews)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idNews",idNews.ToString())
            });
            return await sendRequest<News>($"{SERVER_ADDRESS}/editor.php?action=LeggiNews", content);
        }
        public async Task<Envelop<List<News>>> GetNewsEditor(int idEditor)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idEditor",idEditor.ToString())
            });
            return await sendRequest<List<News>>($"{SERVER_ADDRESS}/editor.php?action=GetNewsEditor", content);
        }
        public async Task<Envelop<List<Editor>>> GetEditorsByLocation(string location)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("localita",location)
            });
            return await sendRequest<List<Editor>>($"{SERVER_ADDRESS}/editor.php?action=GetEditorsByLocation", content);
        }
        public async Task<Envelop<List<Comune>>> GetComuniConEditors()
        {
            return await sendRequest<List<Comune>>($"{SERVER_ADDRESS}/editor.php?action=GetComuniConEditors");
        }
        public async Task<Envelop<List<News>>> GetAllMyNewsFrom(int? fromId)
        {
            FormUrlEncodedContent content = null;
            if (fromId!=null)
                content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("lastId",fromId.ToString())
                });
            return await sendRequest<List<News>>($"{SERVER_ADDRESS}/editor.php?action=GetAllMyNewsFrom", content);
        }
        #endregion

        #region scuola.php
        public async Task<Envelop<string>> RegistraScuola(string nomeScuola, string localitaScuola, string emailScuola, string telScuola, string indirizzoScuola, string cognomePreside, string nomePreside, string usernamePreside, string passwordPreside)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("nomeScuola",nomeScuola),
                new KeyValuePair<string, string>("localitaScuola",localitaScuola),
                new KeyValuePair<string, string>("emailScuola",emailScuola),
                new KeyValuePair<string, string>("telefonoScuola",telScuola),
                new KeyValuePair<string, string>("indirizzoScuola",indirizzoScuola),
                new KeyValuePair<string, string>("nomePreside",nomePreside),
                new KeyValuePair<string, string>("cognomePreside",cognomePreside),
                new KeyValuePair<string, string>("usernamePreside",usernamePreside),
                new KeyValuePair<string, string>("passwordPreside",passwordPreside)
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/scuola.php?action=RegistraScuola", content);
        }
        public async Task<Envelop<List<Scuola>>> GetMieScuoleWriter()
        {
            return await sendRequest<List<Scuola>>($"{SERVER_ADDRESS}/scuola.php?action=GetMieScuoleWriter", null);
        }
        public async Task<Envelop<List<Scuola>>> GetMieScuoleWriterDaApprovare()
        {
            return await sendRequestWithAction<List<Scuola>, List<Dictionary<string,string>>>($"{SERVER_ADDRESS}/scuola.php?action=GetMieScuoleWriterDaApprovare",(x)=> 
            {
                if (x!=null && x.Any())
                {
                    List<Scuola> scuole = new List<Scuola>(x.Count);
                    foreach (var item in x)
                    {
                        Scuola s = new Scuola()
                        {
                            id = Int32.Parse(item["scuolaId"]),
                            nome = item["scuolaNome"],
                            ruolo = item["userRuolo"]
                        };
                        scuole.Add(s);
                    }
                    return scuole;
                }
                return new List<Scuola>(1);
            }, null, true);
        }
        public async Task<Envelop<List<Scuola>>> GetMieScuoleReader()
        {
            return await sendRequest<List<Scuola>>($"{SERVER_ADDRESS}/scuola.php?action=GetMieScuoleReader", null);
        }
        public async Task<Envelop<string>> VerificaAccessoScuola(int idScuola)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idScuola",idScuola.ToString())
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/scuola.php?action=VerificaAccesso", content);
        }
        public async Task<Envelop<string>> AccessoScuola(string username, string password)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username",username),
                new KeyValuePair<string, string>("password",password)
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/scuola.php?action=AccessoScuola", content);
        }
        #endregion


        private async Task<Envelop<T>> sendRequest<T>(string url, HttpContent postContent = null, bool loginRequired = true, Func<Dictionary<string,string>, T> action = null)
        {
            if(loginRequired && !IsLogged)
            {
                var response = await Access(AccessCode);
                if(response.response == StatusCodes.OK)
                {
                    Debug.WriteLine("Login ok");
                    IsLogged = true;
                }
                else
                {
                    Debug.WriteLine("Errore accesso");
                    
                }
            }
            Envelop<T> envelop = new Envelop<T>();
            try
            {
                var response = await http.PostAsync(url, postContent);
                Debug.WriteLine($"REQUEST at {url} - {response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var output = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(output);
                    
                    envelop.time = DateTime.Parse(result["time"], CultureInfo.InvariantCulture);
                    envelop.response = (StatusCodes)Enum.ToObject(typeof(StatusCodes), Int32.Parse(result["response"]));
                    if (action == null)
                    {
                        if (typeof(T) == typeof(string))
                            envelop.content = (T)(object)result["content"];
                        else
                            envelop.content = JsonConvert.DeserializeObject<T>(result["content"]);
                    }
                    else
                    {
                        var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(result["content"]);
                        envelop.content = action.Invoke(values);
                    }
                    return envelop;
                }
                else
                {
                    envelop.time = DateTime.Now;
                    envelop.response = StatusCodes.ERRORE_SERVER;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine($"ERRORE - {e.Message}");
                envelop.time = DateTime.Now;
                envelop.response = StatusCodes.ERRORE_CONNESSIONE;
            }
            return envelop;
        }
        private async Task<Envelop<ContentType>> sendRequestWithAction<ContentType, ContentContainer>(string url, Func<ContentContainer, ContentType> parseAction, HttpContent postContent = null, bool loginRequired = true)
        {
            if (loginRequired && !IsLogged)
            {
                var response = await Access(AccessCode);
                if (response.response == StatusCodes.OK)
                {
                    Debug.WriteLine("Login ok");
                    IsLogged = true;
                }
                else
                {
                    Debug.WriteLine("Errore accesso");
                }
            }
            Envelop<ContentType> envelop = new Envelop<ContentType>();
            try
            {
                var response = await http.PostAsync(url, postContent);
                Debug.WriteLine($"REQUEST at {url} - {response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var output = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(output);

                    envelop.time = DateTime.Parse(result["time"].ToString(), CultureInfo.InvariantCulture);
                    envelop.response = (StatusCodes)Enum.ToObject(typeof(StatusCodes), Int32.Parse(result["response"].ToString()));
                    if (parseAction == null)
                    {
                        if (typeof(ContentType) == typeof(string))
                            envelop.content = (ContentType)(object)result["content"];
                        else
                            envelop.content = JsonConvert.DeserializeObject<ContentType>(result["content"].ToString());
                    }
                    else
                    {
                        var values = JsonConvert.DeserializeObject<ContentContainer>(result["content"].ToString());
                        envelop.content = parseAction.Invoke(values);
                    }
                    return envelop;
                }
                else
                {
                    envelop.time = DateTime.Now;
                    envelop.response = StatusCodes.ERRORE_SERVER;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ERRORE - {e.Message}");
                envelop.time = DateTime.Now;
                envelop.response = StatusCodes.ERRORE_CONNESSIONE;
            }
            return envelop;
        }
        private ObservableCollection<Comune> ComuniList;
        public ObservableCollection<Comune> GetListaComuni()
        {
            if (ComuniList== null || ComuniList.Count == 0)
            {
                var assembly = typeof(PostAppAPI).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("PostApp.Api.comuni.json");
                string text = string.Empty;
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
                var comuni = JsonConvert.DeserializeObject<List<Comune>>(text);
                ComuniList = new ObservableCollection<Comune>(comuni);
            }
            return ComuniList;
        }
        private Dictionary<string, string> ListaCategorie;
        public Dictionary<string, string> GetListaCategorie()
        {
            if(ListaCategorie==null || ListaCategorie.Count == 0)
            {
                ListaCategorie = new Dictionary<string, string>();
                ListaCategorie.Add("Comune", "Comune");
                ListaCategorie.Add("Pro Loco", "ProLoco");
                //ListaCategorie.Add("Comune", "Comune");
                //ListaCategorie.Add("Comune", "Comune");
            }
            return ListaCategorie;
        }
        private string AccessCode;
        private bool IsLogged = false;
        public void SetAccessCode(string code)
        {
            AccessCode = code;
        }
    }
}
