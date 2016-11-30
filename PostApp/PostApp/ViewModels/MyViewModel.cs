﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.ViewModels
{
    public class MyViewModel : ViewModelBase
    {
        private bool busyActive;
        public bool IsBusyActive { get { return busyActive; } set { Set(ref busyActive, value); } }
    }
}
