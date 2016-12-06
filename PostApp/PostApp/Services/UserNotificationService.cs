using Acr.UserDialogs;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Services
{
    public class UserNotificationService
    {
        public void ToastNotification(string titolo, string messaggio)
        {
            CrossLocalNotifications.Current.Show(titolo, messaggio);
        }
        public void ShowMessageDialog(string title, string message, Action action = null)
        {
            UserDialogs.Instance.Alert(new AlertConfig()
            {
                Title = title,
                Message = message,
                OkText = "OK",
                OnAction = action
            });
        }
        public void ConfirmDialog(string titolo, string messaggio, Action<bool> action = null)
        {
            UserDialogs.Instance.Confirm(new ConfirmConfig()
            {
                Title = titolo,
                Message = messaggio,
                CancelText = "No",
                OkText = "Si",
                OnAction = action
            });
        }
    }
}
