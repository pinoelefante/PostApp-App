using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class RegistraEditorPage : ContentPage
    {
        public RegistraEditorPage()
        {
            InitializeComponent();
            this.BindingContext = App.Locator.RegistraEditorPageVM;
        }
        private RegistraEditorPageViewModel VM => (this.BindingContext as RegistraEditorPageViewModel);
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.NavigatedTo();
            categoryPicker.Items.Clear();
            foreach (var item in VM.Categorie)
                categoryPicker.Items.Add(item.Key);
        }
    }
}
