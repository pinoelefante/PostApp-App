﻿using Newtonsoft.Json;
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
        public async Task<Envelop<string>> RegistraPush(string token, PushDevice device)
        {
            FormUrlEncodedContent postContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("token",token),
                new KeyValuePair<string, string>("deviceOS", ((int)device).ToString())
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/authentication.php?action=RegistraPush", postContent);
        }
        public Task<Envelop<string>> CambiaLocalita(string localita)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> UnRegistraPush(string token, PushDevice device)
        {
            throw new NotImplementedException();
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
            return await sendRequestWithAction<List<Editor>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/editor.php?action=WriterEditors", (x) =>
            {
                if(x!=null && x.Any())
                {
                    List<Editor> editors = new List<Editor>(x.Count);
                    foreach (var item in x)
                    {
                        Editor ed = new Editor()
                        {
                            id = Int32.Parse(item["id"]),
                            nome = item["nome"],
                            ruolo = item["ruolo"]
                        };
                        editors.Add(ed);
                    }
                    return editors;
                }
                return new List<Editor>(1);
            });
        }
        public async Task<Envelop<List<Editor>>> WriterEditorsDaApprovare()
        {
            return await sendRequestWithAction<List<Editor>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/editor.php?action=WriterEditorsDaApprovare", (x) =>
            {
                if (x != null && x.Any())
                {
                    List<Editor> editors = new List<Editor>(x.Count);
                    foreach (var item in x)
                    {
                        Editor ed = new Editor()
                        {
                            id = Int32.Parse(item["id"]),
                            nome = item["nome"],
                            ruolo = item["ruolo"]
                        };
                        editors.Add(ed);
                    }
                    return editors;
                }
                return new List<Editor>(1);
            });
        }
        public async Task<Envelop<List<Editor>>> ReaderEditors()
        {
            return await sendRequestWithAction<List<Editor>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/editor.php?action=ReaderEditors", (x) =>
            {
                if (x != null && x.Any())
                {
                    List<Editor> editors = new List<Editor>(x.Count);
                    foreach (var item in x)
                    {
                        Editor ed = new Editor()
                        {
                            id = Int32.Parse(item["id"]),
                            nome = item["nome"],
                            immagine = item["immagine"]
                        };
                        editors.Add(ed);
                    }
                    return editors;
                }
                return new List<Editor>(1);
            });
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
            return await sendRequestWithAction<List<News>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/editor.php?action=GetNotifications", (x) =>
            {
                if(x!=null && x.Any())
                {
                    List<News> news = new List<News>(x.Count);
                    foreach (var item in x)
                    {
                        News n = new News()
                        {
                            publisherId = Int32.Parse(item["editorId"]),
                            publisherNome = item["editorNome"],
                            id = Int32.Parse(item["newsId"]),
                            titolo = item["titolo"],
                            immagine = item["immagine"],
                            posizione = item["posizione"],
                            data = DateTime.Parse(item["data"], CultureInfo.InvariantCulture),
                            tipoNews = NewsType.EDITOR_NEWS
                            //testo = item["corpo"]
                    };
                        news.Add(n);
                    }
                    return news;
                }
                return new List<News>(1);
            }, content);
        }
        public async Task<Envelop<string>> ThanksForNewsEditor(int idNews)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idNews",idNews.ToString())
            });
            return await sendRequest<string>($"{SERVER_ADDRESS}/editor.php?action=ThanksForNews", content);
        }
        public async Task<Envelop<News>> LeggiNewsEditor(int idNews)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idNews",idNews.ToString())
            });
            return await sendRequestWithAction<News, Dictionary<string, string>>($"{SERVER_ADDRESS}/editor.php?action=LeggiNews", (x) =>
            {
                if(x!=null && x.Any())
                {
                    News n = new News()
                    {
                        publisherId = Int32.Parse(x["editorId"]),
                        publisherNome = x["editorNome"],
                        id = Int32.Parse(x["newsId"]),
                        titolo = x["titolo"],
                        immagine = x["immagine"],
                        testo = x["corpo"],
                        data = DateTime.Parse(x["data"], CultureInfo.InvariantCulture),
                        posizione = x["posizione"],
                        thankyou = Int32.Parse(x["thankyou"]),
                        letta = 1,
                        tipoNews = NewsType.EDITOR_NEWS
                    };
                    return n;
                }
                return null;
            }, content);
        }
        public async Task<Envelop<List<News>>> GetNewsEditor(int idEditor, int? lastId)
        {
            var parameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idEditor",idEditor.ToString())
            };
            if (lastId != null)
                parameters.Add(new KeyValuePair<string, string>("from", lastId.ToString()));
            FormUrlEncodedContent content = new FormUrlEncodedContent(parameters);
            return await sendRequestWithAction<List<News>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/editor.php?action=GetNewsEditor", (x)=>
            {
                if(x!=null && x.Any())
                {
                    List<News> news = new List<News>(x.Count);
                    foreach (var item in x)
                    {
                        News n = new News()
                        {
                            publisherId = Int32.Parse(item["editorId"]),
                            publisherNome = item["editorNome"],
                            id = Int32.Parse(item["newsId"]),
                            titolo = item["titolo"],
                            immagine = item["immagine"],
                            testo = item["corpo"],
                            data = DateTime.Parse(item["data"], CultureInfo.InvariantCulture),
                            posizione = item["posizione"],
                            tipoNews = NewsType.EDITOR_NEWS
                        };
                        news.Add(n);
                    }
                    return news;
                }
                return new List<News>(1);
            }, content);
        }
        public async Task<Envelop<List<Editor>>> GetEditorsByLocation(string location)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("localita",location)
            });
            return await sendRequestWithAction<List<Editor>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/editor.php?action=GetEditorsByLocation", (x) =>
            {
                if(x!=null & x.Any())
                {
                    List<Editor> editors = new List<Editor>();
                    foreach (var item in x)
                    {
                        Editor e = new Editor()
                        {
                            id = Int32.Parse(item["editorId"]),
                            nome = item["editorNome"],
                            categoria = item["editorCategoria"],
                            immagine = item["immagine"]
                        };
                        editors.Add(e);
                    }
                    return editors;
                }
                return new List<Editor>(1);
            }, content);
        }
        public async Task<Envelop<List<Comune>>> GetComuniConEditors()
        {
            return await sendRequestWithAction<List<Comune>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/editor.php?action=GetComuniConEditors", (x) =>
            {
                if(x!=null && x.Any())
                {
                    List<Comune> comuni = new List<Comune>(x.Count);
                    foreach (var item in x)
                    {
                        Comune comune = new Comune()
                        {
                            comune = item["comune"],
                            istat = item["id"]
                        };
                        comuni.Add(comune);
                    }
                    return comuni;
                }
                return new List<Comune>(1);
            });
        }
        public async Task<Envelop<List<News>>> GetAllMyNewsFrom(int? fromId)
        {
            FormUrlEncodedContent content = null;
            if (fromId!=null)
                content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("lastId",fromId.ToString())
                });
            return await sendRequestWithAction<List<News>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/editor.php?action=GetAllMyNewsFrom", (x) =>
            {
                if(x!=null && x.Any())
                {
                    List<News> news = new List<News>(x.Count);
                    foreach (var item in x)
                    {
                        News n = new News()
                        {
                            publisherId = Int32.Parse(item["editorId"]),
                            publisherNome = item["editorNome"],
                            id = Int32.Parse(item["newsId"]),
                            titolo = item["titolo"],
                            immagine = item["immagine"],
                            testo = item["corpo"],
                            data = DateTime.Parse(item["data"], CultureInfo.InvariantCulture),
                            posizione = item["posizione"],
                            tipoNews = NewsType.EDITOR_NEWS,
                            letta = Int32.Parse(item["letta"])
                        };
                        news.Add(n);
                    }
                    return news;
                }
                return new List<News>(1);
            }, content);
        }
        public async Task<Envelop<List<News>>> GetAllMyNewsTo(int id)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("to",id.ToString())
            });
            return await sendRequestWithAction<List<News>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/editor.php?action=GetAllMyNewsTo", (x) =>
            {
                if (x != null && x.Any())
                {
                    List<News> news = new List<News>(x.Count);
                    foreach (var item in x)
                    {
                        News n = new News()
                        {
                            publisherId = Int32.Parse(item["editorId"]),
                            publisherNome = item["editorNome"],
                            id = Int32.Parse(item["newsId"]),
                            titolo = item["titolo"],
                            immagine = item["immagine"],
                            testo = item["corpo"],
                            data = DateTime.Parse(item["data"], CultureInfo.InvariantCulture),
                            posizione = item["posizione"],
                            tipoNews = NewsType.EDITOR_NEWS,
                            letta = Int32.Parse(item["letta"])
                        };
                        news.Add(n);
                    }
                    return news;
                }
                return new List<News>(1);
            }, content);
        }
        public async Task<Envelop<List<News>>> GetEditorNewsFromTo(int idEditor,int from, int to)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idEditor",idEditor.ToString()),
                new KeyValuePair<string, string>("from",from.ToString()),
                new KeyValuePair<string, string>("to",to.ToString())
            });
            return await sendRequestWithAction<List<News>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/editor.php?action=GetEditorNewsFromTo", (x) =>
            {
                if (x != null && x.Any())
                {
                    List<News> news = new List<News>(x.Count);
                    foreach (var item in x)
                    {
                        News n = new News()
                        {
                            publisherId = Int32.Parse(item["editorId"]),
                            publisherNome = item["editorNome"],
                            id = Int32.Parse(item["newsId"]),
                            titolo = item["titolo"],
                            immagine = item["immagine"],
                            testo = item["corpo"],
                            data = DateTime.Parse(item["data"], CultureInfo.InvariantCulture),
                            posizione = item["posizione"],
                            tipoNews = NewsType.EDITOR_NEWS,
                            letta = Int32.Parse(item["letta"])
                        };
                        news.Add(n);
                    }
                    return news;
                }
                return new List<News>(1);
            }, content);
        }
        public async Task<Envelop<Editor>> GetEditorInfo(int idEditor)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("idEditor",idEditor.ToString())
            });
            return await sendRequestWithAction<Editor, Dictionary<string, string>>($"{SERVER_ADDRESS}/editor.php?action=GetEditorInfo", (x) =>
            {
                if (x != null && x.Any())
                {
                    Editor editor = new Editor()
                    {
                        descrizione = x["descrizione"],
                        followers = Int32.Parse(x["followers"]),
                        following = Int32.Parse(x["following"]) == 0 ? false : true,
                        geo_coordinate = x["coordinate"],
                        id = Int32.Parse(x["id"]),
                        immagine = x["immagine"],
                        localita = x["localita"],
                        nome = x["nome"]
                    };
                    return editor;
                }
                return null;
            }, content);
        }
        public async Task<Envelop<List<Editor>>> CercaEditor(string query)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("query",query)
            });
            return await sendRequestWithAction<List<Editor>, List<Dictionary<string,string>>>($"{SERVER_ADDRESS}/editor.php?action=CercaEditor", (x)=> 
            {
                if(x!=null && x.Any())
                {
                    List<Editor> editors = new List<Editor>(x.Count);
                    foreach (var item in x)
                    {
                        Editor e = new Editor()
                        {
                            id = Int32.Parse(item["id"]),
                            nome = item["nome"]
                        };
                        editors.Add(e);
                    }
                    return editors;
                }
                return new List<Editor>(1);
            },content);
        }
        public Task<Envelop<List<News>>> GetNewsFromAllEditors(int idFrom)
        {
            throw new NotImplementedException();
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
            return await sendRequestWithAction<List<Scuola>, List<Dictionary<string, string>>>($"{SERVER_ADDRESS}/scuola.php?action=GetMieScuoleWriter", (x) =>
            {
                if (x != null && x.Any())
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
        public Task<Envelop<string>> AggiungiPlesso(int idScuola, string nomePlesso)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> RimuoviPlesso(int idScuola, int idPlesso)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> AggiungiSezione(int idScuola, int idPlesso, int idGrado, int classeInizio, int classeFine, string letteraSezione)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> RimuoviSezione(int idScuola, int idPlesso, int idGrado, string letteraSezione)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> AggiungiGrado(int idScuola, string gradoNome)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> RimuoviGrado(int idScuola, int idGrado)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> AggiungiClasse(int idScuola, int idPlesso, int idGrado, int classe, string letteraSezione)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> RimuoviClasse(int idScuola, int idPlesso, int idGrado, int classe, string letteraSezione)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> SbloccaCodiceScuola(string codice)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<List<News>>> GetNewsMyScuole()
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<List<News>>> GetNewsMyClassi()
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> ThankYouNewsScuola(int idNews)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> ThankYouNewsClasse(int idNews)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<News>> LeggiNewsScuola(int idNews)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> NotificaLetturaScuola(int idNews)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<News>> LeggiNewsClasse(int idNews)
        {
            throw new NotImplementedException();
        }

        public Task<Envelop<string>> NotificaLetturaClasse(int idNews)
        {
            throw new NotImplementedException();
        }
        #endregion


        private async Task<Envelop<T>> sendRequest<T>(string url, HttpContent postContent = null, bool loginRequired = true)
        {
            Envelop<T> envelop = new Envelop<T>();

            if (loginRequired && !IsLogged)
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
                    OnAccessCodeError?.Invoke();
                    return envelop;
                }
            }
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
                    Debug.WriteLine("STAUS CODE: " + envelop.response);
                    if (typeof(T) == typeof(string))
                        envelop.content = (T)(object)result["content"];
                    else
                        envelop.content = JsonConvert.DeserializeObject<T>(result["content"]);
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
            Envelop<ContentType> envelop = new Envelop<ContentType>();

            if (parseAction == null)
                return await sendRequest<ContentType>(url, postContent, loginRequired);

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
                    OnAccessCodeError?.Invoke();
                    return envelop;
                }
            }
            
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
                    var values = JsonConvert.DeserializeObject<ContentContainer>(result["content"].ToString());
                    envelop.content = parseAction.Invoke(values);
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
                if(ListaCategorie == null)
                    ListaCategorie = new Dictionary<string, string>();

                ListaCategorie.Add("Comune", "Comune");
                ListaCategorie.Add("Pro Loco", "ProLoco");
                //ListaCategorie.Add("", "");
                //ListaCategorie.Add("", "");
            }
            return ListaCategorie;
        }
        private Dictionary<string, string> ListaGradiScuola;
        public Dictionary<string, string> GetListaGradiScuola()
        {
            if (ListaGradiScuola == null || !ListaGradiScuola.Any())
            {
                if (ListaGradiScuola == null)
                    ListaGradiScuola = new Dictionary<string, string>();
                ListaGradiScuola.Add("Scuola dell'infanzia", "materna");
                ListaGradiScuola.Add("Scuola elementare", "primaria");
                ListaGradiScuola.Add("Scuola media", "secondaria1");
                ListaGradiScuola.Add("Scuola superiore", "secondaria2");
                //ListaGradiScuola.Add("Università", "universita");
            }
            return ListaGradiScuola;
        }
        private bool IsLogged = false;
        public string AccessCode { get; set; }
        public Action OnAccessCodeError { get; set; }
    }
}
