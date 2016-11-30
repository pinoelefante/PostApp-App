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
            masterPage.ListView.ItemSelected += OnItemSelected;
            App.Locator.NavigationService.Initialize(navigationPage);
            this.BindingContext = App.Locator.MyMasterDetailVM;
        }
        private MyMasterDetailViewModel VM => App.Locator.MyMasterDetailVM;
        private MasterPageItem lastSelectedItem;

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item == lastSelectedItem) //blocca la navigazione se viene selezionato la voce corrente del menu
            {
                IsPresented = false;
                return;
            }
            if (item != null)
            {
                lastSelectedItem = item;
                VM.NavigateCommand.Execute(item.Command);
                //navigationPage.Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetType));
                IsPresented = false;
            }
        }
    }
}
