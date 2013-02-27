using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Controls.Toolkit
{
    public static class DateTimeHelper
    {
        public static Button GetButton(string content, Style style, RoutedEventHandler callback, object tag = null)
        {
            Button btn = new Button();
            btn.Content = content;
            btn.Margin = new Thickness(0, 0, 0, 0);
            btn.Tag = tag;
            btn.Loaded += btn_Loaded;
            if (style != null)
                btn.Style = style;
            if (callback != null)
                btn.Click += callback;
            return btn;
        }

        static void btn_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
                Global.TimeContentHeight = btn.ActualHeight;
        }

        public static DateTime ToHour(this DateTime datetime, int hour)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, hour, datetime.Minute, datetime.Second);
        }

        public static DateTime ToMinute(this DateTime datetime, int minute)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, minute, datetime.Second);
        }
    }

    public class Global
    {
        public static double TimeContentHeight = 0.0d;
    }
}
