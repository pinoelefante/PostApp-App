using PostApp.Api.Data;
using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected override bool OnBackButtonPressed()
        {
            VM.AskClose();
            return true;
            //throw new Exception("Not a real exception: just close app");
        }
    }
}
