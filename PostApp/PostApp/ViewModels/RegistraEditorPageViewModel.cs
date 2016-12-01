using PostApp.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace PostApp.ViewModels
{
    public class RegistraEditorPageViewModel : MyViewModel
    {
        private IPostAppApiService postApp;
        public Dictionary<string, string> Categorie { get; set; }
        private int _categoriaIndex = 0;

        public RegistraEditorPageViewModel(IPostAppApiService api)
        {
            postApp = api;
            Categorie = postApp.GetListaCategorie();
        }
        public override void NavigatedTo(object parameter = null)
        {
        }
        public int CategoriaIndexSelected { get { return _categoriaIndex; } set { Set(ref _categoriaIndex, value); Debug.WriteLine($"Categoria selezionata: {Categorie[Categorie.Keys.ElementAt(_categoriaIndex)]}"); } }
    }
}
