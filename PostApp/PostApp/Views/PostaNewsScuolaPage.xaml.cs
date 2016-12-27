using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class PostaNewsScuolaPage : TabbedPage
    {
        public PostaNewsScuolaPage()
        {
            InitializeComponent();
            PostaNewsScuolaViewModel vm = App.Locator.PostaNewsScuolaVM;
            this.BindingContext = vm;
            vm.PropertyChanged += Vm_PropertyChanged;
        }
        public PostaNewsScuolaViewModel VM => this.BindingContext as PostaNewsScuolaViewModel;
        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(VM.ElencoScuole)))
                CaricaScuolePicker();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.NavigatedTo();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            VM.NavigatedFrom();
        }
        private void CaricaScuolePicker()
        {
            editorPicker.Items?.Clear();
            editorPicker2.Items?.Clear();
            if (VM.ElencoScuole != null)
            {
                foreach (var item in VM.ElencoScuole)
                {
                    editorPicker.Items.Add(item.nome);
                    editorPicker2.Items.Add(item.nome);
                }
            }
        }
    }
}
