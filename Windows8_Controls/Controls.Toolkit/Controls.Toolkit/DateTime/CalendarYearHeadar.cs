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
    public sealed class CalendarYearHeadar : Control
    {
        Button btnLeft;
        Button btnRight;
        public event RoutedEventHandler LeftButtonClick;
        public event RoutedEventHandler RightButtonClick;

        public CalendarYearHeadar()
        {
            this.DefaultStyleKey = typeof(CalendarYearHeadar);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            btnLeft = GetTemplateChild("btnLeft") as Button;
            btnRight = GetTemplateChild("btnRight") as Button;
            this.btnLeft.Click += btnLeft_Click;
            this.btnRight.Click += btnRight_Click;
            this.DataContext = this;
        }

        void btnRight_Click(object sender, RoutedEventArgs e)
        {
            OnRightButtonClick(sender, e);           
        }

        void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            OnLeftButtonClick(sender, e);
        }

        public string MonthTitle
        {
            get { return (string)GetValue(MonthTitleProperty); }
            set { SetValue(MonthTitleProperty, value); }
        }

        public static readonly DependencyProperty MonthTitleProperty =
            DependencyProperty.Register("MonthTitle", typeof(string), typeof(CalendarYearHeadar), new PropertyMetadata(DateTime.Now.Month));

        public string YearTitle
        {
            get { return (string)GetValue(YearTitleProperty); }
            set { SetValue(YearTitleProperty, value); }
        }

        public static readonly DependencyProperty YearTitleProperty =
            DependencyProperty.Register("YearTitle", typeof(string), typeof(CalendarYearHeadar), new PropertyMetadata(DateTime.Now.Year));

        private void OnLeftButtonClick(object sender, RoutedEventArgs e)
        {
            RoutedEventHandler handler = LeftButtonClick;
            if (handler != null)
                handler(sender, e);
        }

        private void OnRightButtonClick(object sender, RoutedEventArgs e)
        {
            RoutedEventHandler handler = RightButtonClick;
            if (handler != null)
                handler(sender, e);
        }
    }
}
