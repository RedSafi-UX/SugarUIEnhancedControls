using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;


namespace Controls.Toolkit
{
    [TemplatePart(Name = "ButtonShowTicks", Type = typeof(Button))]
    [TemplatePart(Name = "ScrollViewerTicks", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "StackPanelTicks", Type = typeof(StackPanel))]
    public sealed class TimerSlider : Control
    {
        private Button ButtonShowTicks;
        private ScrollViewer ScrollViewerTicks;
        private StackPanel StackPanelTicks;
        private double myoffset = 0;
        private int currentValue;

        public event Action<DateTime> DateTimeChanged;
        private void OnDateTimeChanged(DateTime datetime)
        {
            Action<DateTime> handler = DateTimeChanged;
            if (handler != null)
                handler(datetime);
        }

        public Style ShowTicksStyle
        {
            get { return (Style)GetValue(ShowTicksStyleProperty); }
            set { SetValue(ShowTicksStyleProperty, value); }
        }

        public static readonly DependencyProperty ShowTicksStyleProperty =
            DependencyProperty.Register("ShowTicksStyle", typeof(Style), typeof(TimerSlider), new PropertyMetadata(null, (sender, args) =>
            {
                TimerSlider timerslider = sender as TimerSlider;
                if (timerslider != null && args.NewValue != null)
                    timerslider.ButtonShowTicks.Style = (Style)args.NewValue;
            }));
        
        public Style ContentStyle
        {
            get { return (Style)GetValue(ContentStyleProperty); }
            set { SetValue(ContentStyleProperty, value); }
        }
        
        public static readonly DependencyProperty ContentStyleProperty =
            DependencyProperty.Register("ContentStyle", typeof(Style), typeof(TimerSlider), new PropertyMetadata(null));

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(TimerSlider), new PropertyMetadata(23));

        public TimeType CurrentType
        {
            get { return (TimeType)GetValue(CurrentTypeProperty); }
            set { SetValue(CurrentTypeProperty, value); }
        }

        public static readonly DependencyProperty CurrentTypeProperty =
            DependencyProperty.Register("CurrentType", typeof(TimeType), typeof(TimerSlider), new PropertyMetadata(TimeType.HOUR));

        private bool HasValueChanged = false;
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(TimerSlider), new PropertyMetadata(0, (sender, args) =>
            {              
            }));

        public Visibility TicksListVisibility
        {
            get { return (Visibility)GetValue(TicksListVisibilityProperty); }
            set { SetValue(TicksListVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TicksListVisibilityProperty =
            DependencyProperty.Register("TicksListVisibility", typeof(Visibility), typeof(TimerSlider), new PropertyMetadata(Visibility.Collapsed, (sender, args) =>
            {
                TimerSlider timerslider = sender as TimerSlider;
                if (timerslider != null)
                {
                    VisualStateManager.GoToState(timerslider, (Visibility)args.NewValue == Visibility.Collapsed ? "CollapsedListTicks" : "ShowListTicks", false);
                    if (timerslider.HasValueChanged)
                    {
                        timerslider.ButtonShowTicks.Content = timerslider.Value.ToString().PadLeft(2, '0');
                    }
                }
            }));

        public TimerSlider()
        {
            this.DefaultStyleKey = typeof(TimerSlider);

            this.Unloaded += TimerSlider_Unloaded;
        }

        void TimerSlider_Unloaded(object sender, RoutedEventArgs e)
        {
            ScrollViewerTicks.ViewChanged -= ScrollViewerTicks_ViewChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ButtonShowTicks = GetTemplateChild("ButtonShowTicks") as Button;
            ScrollViewerTicks = GetTemplateChild("ScrollViewerTicks") as ScrollViewer;
            StackPanelTicks = GetTemplateChild("StackPanelTicks") as StackPanel;

            InitializeTimerSlider();
            InitializeShowButtonContent();

            ScrollViewerTicks.ViewChanged += ScrollViewerTicks_ViewChanged;
            ButtonShowTicks.Click += ButtonShowTicks_Click;
            this.TimeVerticalOffSet = (Math.Round(this.StackPanelTicks.Children.Count / 2.0) * 80) - 80;
            VisualStateManager.GoToState(this, "CollapsedListTicks", false);

            if (this.Foreground != null)
                this.ButtonShowTicks.Foreground = this.Foreground;
        }
        
        void ButtonShowTicks_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "ShowListTicks", false);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, object e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;
            timer.Tick -= timer_Tick;
            timer.Stop();

            ScrollToMiddle();
        }

        private void InitializeTimerSlider()
        {
            switch (CurrentType)
            {
                case TimeType.HOUR:
                    currentValue = DateTime.Now.Hour;
                    break;
                case TimeType.MINUTE:
                    currentValue = DateTime.Now.Minute;
                    break;
                default:
                    currentValue = DateTime.Now.Hour;
                    break;
            }

            InitializeDataSource(MaxValue + 1);
        }

        private void InitializeShowButtonContent()
        {
            switch (CurrentType)
            {
                case TimeType.HOUR:
                    ButtonShowTicks.Content = DateTime.Now.Hour.ToString().PadLeft(2, '0');
                    break;
                case TimeType.MINUTE:
                    ButtonShowTicks.Content = DateTime.Now.Minute.ToString().PadLeft(2, '0');
                    break;
                default:
                    break;
            }
        }

        private void InitializeDataSource(int count)
        {
            int reallyCount = count / 2 - 1;
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < count; i++)
                {
                    AddObj(i.ToString().PadLeft(2, '0'));
                }
            }
        }

        private bool isScrollCompleteProcessing = false;

        void ScrollViewerTicks_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (this.isScrollCompleteProcessing)
            {
                return;
            }

            if (!e.IsIntermediate)
            {
                ScrollViewer scrollViewer = sender as ScrollViewer;
                double offSet = scrollViewer.VerticalOffset % (Global.TimeContentHeight);
                if (!isScrolling)
                {
                    Storyboard storyboard = new Storyboard();
                    DoubleAnimation animation = new DoubleAnimation();
                    animation.EnableDependentAnimation = true;
                    animation.Duration = TimeSpan.FromSeconds(0.01);
                    var ver = scrollViewer.VerticalOffset;
                    if (ver < (Global.TimeContentHeight) * (this.MaxValue + 1))
                    {
                        ver = (scrollViewer.VerticalOffset + (Global.TimeContentHeight) * (this.MaxValue + 1) * 2);
                    }
                    if (ver > (Global.TimeContentHeight) * (this.MaxValue + 1) * 4)
                    {
                        ver = (scrollViewer.VerticalOffset - (Global.TimeContentHeight) * (this.MaxValue + 1) * 2);
                    }

                    animation.From = ver;
                    animation.To = ver;
                    myoffset = ver;
                    storyboard.Children.Add(animation);
                    Storyboard.SetTarget(animation, this);
                    Storyboard.SetTargetProperty(animation, "(TimerSlider.TimeVerticalOffSet)");
                    storyboard.Begin();
                }

                this.isScrollCompleteProcessing = false;

                if (offSet != 0 && !this.isScrolling)
                {
                    this.isScrolling = true;
                    Storyboard storyboard = new Storyboard();
                    DoubleAnimation animation = new DoubleAnimation();
                    animation.EnableDependentAnimation = true;
                    animation.Duration = TimeSpan.FromSeconds(0.19);
                    var ver = scrollViewer.VerticalOffset;
                    if (ver < (Global.TimeContentHeight) * (this.MaxValue + 1))
                    {
                        ver = (scrollViewer.VerticalOffset + (Global.TimeContentHeight) * (this.MaxValue + 1) * 2);
                    }
                    if (ver > (Global.TimeContentHeight) * (this.MaxValue + 1) * 4)
                    {
                        ver = (scrollViewer.VerticalOffset - (Global.TimeContentHeight) * (this.MaxValue + 1) * 2);
                    }

                    animation.From = ver;

                    if (offSet > Global.TimeContentHeight / 2)
                    {
                        myoffset = Math.Round(ver + (Global.TimeContentHeight) - offSet, 0);
                        animation.To = Math.Round(ver + (Global.TimeContentHeight) - offSet, 0);

                    }
                    else
                    {
                        myoffset = Math.Round(ver - offSet, 0);
                        animation.To = Math.Round(ver - offSet, 0);
                    }
                    storyboard.Children.Add(animation);
                    Storyboard.SetTarget(animation, this);
                    Storyboard.SetTargetProperty(animation, "(TimerSlider.TimeVerticalOffSet)");

                    storyboard.Begin();
                }
            }
            else
            {
                this.isScrolling = false;
            }
        }

        /// <summary>
        /// Gets or sets the scroll viewer vertical offset
        /// </summary>
        internal double TimeVerticalOffSet
        {
            get { return (double)this.GetValue(TimeVerticalOffSetProperty); }
            set { this.SetValue(TimeVerticalOffSetProperty, value); }
        }

        /// <summary>
        /// Occurs when Time Vertical Offset property is set
        /// </summary>
        /// <param name="sender">The source</param>
        /// <param name="args">The property changed event argument</param>
        internal static void OnTimeVerticalOffSetPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue != args.OldValue)
            {
                TimerSlider thisInstance = sender as TimerSlider;
                thisInstance.ScrollToVerticalOffset((double)args.NewValue);
            }
        }

        /// <summary>
        /// Scroll viewer vertical offset property
        /// </summary>
        internal static readonly DependencyProperty TimeVerticalOffSetProperty =
               DependencyProperty.Register("TimeVerticalOffSet", typeof(double), typeof(TimerSlider), new PropertyMetadata(0.0, OnTimeVerticalOffSetPropertyChanged));
       
        /// <summary>
        /// Scroll the scroll viewer to specified offset
        /// </summary>
        /// <param name="scrollOffSet">The scroll offset</param>
        internal void ScrollToVerticalOffset(double scrollOffSet)
        {
            if (this.ScrollViewerTicks != null)
            {
                this.ScrollViewerTicks.ScrollToVerticalOffset(scrollOffSet);
                myoffset = scrollOffSet;
            }
        }

        /// <summary>
        /// Whether scroll viewer is scrolling
        /// </summary>
        private bool isScrolling = false;
        
        private void ScrollToMiddle()
        {
            Scroll((Global.TimeContentHeight) * (currentValue + (this.MaxValue + 1) * 2));
        }

        private void Scroll(double offset)
        {
            ScrollViewerTicks.ScrollToVerticalOffset(offset);

            myoffset = offset;
        }

        private void AddObj(string content)
        {
            StackPanelTicks.Children.Add(DateTimeHelper.GetButton(content, ContentStyle, ContentButtonClick));
        }
        
        void ContentButtonClick(object sender, RoutedEventArgs args)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                ButtonShowTicks.Content = btn.Content;
                currentValue = ConvertToInt32(btn.Content);

                this.Value = ConvertToInt32(btn.Content);

                myoffset = (Global.TimeContentHeight) * ((this.MaxValue + 1) * 2 + currentValue);
                ScrollViewerTicks.ScrollToVerticalOffset(myoffset);

                switch (CurrentType)
                {
                    case TimeType.HOUR:
                        OnDateTimeChanged(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, currentValue, DateTime.Now.Minute, DateTime.Now.Second));
                        break;
                    case TimeType.MINUTE:
                        OnDateTimeChanged(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, currentValue, DateTime.Now.Second));
                        break;
                    default:
                        OnDateTimeChanged(DateTime.Now);
                        break;
                }

                VisualStateManager.GoToState(this, "CollapsedListTicks", false);
            }
            this.TicksListVisibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private int ConvertToInt32(object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (InvalidCastException)
            {
                return 0;
            }
        }

        internal void SetTime()
        {
            double offSet =  myoffset % (Global.TimeContentHeight);
            double verValue = 0;
            if (offSet > Global.TimeContentHeight / 2)
            {
                verValue = Math.Round(myoffset + (Global.TimeContentHeight) - offSet, 0);
            }
            else
            {
                verValue = Math.Round(myoffset - offSet, 0);
            }
            var selectvalue = verValue / (Global.TimeContentHeight) % (this.MaxValue + 1);
            if (double.IsNaN(selectvalue) || double.IsInfinity(selectvalue)) return;
            this.currentValue = ConvertToInt32(selectvalue);
            if (currentValue >= 0 && currentValue<=this.MaxValue)
            {
                ButtonShowTicks.Content = selectvalue.ToString().PadLeft(2, '0'); 
                this.Value = ConvertToInt32(selectvalue);
                switch (CurrentType)
                {
                    case TimeType.HOUR:
                        OnDateTimeChanged(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, this.currentValue, DateTime.Now.Minute, DateTime.Now.Second));
                        break;
                    case TimeType.MINUTE:
                        OnDateTimeChanged(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, this.currentValue, DateTime.Now.Second));
                        break;
                    default:
                        OnDateTimeChanged(DateTime.Now);
                        break;
                }
            }
        }
    }
}
