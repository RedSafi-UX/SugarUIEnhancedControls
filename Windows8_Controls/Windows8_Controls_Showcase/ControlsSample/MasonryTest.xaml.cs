using Controls.Toolkit;
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
    public sealed partial class MasonryTest : Page
    {
        public MasonryTest()
        {
            this.InitializeComponent();

            this.Loaded += MasonryTest_Loaded;
        }

        void MasonryTest_Loaded(object sender, RoutedEventArgs e)
        {
            mp.Items = GetTestSample();
        }

        private List<MasonryItem> GetTestSample()
        {
            List<MasonryItem> list = new List<MasonryItem>();
            for (int i = 0; i < 10; i++)
            {

                list.Add(new MasonryItem() { ItemWidth = 436, ItemHeight = 544, ImageSource = "http://images-fast.digu.com/sp/width/619/94e0f31d11dc416e95e621dac27c650a0003.jpg?f=detail" });
                list.Add(new MasonryItem() { ItemWidth = 400, ItemHeight = 600, ImageSource = "http://img.hb.aicdn.com/4eb0223b2d3c98b9a3a6ed1c4a3eb880f3c18b101b22c-en8GMJ_fw554" });

                list.Add(new MasonryItem() { ItemWidth = 300, ItemHeight = 450, ImageSource = "http://images-fast.digu.com/sp/width/619/c6ed0e9e33f54441b1f19e92d8ed36850004.jpg?f=detail" });
                list.Add(new MasonryItem() { ItemWidth = 604, ItemHeight = 402, ImageSource = "http://pic3.nipic.com/20090630/2955768_130823013_2.jpg" });

                list.Add(new MasonryItem(554, 370, "http://img.hb.aicdn.com/0fac4c2984a526b0cfcf8ece36d9eec41b49a3b59745-uA88yw_fw554"));
                list.Add(new MasonryItem(450, 450, "http://img.hb.aicdn.com/249527e9e96b2bb1ae55bb4acd614d28bc1146981eb39-zSQKqN_fw554"));

                list.Add(new MasonryItem(554, 402, "http://img.hb.aicdn.com/1d38a7ed5cc4503fde8fc748784f545a97c4d6563e3b2-nS6MdJ_fw554"));
                list.Add(new MasonryItem(250, 313, "http://img.hb.aicdn.com/8de66bb462c4ef2233b70b50d435f8a254f2dbf48567-hGp9Dz_fw554"));
                list.Add(new MasonryItem(500, 375, "http://img.hb.aicdn.com/10666b89bfb4d13b4da794264ec686595a598679225a4-CyHlHC_fw554"));
                list.Add(new MasonryItem(444, 444, "http://images-fast.digu.com/sp/width/619/f803740fe2b64ae288c2a6eb690020010003.jpg?f=detail"));
                list.Add(new MasonryItem(433, 650, "http://f12.topit.me/l159/10159027947833b956.jpg"));
                list.Add(new MasonryItem(1000, 611, "http://img13.meitudata.com/506e4bc4deb4e2310.jpg!thumbnail1000"));
                list.Add(new MasonryItem(1000, 667, "http://img13.meitudata.com/506d47ee3a3d93993.jpg!thumbnail1000"));
                list.Add(new MasonryItem(1000, 589, "http://img13.meitudata.com/506d3857dbdac5531.jpg!thumb100"));
                list.Add(new MasonryItem(1000, 672, "http://img13.meitudata.com/506d1a97609224234.jpg!thumbnail1000"));
                list.Add(new MasonryItem(1000, 605, "http://img14.meitudata.com/50723380a5c012180.jpg!thumbnail1000"));
                list.Add(new MasonryItem(1000, 667, "http://img14.meitudata.com/507b7c627803b3822.jpg!thumbnail1000"));
                list.Add(new MasonryItem(1000, 444, "http://img13.meitudata.com/201210/23/e76c5l97kz904uqyij.jpg!thumbnail1000"));
                list.Add(new MasonryItem(1000, 511, "http://img13.meitudata.com/201301/21/3pqaclf2xdoqjfn5p9.jpg!thumbnail1000", DisplayMode.Button));
                list.Add(new MasonryItem(500, 375, "http://img.hb.aicdn.com/b74f08822f38ea15ade84301fa9eca4043b700a0ebb6-uFcpEn_fw554", DisplayMode.Button));
                list.Add(new MasonryItem(500, 365, "http://pic.yupoo.com/wnandy/BvkLaQ7w/JOwRn.jpg", DisplayMode.Button));

            }
            return list;
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
    }
}
