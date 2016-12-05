using PostApp.Controls;
using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class MyMasterDetail : MasterDetailPage
    {
        public MyMasterDetail()
        {
            InitializeComponent();
            masterPage.ListView.ItemTapped += ListView_ItemTapped;
            this.BindingContext = App.Locator.MyMasterDetailVM;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as MasterPageItem;
            /*
            if (item == null || item == lastSelectedItem) //blocca la navigazione se viene selezionato la voce corrente del menu
            {
                IsPresented = false;
                return;
            }
            */
            lastSelectedItem = item;
            VM.NavigateCommand.Execute(item.Command);
            masterPage.ListView.SelectedItem = null;
            IsPresented = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Locator.MyMasterDetailVM.RegistraNavigation(navigationPage);
        }
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
        private MyMasterDetailViewModel VM => App.Locator.MyMasterDetailVM;
        private MasterPageItem lastSelectedItem;
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item == null || item == lastSelectedItem) //blocca la navigazione se viene selezionato la voce corrente del menu
            {
                IsPresented = false;
                return;
            }
            lastSelectedItem = item;
            VM.NavigateCommand.Execute(item.Command);
            masterPage.ListView.SelectedItem = null;
            IsPresented = false;
        }
    }
}
