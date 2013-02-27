using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace Controls.Toolkit
{
    public sealed class CalendarItemMonthButton : ToggleButton
    {
        private bool IsLoaded = false;
        private bool IsTemplated = false;

        public CalendarItemMonthButton()
        {
            this.DefaultStyleKey = typeof(CalendarItemMonthButton);

            this.Loaded += CalendarItemMonthButton_Loaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            IsTemplated = true;
            SetState();
        }

        void CalendarItemMonthButton_Loaded(object sender, RoutedEventArgs e)
        {
            IsLoaded = true;
            SetState();
        }
        
        private void SetState()
        {
            if (IsLoaded && IsTemplated)
            {
                Month month = this.DataContext as Month;
                this.Click += CalendarItemMonthButton_Click;
            }
        }

        void CalendarItemMonthButton_Click(object sender, RoutedEventArgs e)
        {
            Month month = this.DataContext as Month;
            month.OnMOnthSelected();
        }
    }
}
