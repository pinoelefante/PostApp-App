using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class CittaPage : ContentPage
    {
        public CittaPage()
        {
            InitializeComponent();
            this.BindingContext = App.Locator.CittaPageVM;
        }
        private CittaPageViewModel VM => this.BindingContext as CittaPageViewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.NavigatedTo();
        }
    }
}
