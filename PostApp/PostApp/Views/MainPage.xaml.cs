using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = App.Locator.MainPageViewModel;
        }
        private MainViewModel VM => this.BindingContext as MainViewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.NavigatedTo();
        }
    }
}
