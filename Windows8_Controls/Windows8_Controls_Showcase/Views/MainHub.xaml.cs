using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows8_Controls_Showcase.ControlsSample;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Windows8_Controls_Showcase.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainHub : Windows8_Controls_Showcase.Common.LayoutAwarePage
    {
        public MainHub()
        {
            this.InitializeComponent();

            mainGridView.ItemClick += mainGridView_ItemClick;         
        }

        void mainGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //e.ClickedItem

            int itenIndex = mainGridView.Items.IndexOf(e.ClickedItem);

            switch (itenIndex)
            {
                case 0:
                    App.NavigateToPage(typeof(DateTimeTest));
                    break;
                case 1:
                    App.NavigateToPage(typeof(KnobTest));
                    break;
                case 2:
                    App.NavigateToPage(typeof(CascadingMenuTest));
                    break;
                case 3:
                    App.NavigateToPage(typeof(ContentTextBoxTest));
                    break;
                case 4:
                    App.NavigateToPage(typeof(SemanticZoomOutTest));
                    break;
                case 5:
                    App.NavigateToPage(typeof(RangeSliderTest));
                    break;
                case 6:
                    App.NavigateToPage(typeof(StepIndicatorTest));
                    break;
                case 7:
                    App.NavigateToPage(typeof(MasonryTest));
                    break;
             
                default:
                    break;
            }
         
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
    }
}
