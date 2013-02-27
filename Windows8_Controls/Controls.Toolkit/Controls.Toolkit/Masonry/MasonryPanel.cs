using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Controls.Toolkit
{
    [TemplatePart(Name = "PanelBig", Type = typeof(StackPanel))]
    [TemplatePart(Name = "PanelA", Type = typeof(StackPanel))]
    [TemplatePart(Name = "PanelB", Type = typeof(StackPanel))]
    [TemplatePart(Name = "Panel1", Type = typeof(StackPanel))]
    [TemplatePart(Name = "Panel2", Type = typeof(StackPanel))]
    [TemplatePart(Name = "Panel3", Type = typeof(StackPanel))]
    [TemplatePart(Name = "Panel4", Type = typeof(StackPanel))]
    public sealed class MasonryPanel : Control
    {
        public MasonryPanel()
        {
             this.DefaultStyleKey = typeof(MasonryPanel);
            this.Loaded += MasonryPanel_Loaded;
            this.Unloaded += MasonryPanel_Unloaded;
        }

        void MasonryPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Panel1.Children.Clear();
            this.Panel2.Children.Clear();
            this.Panel3.Children.Clear();
            this.Panel4.Children.Clear();
            this.PanelA.Children.Clear();
            this.PanelB.Children.Clear();
            this.PanelBig.Children.Clear();
            foreach (var item in this.Items)
            {
                item.CleanUp();
            }
            this.Items.Clear();
        }

        public void ResetLayout()
        {
            if (loadOK && tempOK)
            {
                this.PanelBig.Children.Clear();
                this.PanelA.Children.Clear();
                this.PanelB.Children.Clear();
                this.Panel1.Children.Clear();
                this.Panel2.Children.Clear();
                this.Panel3.Children.Clear();
                this.Panel4.Children.Clear();

                Panel1.Margin = new Thickness(0);
                Panel2.Margin = new Thickness(0);
                Panel3.Margin = new Thickness(0);
                Panel4.Margin = new Thickness(0);
                PanelA.Margin = new Thickness(0);
                PanelB.Margin = new Thickness(0);
                PanelBig.Margin = new Thickness(0);
                Panel1.Height = 0;
                Panel2.Height = 0;
                Panel3.Height = 0;
                Panel4.Height = 0;
                PanelA.Height = 0;
                PanelB.Height = 0;
                PanelBig.Height = 0;

                double ControlHeight = this.MaxnumHeight < this.ActualHeight ? this.MaxnumHeight : this.ActualHeight;
                int currentIndex = 0;
                double WidthA = 0;
                double WidthB = 0;

                double Width1 = 0;
                double Width2 = 0;
                double Width3 = 0;
                double Width4 = 0;

                #region Row1 Layout
                if (
                    this.LargeImageNumber > 0
                    &&
                    (
                        this.LayoutType == MasonryPanelLayoutType.Auto
                        || this.LayoutType == MasonryPanelLayoutType.Row1
                        || this.LayoutType == MasonryPanelLayoutType.Row12
                        || this.LayoutType == MasonryPanelLayoutType.Row124
                        || this.LayoutType == MasonryPanelLayoutType.Row13
                        || this.LayoutType == MasonryPanelLayoutType.Row14
                    )
                )
                {
                    PanelBig.Height = ControlHeight;

                    while ((currentIndex < this.LargeImageNumber || this.LayoutType == MasonryPanelLayoutType.Row1) && currentIndex < this.Items.Count)
                    {

                        var item = this.Items[currentIndex];
                        item.Margin = new Thickness(0, 0, this.ElementInterval, 0);
                        item.Height = ControlHeight;
                        item.Width = (ControlHeight / item.ItemHeight) * item.ItemWidth;
                        PanelBig.Children.Add(item);
                        currentIndex++;

                    }
                }
                #endregion
                #region Row2 Layout
                if (
                     this.MiddleImageNumber > 0 &&
                     (
                        (this.LayoutType == MasonryPanelLayoutType.Auto && ControlHeight > 900)
                        || this.LayoutType == MasonryPanelLayoutType.Row2
                        || this.LayoutType == MasonryPanelLayoutType.Row12
                        || this.LayoutType == MasonryPanelLayoutType.Row124
                        || this.LayoutType == MasonryPanelLayoutType.Row24
                    )
                )
                {
                    int middleImgNum = 0;
                    double itemHeight = (ControlHeight - this.ElementInterval) / 2.0;
                    PanelA.Margin = new Thickness(0);
                    PanelB.Margin = new Thickness(0, this.ElementInterval, 0, 0);
                    PanelA.Height = itemHeight;
                    PanelB.Height = itemHeight;
                    while (currentIndex < this.Items.Count && (middleImgNum < this.MiddleImageNumber || this.LayoutType == MasonryPanelLayoutType.Row12 || this.LayoutType == MasonryPanelLayoutType.Row2))
                    {
                        if (WidthA <= WidthB)
                        {
                            var item = this.Items[currentIndex];
                            item.Margin = new Thickness(0, 0, this.ElementInterval, 0);
                            item.Height = itemHeight;
                            var newWidth = (itemHeight / item.ItemHeight) * item.ItemWidth;
                            item.Width = newWidth;
                            WidthA += newWidth;
                            Width1 += newWidth;
                            Width2 += newWidth;
                            PanelA.Children.Add(item);
                        }
                        else
                        {
                            var item = this.Items[currentIndex];
                            item.Margin = new Thickness(0, 0, this.ElementInterval, 0);
                            item.Height = itemHeight;
                            var newWidth = (itemHeight / item.ItemHeight) * item.ItemWidth;
                            item.Width = newWidth;
                            WidthB += newWidth;
                            Width3 += newWidth;
                            Width4 += newWidth;
                            PanelB.Children.Add(item);
                        }
                        currentIndex++;
                        middleImgNum++;
                    }

                }
                #endregion
                #region Row3 Layout
                if (
                    (this.LayoutType == MasonryPanelLayoutType.Auto && ControlHeight <= 900)
                    || this.LayoutType == MasonryPanelLayoutType.Row13
                    || this.LayoutType == MasonryPanelLayoutType.Row3
                )
                {
                    double itemHeight = (ControlHeight - this.ElementInterval * 2) / 3.0;
                    Panel1.Margin = new Thickness(0);
                    Panel2.Margin = new Thickness(0, this.ElementInterval, 0, 0);
                    Panel3.Margin = new Thickness(0, this.ElementInterval, 0, 0);
                    Panel1.Height = itemHeight;
                    Panel2.Height = itemHeight;
                    Panel3.Height = itemHeight;
                    Panel4.Height = 0;
                    while (currentIndex < this.Items.Count)
                    {
                        if (Width1 <= Width2 && Width1 <= Width3)
                        {
                            var item = this.Items[currentIndex];
                            item.Margin = new Thickness(0, 0, this.ElementInterval, 0);
                            item.Height = itemHeight;
                            var newWidth = (itemHeight / item.ItemHeight) * item.ItemWidth;
                            item.Width = newWidth;
                            Width1 += newWidth;
                            Panel1.Children.Add(item);
                        }
                        else if (Width2 <= Width1 && Width2 <= Width3)
                        {
                            var item = this.Items[currentIndex];
                            item.Margin = new Thickness(0, 0, this.ElementInterval, 0);
                            item.Height = itemHeight;
                            var newWidth = (itemHeight / item.ItemHeight) * item.ItemWidth;
                            item.Width = newWidth;
                            Width2 += newWidth;
                            Panel2.Children.Add(item);
                        }
                        else if (Width3 <= Width1 && Width3 <= Width2)
                        {
                            var item = this.Items[currentIndex];
                            item.Margin = new Thickness(0, 0, this.ElementInterval, 0);
                            item.Height = itemHeight;
                            var newWidth = (itemHeight / item.ItemHeight) * item.ItemWidth;
                            item.Width = newWidth;
                            Width3 += newWidth;
                            Panel3.Children.Add(item);
                        }
                        currentIndex++;
                    }

                }

                #endregion
                #region Row4 Layout
                if (
                    (this.LayoutType == MasonryPanelLayoutType.Auto && ControlHeight > 900)
                    || this.LayoutType == MasonryPanelLayoutType.Row124
                    || this.LayoutType == MasonryPanelLayoutType.Row14
                    || this.LayoutType == MasonryPanelLayoutType.Row24
                    || this.LayoutType == MasonryPanelLayoutType.Row4
                )
                {
                    double itemHeight = (ControlHeight - this.ElementInterval * 3) / 4.0;

                    Panel1.Margin = new Thickness(0);
                    Panel2.Margin = new Thickness(0, this.ElementInterval, 0, 0);
                    Panel3.Margin = new Thickness(0, this.ElementInterval, 0, 0);
                    Panel4.Margin = new Thickness(0, this.ElementInterval, 0, 0);
                    Panel1.Height = itemHeight;
                    Panel2.Height = itemHeight;
                    Panel3.Height = itemHeight;
                    Panel4.Height = itemHeight;
                    while (currentIndex < this.Items.Count)
                    {
                        if (Width1 <= Width2 && Width1 <= Width3 && Width1 <= Width4)
                        {
                            var item = this.Items[currentIndex];
                            item.Margin = new Thickness(0, 0, this.ElementInterval, 0);
                            item.Height = itemHeight;
                            var newWidth = (itemHeight / item.ItemHeight) * item.ItemWidth;
                            item.Width = newWidth;
                            Width1 += newWidth;
                            Panel1.Children.Add(item);
                        }
                        else if (Width2 <= Width1 && Width2 <= Width3 && Width2 < Width4)
                        {
                            var item = this.Items[currentIndex];
                            item.Margin = new Thickness(0, 0, this.ElementInterval, 0);
                            item.Height = itemHeight;
                            var newWidth = (itemHeight / item.ItemHeight) * item.ItemWidth;
                            item.Width = newWidth;
                            Width2 += newWidth;
                            Panel2.Children.Add(item);
                        }
                        else if (Width3 <= Width1 && Width3 <= Width2 && Width3 <= Width4)
                        {
                            var item = this.Items[currentIndex];
                            item.Margin = new Thickness(0, 0, this.ElementInterval, 0);
                            item.Height = itemHeight;
                            var newWidth = (itemHeight / item.ItemHeight) * item.ItemWidth;
                            item.Width = newWidth;
                            Width3 += newWidth;
                            Panel3.Children.Add(item);
                        }
                        else if (Width4 <= Width1 && Width4 <= Width2 && Width4 <= Width3)
                        {
                            var item = this.Items[currentIndex];
                            item.Margin = new Thickness(0, 0, this.ElementInterval, 0);
                            item.Height = itemHeight;
                            var newWidth = (itemHeight / item.ItemHeight) * item.ItemWidth;
                            item.Width = newWidth;
                            Width4 += newWidth;
                            Panel4.Children.Add(item);
                        }
                        currentIndex++;
                    }
                }
                #endregion
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            tempOK = true;
            if (PanelBig != null)
            {
                PanelBig.Children.Clear();
                PanelA.Children.Clear();
                PanelB.Children.Clear();
            }
            Panel1 = this.GetTemplateChild("Panel1") as StackPanel;
            Panel2 = this.GetTemplateChild("Panel2") as StackPanel;
            Panel3 = this.GetTemplateChild("Panel3") as StackPanel;
            Panel4 = this.GetTemplateChild("Panel4") as StackPanel;
            PanelA = this.GetTemplateChild("PanelA") as StackPanel;
            PanelB = this.GetTemplateChild("PanelB") as StackPanel;
            PanelBig = this.GetTemplateChild("PanelBig") as StackPanel;
            ResetLayout();
        }

        private void MasonryPanel_Loaded(object sender, RoutedEventArgs e)
        {
            loadOK = true;
            ResetLayout();
        }

        #region

        private StackPanel Panel1;
        private StackPanel Panel2;
        private StackPanel Panel3;
        private StackPanel Panel4;
        private StackPanel PanelA;
        private StackPanel PanelB;
        private StackPanel PanelBig;

        private bool loadOK = false;
        private bool tempOK = false;

        #endregion

        #region Items
        public List<MasonryItem> Items
        {
            get { return (List<MasonryItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(List<MasonryItem>), typeof(MasonryPanel), new PropertyMetadata(new List<MasonryItem>(), (s, e) =>
            {
                var panel = s as MasonryPanel;
                if (panel != null && panel.Items != null && panel.Items.Count > 0)
                {
                    panel.ResetLayout();
                }
            }));
        #endregion

        #region LayoutType

        public MasonryPanelLayoutType LayoutType
        {
            get { return (MasonryPanelLayoutType)GetValue(LayoutTypeProperty); }
            set { SetValue(LayoutTypeProperty, value); }
        }

        public static readonly DependencyProperty LayoutTypeProperty =
            DependencyProperty.Register("LayoutType", typeof(MasonryPanelLayoutType), typeof(MasonryPanel), new PropertyMetadata(MasonryPanelLayoutType.Auto, (s, e) =>
            {
                var panel = s as MasonryPanel;
                if (panel != null && panel.Items != null && panel.Items.Count > 0)
                {
                    panel.ResetLayout();
                }
            }));

        #endregion

        #region MaxnumHeight


        public double MaxnumHeight
        {
            get { return (double)GetValue(MaxnumHeightProperty); }
            set { SetValue(MaxnumHeightProperty, value); }
        }

        public static readonly DependencyProperty MaxnumHeightProperty =
            DependencyProperty.Register("MaxnumHeight", typeof(double), typeof(MasonryPanel), new PropertyMetadata(900.0, (s, e) =>
            {
                var panel = s as MasonryPanel;
                if (panel != null && panel.Items != null && panel.Items.Count > 0)
                {
                    panel.ResetLayout();
                }
            }));
        #endregion

        #region ElementInterval

        public double ElementInterval
        {
            get { return (double)GetValue(ElementIntervalProperty); }
            set { SetValue(ElementIntervalProperty, value); }
        }

        public static readonly DependencyProperty ElementIntervalProperty =
            DependencyProperty.Register("ElementInterval", typeof(double), typeof(MasonryPanel), new PropertyMetadata(0.0, (s, e) =>
            {
                var panel = s as MasonryPanel;
                if (panel != null && panel.Items != null && panel.Items.Count > 0)
                {
                    panel.ResetLayout();
                }
            }));

        #endregion

        #region LargeImageNumber

        public int LargeImageNumber
        {
            get { return (int)GetValue(LargeImageNumberProperty); }
            set { SetValue(LargeImageNumberProperty, value); }
        }

        public static readonly DependencyProperty LargeImageNumberProperty =
            DependencyProperty.Register("LargeImageNumber", typeof(int), typeof(MasonryPanel), new PropertyMetadata(1, (s, e) =>
            {
                var panel = s as MasonryPanel;
                if (panel != null && panel.Items != null && panel.Items.Count > 0)
                {
                    panel.ResetLayout();
                }
            }));

        #endregion

        #region MiddleImageNumber

        public int MiddleImageNumber
        {
            get { return (int)GetValue(MiddleImageNumberProperty); }
            set { SetValue(MiddleImageNumberProperty, value); }
        }

        public static readonly DependencyProperty MiddleImageNumberProperty =
            DependencyProperty.Register("MiddleImageNumber", typeof(int), typeof(MasonryPanel), new PropertyMetadata(2, (s, e) =>
            {
                var panel = s as MasonryPanel;
                if (panel != null && panel.Items != null && panel.Items.Count > 0)
                {
                    panel.ResetLayout();
                }
            }));

        #endregion
        
    }

    public enum MasonryPanelLayoutType
    {
        Auto,
        Row1,
        Row12,
        Row13,
        Row14,
        Row124,
        Row24,
        Row2,
        Row3,
        Row4
    }
}
