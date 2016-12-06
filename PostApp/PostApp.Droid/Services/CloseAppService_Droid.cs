using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PostApp.Services;
using Xamarin.Forms;
using PostApp.Droid.Services;

[assembly: Dependency(typeof(CloseAppService_Droid))]
namespace PostApp.Droid.Services
{
    public class CloseAppService_Droid : IClosingApp
    {
        public void CloseApp()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}