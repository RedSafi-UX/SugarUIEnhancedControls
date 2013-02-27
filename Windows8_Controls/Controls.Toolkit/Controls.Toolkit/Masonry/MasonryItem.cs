using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Controls.Toolkit
{
    public sealed class MasonryItem : Control
    {
        public event EventHandler<MasonryItemClickArgs> ItemButtonClick;

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(MasonryItem), new PropertyMetadata(0));

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(MasonryItem), new PropertyMetadata(0));

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(MasonryItem), new PropertyMetadata(null));

        public DisplayMode DisplayMode
        {
            get { return (DisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(DisplayMode), typeof(MasonryItem), new PropertyMetadata(DisplayMode.Image));

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(MasonryItem), new PropertyMetadata(0));

        public string ImageSource
        {
            get { return (string)GetValue(IamgeSourceProperty); }
            set { SetValue(IamgeSourceProperty, value); }
        }

        public static readonly DependencyProperty IamgeSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(MasonryItem), new PropertyMetadata(string.Empty, (s, e) =>
            {
                if (s != null && e.NewValue != null)
                {
                    (s as MasonryItem).SetContent();
                }
            }));

        public void SetContent()
        {
            if (this.DisplayMode == DisplayMode.Image)
            {
                this.Content = new Image() { Source = new BitmapImage(new Uri(this.ImageSource, UriKind.RelativeOrAbsolute) { }), Stretch = Stretch.UniformToFill };
            }
            else if (this.DisplayMode == Toolkit.DisplayMode.Button)
            {
                Image img = new Image() { Source = new BitmapImage(new Uri(this.ImageSource, UriKind.RelativeOrAbsolute) { }), Stretch = Stretch.UniformToFill };
                Button btn = new Button() { Margin = new Thickness(0), Padding = new Thickness(0), Content = img };
                btn.Click += btn_Click;
                this.Content = btn;
            }
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.ItemButtonClick != null)
            {
                this.ItemButtonClick(this, new MasonryItemClickArgs() { ItemID = this.Id });
            }
        }

        public void CleanUp()
        {
            if (this.Content == null)
                return;
            if (this.Content is Button)
            {
                Button btn = this.Content as Button;
                btn.Click -= btn_Click;
                Image img = btn.Content as Image;
                if (img != null)
                {
                    BitmapImage bitmap = img.Source as BitmapImage;
                    bitmap.UriSource = null;
                    bitmap = null;
                    img.Source = null;
                    img = null;
                }
                this.Content = null;
                return;
            }
            else if (this.Content is Image)
            {
                Image img = this.Content as Image;
                if (img != null)
                {
                    BitmapImage bitmap = img.Source as BitmapImage;
                    bitmap.UriSource = null;
                    bitmap = null;
                    img.Source = null;
                    img = null;
                }
                this.Content = null;
                return;
            }
            else
            {
                this.Content = null;
            }

        }

        public MasonryItem()
        {
            this.DefaultStyleKey = typeof(MasonryItem);
        }

        public MasonryItem(double itemWidth, double itemHeight, string imageSource, DisplayMode displaymode = DisplayMode.Image)
        {
            this.DefaultStyleKey = typeof(MasonryItem);
            this.ItemWidth = itemWidth;
            this.ItemHeight = itemHeight;
            this.ImageSource = imageSource;
            this.DisplayMode = displaymode;
        }
    }
    public class MasonryItemClickArgs : EventArgs
    {
        public int ItemID { get; set; }
    }

    public enum DisplayMode
    {
        Image = 0,
        Button = 1,
        Content = 2,
    }
}
