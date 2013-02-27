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
using Windows8_Controls_Showcase.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Windows8_Controls_Showcase.ControlsSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StepIndicatorTest : Page
    {
        public StepIndicatorTest()
        {
            this.InitializeComponent();
            stepIndicator.SelectIndexChanged += stepIndicator_SelectIndexChanged;
            
        }

        void stepIndicator_SelectIndexChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int index =Convert.ToInt32( e.NewValue);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            App.NavigateToPage(typeof(MainHub));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            stepIndicator.GotoBack();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            stepIndicator.GotoNext();
        }
    }
}
