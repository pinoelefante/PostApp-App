using PostApp.Api.Data;
using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class ViewNewsPage : ContentPage
    {
        public ViewNewsPage(News news)
        {
            InitializeComponent();
            newsSelezionata = news;
            this.BindingContext = App.Locator.ViewNewsPageVM;
        }
        private ViewNewsPageViewModel VM => this.BindingContext as ViewNewsPageViewModel;
        private News newsSelezionata;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.NavigatedTo(newsSelezionata);
        }
    }
}
