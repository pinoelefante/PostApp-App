using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PostApp.Controls
{
    public class Accordion : ContentView
    {
        #region Private Variables
        List<AccordionSource> mDataSource;
        bool mFirstExpaned = false;
        StackLayout mMainLayout;
        #endregion

        public Accordion()
        {
            var mMainLayout = new StackLayout();
            Content = mMainLayout;
        }
        public Accordion(List<AccordionSource> aSource)
        {
            mDataSource = aSource;
            DataBind();
        }
        #region Properties
        public List<AccordionSource> DataSource
        {
            get { return mDataSource; }
            set { mDataSource = value; }
        }
        public bool FirstExpaned
        {
            get { return mFirstExpaned; }
            set { mFirstExpaned = value; }
        }
        #endregion

        public void DataBind()
        {
            var vMainLayout = new StackLayout();
            var vFirst = true;
            if (mDataSource != null)
            {
                foreach (var vSingleItem in mDataSource)
                {
                    var vHeaderButton = new AccordionButton()
                    {
                        Text = vSingleItem.HeaderText,
                        TextColor = vSingleItem.HeaderTextColor,
                        BackgroundColor = vSingleItem.HeaderBackGroundColor,
                        ContentId = vSingleItem.ParentId
                    };

                    var vAccordionContent = new ContentView()
                    {
                        Content = vSingleItem.ContentItems,
                        IsVisible = false
                    };
                    if (vFirst)
                    {
                        vHeaderButton.Expand = mFirstExpaned;
                        vAccordionContent.IsVisible = mFirstExpaned;
                        vFirst = false;
                    }
                    vHeaderButton.AssosiatedContent = vAccordionContent;
                    vHeaderButton.Clicked += OnAccordionButtonClicked;
                    vMainLayout.Children.Add(vHeaderButton);
                    vMainLayout.Children.Add(vAccordionContent);
                }
            }
            mMainLayout = vMainLayout;
            Content = mMainLayout;
        }

        void OnAccordionButtonClicked(object sender, EventArgs args)
        {
            if (AccordionButtonClicked != null)
                AccordionButtonClicked.Invoke((sender as AccordionButton).ContentId, () => AccordionButtonExpand(sender));
            else
                AccordionButtonExpand(sender);
        }
        private void AccordionButtonExpand(object sender)
        {
            foreach (var vChildItem in mMainLayout.Children)
            {
                if (vChildItem.GetType() == typeof(ContentView))
                    vChildItem.IsVisible = false;
                if (vChildItem.GetType() == typeof(AccordionButton))
                {
                    var vButton = (AccordionButton)vChildItem;
                    vButton.Expand = false;
                }
            }
            var vSenderButton = (AccordionButton)sender;

            if (vSenderButton.Expand)
                vSenderButton.Expand = false;
            else
                vSenderButton.Expand = true;
            vSenderButton.AssosiatedContent.IsVisible = vSenderButton.Expand;
        }
        public Action<object, Action> AccordionButtonClicked { get; set; }
    }
    public class AccordionSource
    {
        public string HeaderText { get; set; }
        public Color HeaderTextColor { get; set; }
        public Color HeaderBackGroundColor { get; set; }
        public View ContentItems { get; set; }
        public object ParentId { get; set; }
    }
    public class AccordionButton : Button
    {
        #region Private Variables
        bool mExpand = false;
        #endregion
        public AccordionButton()
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            BorderColor = Color.Transparent;
            BorderRadius = 10;
            BorderWidth = 0;
        }
        #region Properties
        public bool Expand
        {
            get { return mExpand; }
            set { mExpand = value; }
        }
        public ContentView AssosiatedContent { get; set; }
        public object ContentId { get; set; }
        #endregion
    }
}
