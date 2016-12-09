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
            loadMoreButton.Clicked += (s, e) => VM.CaricaAltreNewsCommand.Execute(null);
            VM.PropertyChanged += VM_PropertyChanged;
        }

        private void VM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(VM.LoadMoreVisibility)))
                loadMoreButton.IsVisible = VM.LoadMoreVisibility;
        }

        private MainViewModel VM => this.BindingContext as MainViewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.NavigatedTo();
            loadMoreButton.IsVisible = VM.LoadMoreVisibility;
        }
        protected override bool OnBackButtonPressed()
        {
            VM.AskClose();
            return true;
        }
    }
}
