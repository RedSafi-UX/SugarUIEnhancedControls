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
    public sealed class CalendarHeader : Control
    {
        public event RoutedEventHandler LeftButtonClick;
        public event RoutedEventHandler RightButtonClick;

        private Button btnRight;
        public Button BtnRight
        {
            get { return btnRight; }
            set
            {
                if (btnRight != null)
                    btnRight.Click -= btnRight_Click;
                btnRight = value;
                if (btnRight != null)
                    btnRight.Click += btnRight_Click;
            }
        }

        private Button btnLeft;
        public Button BtnLeft
        {
            get { return btnLeft; }
            set
            {
                if (btnLeft != null)
                    btnLeft.Click -= btnLeft_Click;
                btnLeft = value;
                if (btnLeft != null)
                    btnLeft.Click += btnLeft_Click;
            }
        }

        public string YearTitle
        {
            get { return (string)GetValue(YearTitleProperty); }
            set { SetValue(YearTitleProperty, value); }
        }

        public static readonly DependencyProperty YearTitleProperty =
            DependencyProperty.Register("YearTitle", typeof(string), typeof(CalendarHeader), new PropertyMetadata(DateTime.Now.Year.ToString()));

        public string MonthTitle
        {
            get { return (string)GetValue(MonthTitleProperty); }
            set { SetValue(MonthTitleProperty, value); }
        }

        public static readonly DependencyProperty MonthTitleProperty =
            DependencyProperty.Register("MonthTitle", typeof(string), typeof(CalendarHeader), new PropertyMetadata(DateTime.Now.Month.ToString()));


        public CalendarHeader()
        {
            this.DefaultStyleKey = typeof(CalendarHeader);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            BtnLeft = GetTemplateChild("btnLeft") as Button;
            BtnRight = GetTemplateChild("btnRight") as Button;
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
