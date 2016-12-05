using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class PostaNewsEditorPage : ContentPage
    {
        public PostaNewsEditorPage()
        {
            InitializeComponent();
            PostaNewsEditorPageViewModel vm = App.Locator.PostaNewsEditorPageVM;
            this.BindingContext = vm;
            vm.PropertyChanged += Vm_PropertyChanged;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(VM.ListaEditor)))
                CaricaEditorPicker();
        }
        private PostaNewsEditorPageViewModel VM => this.BindingContext as PostaNewsEditorPageViewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.NavigatedTo();
        }
        private void CaricaEditorPicker()
        {
            editorPicker.Items?.Clear();
            if (VM.ListaEditor != null)
            {
                foreach (var item in VM.ListaEditor)
                    editorPicker.Items.Add(item.nome);
            }
        }
    }
}
