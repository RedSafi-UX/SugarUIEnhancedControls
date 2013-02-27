using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows8_Controls_Showcase.Common;
using Windows8_Controls_Showcase.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Windows8_Controls_Showcase.ControlsSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CascadingMenuTest : Page
    {
        public CascadingMenuTest()
        {
            this.InitializeComponent();

            List.Clear();
            List<string> Title = new List<string> {"News","Sports","Movie","Travel" };
            for (int i = 0; i < 4; i++)
            {
                List.Add(new TestData { SomeName = Title[i], DataList = new List<TestData>() });
                for (int j = 1; j < 5; j++)
                {
                    List[i].DataList.Add(new TestData { SomeName = "Sectrion " + j.ToString() });
                }
            }
        }

        public ObservableCollection<TestData> List
        {
            get { return (ObservableCollection<TestData>)GetValue(ListProperty); }
            set { SetValue(ListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for List.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ListProperty =
            DependencyProperty.Register("List", typeof(ObservableCollection<TestData>), typeof(CascadingMenuTest), new PropertyMetadata(new ObservableCollection<TestData>()));



        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.cascading.ItemsSource = List;


        }
        public class TestData
        {
            public string SomeName { get; set; }
            public List<TestData> DataList { get; set; }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            App.NavigateToPage(typeof(MainHub));
        }
    }
}
