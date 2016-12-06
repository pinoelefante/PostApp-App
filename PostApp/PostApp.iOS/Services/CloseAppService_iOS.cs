using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using PostApp.Services;
using PostApp.iOS.Services;
using System.Threading;

[assembly: Xamarin.Forms.Dependency(typeof(CloseAppService_iOS))]
namespace PostApp.iOS.Services
{
    public class CloseAppService_iOS : IClosingApp
    {
        public void CloseApp()
        {
            Thread.CurrentThread.Abort();
        }
    }
}
