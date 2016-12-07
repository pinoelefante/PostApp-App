using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class CercaEditorPage : ContentPage
    {
        public CercaEditorPage()
        {
            InitializeComponent();
            this.BindingContext = App.Locator.CercaEditorPageVM;
        }
    }
}
