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
    [TemplatePart(Name = "TimerSliderHour", Type = typeof(TimerSlider))]
    [TemplatePart(Name = "TimerSliderMinute", Type = typeof(TimerSlider))]
    [TemplatePart(Name = "TxtSplit", Type = typeof(TextBlock))]
    public sealed class TimePicker : Control
    {
        public TimerSlider TimerSliderHour;
        public TimerSlider TimerSliderMinute;
        public TextBlock TxtSplit;
        private DateTime selectedTime;

        public Brush ForegroundBrush
        {
            get { return (Brush)GetValue(ForegroundBrushProperty); }
            set { SetValue(ForegroundBrushProperty, value); }
        }

        public static readonly DependencyProperty ForegroundBrushProperty =
            DependencyProperty.Register("ForegroundBrush", typeof(Brush), typeof(TimePicker), new PropertyMetadata(null));

        public TimePicker()
        {
            this.DefaultStyleKey = typeof(TimePicker);
        }

        public event Action<DateTime> DateTimeChanged;
        private void OnDateTimeChange(DateTime datetime)
        {
            Action<DateTime> handler = DateTimeChanged;
            if (handler != null)
                handler(datetime);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TimerSliderHour = GetTemplateChild("TimerSliderHour") as TimerSlider;
            TimerSliderMinute = GetTemplateChild("TimerSliderMinute") as TimerSlider;
            TxtSplit = GetTemplateChild("TxtSplit") as TextBlock;
            selectedTime = DateTime.Now;
            TimerSliderHour.DateTimeChanged += TimerSliderHour_DateTimeChanged;
            TimerSliderHour.Tapped += TimerSliderHour_Tapped;
            TimerSliderMinute.DateTimeChanged += TimerSliderMinute_DateTimeChanged;
            TimerSliderMinute.Tapped += TimerSliderMinute_Tapped;
            if (this.ForegroundBrush != null)
            {
                this.TimerSliderHour.Foreground = ForegroundBrush;
                this.TimerSliderMinute.Foreground = ForegroundBrush;
                this.TxtSplit.Foreground = ForegroundBrush;
            }
        }

        void TimerSliderMinute_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (TimerSliderHour.TicksListVisibility == Windows.UI.Xaml.Visibility.Visible)
            {
                TimerSliderMinute.TicksListVisibility = Visibility.Visible;
            }
            TimerSliderHour.SetTime();
            TimerSliderHour.TicksListVisibility = Visibility.Collapsed;
        }

        void TimerSliderHour_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (TimerSliderMinute.TicksListVisibility == Windows.UI.Xaml.Visibility.Visible)
            {
                TimerSliderHour.TicksListVisibility = Visibility.Visible;
            }
            TimerSliderMinute.SetTime();
            TimerSliderMinute.TicksListVisibility = Visibility.Collapsed;
        }

        void TimerSliderMinute_DateTimeChanged(DateTime obj)
        {
            selectedTime = selectedTime.ToMinute(obj.Minute);
            OnDateTimeChange(selectedTime);
        }

        void TimerSliderHour_DateTimeChanged(DateTime obj)
        {
            selectedTime = selectedTime.ToHour(obj.Hour);
            OnDateTimeChange(selectedTime);
        }
    }
}
