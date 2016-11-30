using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class FirstAccessPage : ContentPage
    {
        public FirstAccessPage()
        {
            InitializeComponent();
            BindingContext = App.Locator.FirstAccessVM;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Locator.FirstAccessVM.NavigatedTo();
        }
    }
}
