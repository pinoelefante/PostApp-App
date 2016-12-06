using PostApp.Services;
using PostApp.UWP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseAppService_UWP))]
namespace PostApp.UWP.Services
{
    public class CloseAppService_UWP : IClosingApp
    {
        public void CloseApp()
        {
            App.Current.Exit();
        }
    }
}
