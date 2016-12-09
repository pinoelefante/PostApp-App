using PostApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PostApp.Views
{
    public partial class ViewEditorPage : ContentPage
    {
        public ViewEditorPage(int idEditor)
        {
            InitializeComponent();
            this.BindingContext = App.Locator.ViewEditorPageVM;
            CurrentEditorId = idEditor;
            loadMoreButton.Clicked += (s, e) => VM.LoadMoreNewsCommand.Execute(null);
            VM.PropertyChanged += VM_PropertyChanged;
        }

        private void VM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(VM.LoadMoreVisibility)))
                loadMoreButton.IsVisible = VM.LoadMoreVisibility;
        }

        private ViewEditorPageViewModel VM => this.BindingContext as ViewEditorPageViewModel;
        private int CurrentEditorId;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.CurrentEditorId = CurrentEditorId;
            loadMoreButton.IsVisible = VM.LoadMoreVisibility;
        }
    }
}
