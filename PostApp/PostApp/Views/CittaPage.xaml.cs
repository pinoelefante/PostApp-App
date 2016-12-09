using PostApp.Api.Data;
using PostApp.Controls;
using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class CittaPage : ContentPage
    {
        public CittaPage()
        {
            InitializeComponent();
            this.BindingContext = App.Locator.CittaPageVM;
            VM.PropertyChanged += VM_PropertyChanged;
        }

        private void VM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(VM.Editors)))
                initAccordion();
        }

        private CittaPageViewModel VM => this.BindingContext as CittaPageViewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.NavigatedTo();
        }
        private void initAccordion()
        {
            if (accordion.DataSource!=null && accordion.DataSource.Any()) //quando si torna indietro
                return;

            var dataSource = new List<AccordionSource>();
            foreach (var item in VM.Editors)
            {
                var cittaNome = item.Key;
                var itemList = new ListView()
                {
                    RowHeight = 50,
                    ItemsSource = item.Value,
                    ItemTemplate = new DataTemplate(typeof(ListDataViewCell)),
                };
                itemList.ItemTapped += (s, e) => { /*Debug.WriteLine("ItemTapped");*/ VM.ApriEditor(e.Item as Api.Data.Editor); };
                AccordionSource source = new AccordionSource()
                {
                    HeaderText = item.Key.comune,
                    ContentItems = itemList,
                    ParentId = item.Key.istat
                };
                dataSource.Add(source);
            }
            accordion.AccordionButtonClicked = (objId) =>
            {
                VM.CaricaEditors(objId.ToString());
            };
            accordion.DataSource = dataSource;
            accordion.DataBind();
        }


        public class ListDataViewCell : ViewCell
        {
            public ListDataViewCell()
            {
                var label = new Label()
                {
                    TextColor = Color.Blue
                };
                label.SetBinding(Label.TextProperty, new Binding("nome"));
                label.SetBinding(Label.ClassIdProperty, new Binding("id"));
                View = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Padding = new Thickness(12, 8),
                    Children =
                    {
                        label
                    }
                };
            }
        }
    }
}
