using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class RegistraScuolaPage : ContentPage
    {
        public RegistraScuolaPage()
        {
            InitializeComponent();
            this.BindingContext = App.Locator.RegistraScuolaPageVM;
        }
    }
}
