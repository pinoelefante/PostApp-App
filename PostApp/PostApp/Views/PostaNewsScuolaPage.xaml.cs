using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class PostaNewsScuolaPage : ContentPage
    {
        public PostaNewsScuolaPage()
        {
            InitializeComponent();
            this.BindingContext = App.Locator.PostaNewsScuolaVM;
        }
        public PostaNewsScuolaViewModel VM => this.BindingContext as PostaNewsScuolaViewModel;
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
    }
}
