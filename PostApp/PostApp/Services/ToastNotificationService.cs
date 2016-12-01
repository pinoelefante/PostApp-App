using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Services
{
    public class ToastNotificationService
    {
        public void Notify(string titolo, string messaggio)
        {
            CrossLocalNotifications.Current.Show(titolo, messaggio);
        }
    }
}
