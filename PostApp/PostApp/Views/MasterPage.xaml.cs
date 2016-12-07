using PostApp.Controls;
using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }
        public MasterPage()
        {
            InitializeComponent();

            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Home",
                //IconSource = "todo.png",
                Command = ViewModelLocator.MainPage,
            });
            
            masterPageItems.Add(new MasterPageItem
            {
                Title = "First Access",
                //IconSource = "todo.png",
                Command = ViewModelLocator.FirstAccessPage,
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Cerca editor",
                //IconSource = "todo.png",
                Command = ViewModelLocator.CercaEditorPage,
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Registra Editor",
                Command = ViewModelLocator.RegistraEditorPage
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Pubblica news editor",
                Command = ViewModelLocator.PostaNewsEditorPage
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Registra scuola",
                //IconSource = "todo.png",
                Command = ViewModelLocator.RegistraScuolaPage
            });

            listView.ItemsSource = masterPageItems;
        }
    }
}
