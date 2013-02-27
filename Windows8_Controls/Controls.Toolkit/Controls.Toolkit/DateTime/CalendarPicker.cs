using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace Controls.Toolkit
{
    [TemplatePart(Name = "LayoutRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "header", Type = typeof(CalendarHeader))]
    [TemplatePart(Name = "yearheader", Type = typeof(CalendarYearHeadar))]
    [TemplatePart(Name = "semanticzoomDate", Type = typeof(SemanticZoom))]
    [TemplatePart(Name = "gridviewDays", Type = typeof(GridView))]
    [TemplatePart(Name = "gridviewMonths", Type = typeof(GridView))]
    public sealed class CalendarPicker : Control
    {
        private Grid LayoutRoot;
        private GridView gridviewDays;
        private GridView gridviewMonths;
        private CalendarHeader header;
        private CalendarYearHeadar yearheader;
        private SemanticZoom semanticzoomDate;

        private Day lastDay = null;
        private Month lastMonth = null;
        private bool isDefaultStyle = true;
        private Button lastTempButton = null;
        private DateTime time = default(DateTime);

        void gridviewDays_ItemClick(object sender, ItemClickEventArgs e)
        {
            object obj = sender;
            object obje = e.ClickedItem;
        }

        public CalendarPicker()
        {
            this.DefaultStyleKey = typeof(CalendarPicker);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            LayoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            header = GetTemplateChild("header") as CalendarHeader;
            yearheader = GetTemplateChild("yearheader") as CalendarYearHeadar;
            semanticzoomDate = GetTemplateChild("semanticzoomDate") as SemanticZoom;
            gridviewDays = GetTemplateChild("gridviewDays") as GridView;
            gridviewMonths = GetTemplateChild("gridviewMonths") as GridView;

            Days = new ObservableCollection<Day>();
            Months = new ObservableCollection<Month>();
            Years = new ObservableCollection<Year>();

            this.Loaded += Calendar_Loaded;
            this.header.LeftButtonClick += header_LeftButtonClick;
            this.header.RightButtonClick += header_RightButtonClick;

            this.yearheader.LeftButtonClick += yearheader_LeftButtonClick;
            this.yearheader.RightButtonClick += yearheader_RightButtonClick;

            this.gridviewDays.ItemClick += gridviewDays_ItemClick;

            this.semanticzoomDate.ViewChangeStarted += semanticzoomDate_ViewChangeStarted;
        }

        void semanticzoomDate_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            SwitchState();
        }

        void yearheader_RightButtonClick(object sender, RoutedEventArgs e)
        {
            GoToNextDateTime_Click(sender, e);
            this.yearheader.YearTitle = this.YearTitle.ToString();
            this.YearTitle = int.Parse(this.yearheader.YearTitle);
        }

        void yearheader_LeftButtonClick(object sender, RoutedEventArgs e)
        {
            GoToPreviousDateTime_Click(sender, e);
            this.yearheader.YearTitle = this.YearTitle.ToString();
            this.YearTitle = int.Parse(this.yearheader.YearTitle);
        }

        void header_RightButtonClick(object sender, RoutedEventArgs e)
        {
            GoToNextDateTime_Click(sender, e);
            this.header.MonthTitle = this.MonthTitle.ToString();
            this.MonthTitle = int.Parse(this.header.MonthTitle);
        }

        void header_LeftButtonClick(object sender, RoutedEventArgs e)
        {
            GoToPreviousDateTime_Click(sender, e);
            this.header.MonthTitle = this.MonthTitle.ToString();
            this.YearTitle = int.Parse(this.header.MonthTitle);
        }

        #region today
        public DateTime TodayOfDate
        {
            get { return (DateTime)GetValue(TodayOfDateProperty); }
            set { SetValue(TodayOfDateProperty, value); }
        }

        public static readonly DependencyProperty TodayOfDateProperty =
            DependencyProperty.Register("TodayOfDate", typeof(DateTime), typeof(CalendarPicker), new PropertyMetadata(DateTime.Now));
        #endregion

        #region selected

        #region startdate
        
        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime), typeof(CalendarPicker), new PropertyMetadata(DateTime.MaxValue));

        #endregion

        #region enddate
        
        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime), typeof(CalendarPicker), new PropertyMetadata(DateTime.MinValue, (sender, args) =>
            {
                CalendarPicker calendar = sender as CalendarPicker;
                if (calendar != null)
                {
                    DateTime datetime = DateTime.Now;
                    try
                    {
                        datetime = Convert.ToDateTime(args.NewValue);
                        if (datetime == DateTime.MinValue)
                            datetime = DateTime.Now;
                    }
                    catch (Exception)
                    {
                        datetime = DateTime.Now;
                    }
                    finally
                    {
                        calendar.UpdateSelectedDays(datetime);
                    }
                }
            }));

        #endregion

        public bool ShowOtherMonth
        {
            get { return (bool)GetValue(ShowOtherMonthProperty); }
            set { SetValue(ShowOtherMonthProperty, value); }
        }

        public static readonly DependencyProperty ShowOtherMonthProperty =
            DependencyProperty.Register("ShowOtherMonth", typeof(bool), typeof(CalendarPicker), new PropertyMetadata(true));

        #endregion

        #region calendar style

        #region Title brush
        public Brush YearOnDaysTitleBrush
        {
            get { return (Brush)GetValue(YearOnDaysTitleBrushProperty); }
            set { SetValue(YearOnDaysTitleBrushProperty, value); }
        }

        public static readonly DependencyProperty YearOnDaysTitleBrushProperty =
            DependencyProperty.Register("YearOnDaysTitleBrush", typeof(Brush), typeof(CalendarPicker), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        
        public Brush MonthOnDaysTitleBrush
        {
            get { return (Brush)GetValue(MonthOnDaysTitleBrushProperty); }
            set { SetValue(MonthOnDaysTitleBrushProperty, value); }
        }

        public static readonly DependencyProperty MonthOnDaysTitleBrushProperty =
            DependencyProperty.Register("MonthOnDaysTitleBrush", typeof(Brush), typeof(CalendarPicker), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));
        
        public Brush YearOnMonthsTitlBrush
        {
            get { return (Brush)GetValue(YearOnMonthsTitlBrushProperty); }
            set { SetValue(YearOnMonthsTitlBrushProperty, value); }
        }

        public static readonly DependencyProperty YearOnMonthsTitlBrushProperty =
            DependencyProperty.Register("YearOnMonthsTitlBrush", typeof(Brush), typeof(CalendarPicker), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));
        
        public Brush MonthOnMonthsTitleBrush
        {
            get { return (Brush)GetValue(MonthOnMonthsTitleBrushProperty); }
            set { SetValue(MonthOnMonthsTitleBrushProperty, value); }
        }

        public static readonly DependencyProperty MonthOnMonthsTitleBrushProperty =
            DependencyProperty.Register("MonthOnMonthsTitleBrush", typeof(Brush), typeof(CalendarPicker), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        #endregion

        #region previous and next button style

        public Style PreviousButtonStyle
        {
            get { return (Style)GetValue(PreviousButtonStyleProperty); }
            set { SetValue(PreviousButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty PreviousButtonStyleProperty =
            DependencyProperty.Register("PreviousButtonStyle", typeof(Style), typeof(CalendarPicker), new PropertyMetadata(null));

        public Style NextButtonStyle
        {
            get { return (Style)GetValue(NextButtonStyleProperty); }
            set { SetValue(NextButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty NextButtonStyleProperty =
            DependencyProperty.Register("NextButtonStyle", typeof(Style), typeof(CalendarPicker), new PropertyMetadata(null));
        #endregion

        #region week brush

        public Brush WeekTitleBrush
        {
            get { return (Brush)GetValue(WeekTitleBrushProperty); }
            set { SetValue(WeekTitleBrushProperty, value); }
        }

        public static readonly DependencyProperty WeekTitleBrushProperty =
            DependencyProperty.Register("WeekTitleBrush", typeof(Brush), typeof(CalendarPicker), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        
        public Brush FistAndLastWeekBrush
        {
            get { return (Brush)GetValue(FistAndLastWeekBrushProperty); }
            set { SetValue(FistAndLastWeekBrushProperty, value); }
        }

        public static readonly DependencyProperty FistAndLastWeekBrushProperty =
            DependencyProperty.Register("FistAndLastWeekBrush", typeof(Brush), typeof(CalendarPicker), new PropertyMetadata(new SolidColorBrush(Colors.Red)));
        #endregion

        #region day forground

        public Brush DayForground
        {
            get { return (Brush)GetValue(DayForgroundProperty); }
            set { SetValue(DayForgroundProperty, value); }
        }

        public static readonly DependencyProperty DayForgroundProperty =
            DependencyProperty.Register("DayForground", typeof(Brush), typeof(CalendarPicker), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        
        public Brush FirstAndLastDayColumnForground
        {
            get { return (Brush)GetValue(FirstAndLastDayColumnForgroundProperty); }
            set { SetValue(FirstAndLastDayColumnForgroundProperty, value); }
        }

        public static readonly DependencyProperty FirstAndLastDayColumnForgroundProperty =
            DependencyProperty.Register("FirstAndLastDayColumnForground", typeof(Brush), typeof(CalendarPicker), new PropertyMetadata(new SolidColorBrush(Colors.Red)));
     
        #endregion

        #region month style

        public Style MonthStyle
        {
            get { return (Style)GetValue(MonthStyleProperty); }
            set { SetValue(MonthStyleProperty, value); }
        }

        public static readonly DependencyProperty MonthStyleProperty =
            DependencyProperty.Register("MonthStyle", typeof(Style), typeof(CalendarPicker), new PropertyMetadata(null));

        public Style SelectedMonthStyle
        {
            get { return (Style)GetValue(SelectedMonthStyleProperty); }
            set { SetValue(SelectedMonthStyleProperty, value); }
        }

        public static readonly DependencyProperty SelectedMonthStyleProperty =
            DependencyProperty.Register("SelectedMonthStyle", typeof(Style), typeof(CalendarPicker), new PropertyMetadata(null));
       
        #endregion

        public Style DaySelectedStyle
        {
            get { return (Style)GetValue(DaySelectedStyleProperty); }
            set { SetValue(DaySelectedStyleProperty, value); }
        }

        public static readonly DependencyProperty DaySelectedStyleProperty =
            DependencyProperty.Register("DaySelectedStyle", typeof(Style), typeof(CalendarPicker), new PropertyMetadata(null));

        public Style MonthSelectedStyle
        {
            get { return (Style)GetValue(MonthSelectedStyleProperty); }
            set { SetValue(MonthSelectedStyleProperty, value); }
        }

        public static readonly DependencyProperty MonthSelectedStyleProperty =
            DependencyProperty.Register("MonthSelectedStyle", typeof(Style), typeof(CalendarPicker), new PropertyMetadata(null));

        public Brush OtherMonthDayBrush
        {
            get { return (Brush)GetValue(OtherMonthDayBrushProperty); }
            set { SetValue(OtherMonthDayBrushProperty, value); }
        }

        public static readonly DependencyProperty OtherMonthDayBrushProperty =
            DependencyProperty.Register("OtherMonthDayBrush", typeof(Brush), typeof(CalendarPicker), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public Style DayStyle
        {
            get { return (Style)GetValue(DayStyleProperty); }
            set { SetValue(DayStyleProperty, value); }
        }

        public static readonly DependencyProperty DayStyleProperty =
            DependencyProperty.Register("DayStyle", typeof(Style), typeof(CalendarPicker), new PropertyMetadata(null));

        public Style SelectedAeroStyle
        {
            get { return (Style)GetValue(SelectedAeroStyleProperty); }
            set { SetValue(SelectedAeroStyleProperty, value); }
        }

        public static readonly DependencyProperty SelectedAeroStyleProperty =
            DependencyProperty.Register("SelectedAeroStyle", typeof(Style), typeof(CalendarPicker), new PropertyMetadata(null));

        public Style SelectedDayStyle
        {
            get { return (Style)GetValue(SelectedDayStyleProperty); }
            set { SetValue(SelectedDayStyleProperty, value); }
        }

        public static readonly DependencyProperty SelectedDayStyleProperty =
            DependencyProperty.Register("SelectedDayStyle", typeof(Style), typeof(CalendarPicker), new PropertyMetadata(null));
      
        #endregion

        #region layout
        
        public double CalendarWidth
        {
            get { return (double)GetValue(CalendarWidthProperty); }
            set { SetValue(CalendarWidthProperty, value); }
        }

        public static readonly DependencyProperty CalendarWidthProperty =
            DependencyProperty.Register("CalendarWidth", typeof(double), typeof(CalendarPicker), new PropertyMetadata(0));
        
        public double CalendarHeight
        {
            get { return (double)GetValue(CalendarHeightProperty); }
            set { SetValue(CalendarHeightProperty, value); }
        }

        public static readonly DependencyProperty CalendarHeightProperty =
            DependencyProperty.Register("CalendarHeight", typeof(double), typeof(CalendarPicker), new PropertyMetadata(0));

        #endregion

        #region data source
        private ObservableCollection<Day> days;
        public ObservableCollection<Day> Days
        {
            get { return days; }
            set { days = value; }
        }

        private ObservableCollection<Month> months;
        public ObservableCollection<Month> Months
        {
            get { return months; }
            set { months = value; }
        }

        private ObservableCollection<Year> years;
        public ObservableCollection<Year> Years
        {
            get { return years; }
            set { years = value; }
        }
        #endregion

        private DateTime _currentDateTime;
        public DateTime CurrentDateTime
        {
            get { return _currentDateTime; }
            set
            {
                _currentDateTime = value;
                if (_currentDateTime != null)
                    CreateDay(_currentDateTime);
                else
                    CreateDay(DateTime.Now);
            }
        }

        private DateTime _selectedDateTime;
        public DateTime SelectedDateTime
        {
            get { return _selectedDateTime; }
            set { _selectedDateTime = value; }
        }

        private int _monthTitle = DateTime.Now.Month;
        public int MonthTitle
        {
            get { return _monthTitle; }
            set
            {
                _monthTitle = value;
                this.OnPropertyChanged();
            }
        }

        private int _yearTitle = DateTime.Now.Year;
        public int YearTitle
        {
            get { return _yearTitle; }
            set
            {
                _yearTitle = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event DateTimeSelectedEventhandler DateTimeSelected;
        private void OnDateTimeSelected(DateTime datetime)
        {
            DateTimeSelectedEventhandler handler = DateTimeSelected;
            if (handler != null)
                handler(datetime);
        }

        public double HeaderWidth
        {
            get { return (double)GetValue(HeaderWidthProperty); }
            set { SetValue(HeaderWidthProperty, value); }
        }

        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register("HeaderWidth", typeof(double), typeof(CalendarPicker), new PropertyMetadata(400));

        void Calendar_Loaded(object sender, RoutedEventArgs e)
        {
            TodayOfDate = DateTime.Now;

            CreateDay(DateTime.Now);
            CreateMonth(DateTime.Now);

            SelectedDateTime = DateTime.Now;

            gridviewDays.ItemsSource = Days;
            gridviewMonths.ItemsSource = Months;

            SwitchState();
        }
                
        public void UpdateSelectedDays(DateTime datetime)
        {
            CreateDay(datetime);
        }
        public Day GeneralDay { get; set; }

        private ObservableCollection<Day> CreateDay(DateTime date)
        {
            days.Clear();

            time = date;

            DayOfWeek week = date.DayOfWeek;
            DateTime tempDateTime = new DateTime(date.Year, date.Month, 1);

            bool isinselected = false;
            if (ShowOtherMonth)
            {
                while (tempDateTime.DayOfWeek != DayOfWeek.Sunday)
                {
                    tempDateTime = tempDateTime.AddDays(-1);
                    GeneralSelectedAero(out isinselected, tempDateTime);
                    GeneralDay = new Day() { SelectedDate = tempDateTime, CurrentDate = date, DefaultDate = date, DayForeground = GetDayForeBrush(tempDateTime, isOtherMonth: true), IsOtherMonth = true, DayStyle = DayStyle, IsInSelectedAero = isinselected };
                    GeneralDay.Selected += daynew_Selected;
                    Days.Insert(0, GeneralDay);
                }
            }

            tempDateTime = new DateTime(date.Year, date.Month, 1);
            DateTime tempNextDateTime = DateTime.Now;
            if (date.Month != 12)
                tempNextDateTime = new DateTime(date.Year, date.Month + 1, 1);
            else
                tempNextDateTime = new DateTime(date.Year + 1, 1, 1);


            GeneralSelectedAero(out isinselected, tempDateTime);
            if (tempDateTime.Year == DateTime.Now.Year && tempDateTime.Month == DateTime.Now.Month)
            {
                GeneralDay = new Day() { CurrentDate = date, DefaultDate = date, SelectedDate = tempDateTime, DayForeground = GetDayForeBrush(tempDateTime), IsOtherMonth = false, DayStyle = DayStyle, IsInSelectedAero = isinselected };
                GeneralDay.Selected += daynew_Selected;
                Days.Add(GeneralDay);
            }
            else
            {
                GeneralDay = new Day() { CurrentDate = date, DefaultDate = date, SelectedDate = tempDateTime, DayForeground = GetDayForeBrush(tempDateTime), IsOtherMonth = false, DayStyle = GetDayForeBrush(), IsInSelectedAero = isinselected };
                GeneralDay.Selected += daynew_Selected;
                Days.Add(GeneralDay);
            }
            tempDateTime = tempDateTime.AddDays(1);

            while (tempDateTime < tempNextDateTime)
            {
                GeneralSelectedAero(out isinselected, tempDateTime);
                GeneralDay = new Day() { CurrentDate = date, DefaultDate = date, SelectedDate = tempDateTime, DayForeground = GetDayForeBrush(tempDateTime), IsOtherMonth = false, DayStyle = DayStyle, IsInSelectedAero = isinselected };
                GeneralDay.Selected += daynew_Selected;
                Days.Add(GeneralDay);
                tempDateTime = tempDateTime.AddDays(1);
            }

            if (ShowOtherMonth)
            {
                if (date.Month != 12)
                    tempNextDateTime = new DateTime(date.Year, date.Month + 1, 1);
                else
                    tempNextDateTime = new DateTime(date.Year + 1, 1, 1);

                while (tempNextDateTime.DayOfWeek != DayOfWeek.Saturday)
                {
                    GeneralSelectedAero(out isinselected, tempNextDateTime);
                    GeneralDay = new Day() { SelectedDate = tempNextDateTime, CurrentDate = date, DefaultDate = date, DayForeground = GetDayForeBrush(tempNextDateTime, isOtherMonth: true), IsInSelectedAero = isinselected, IsOtherMonth = true, DayStyle = DayStyle };
                    GeneralDay.Selected += daynew_Selected;
                    Days.Add(GeneralDay);
                    tempNextDateTime = tempNextDateTime.AddDays(1);
                }

                GeneralSelectedAero(out isinselected, tempNextDateTime);
                GeneralDay = new Day() { SelectedDate = tempNextDateTime, CurrentDate = date, DefaultDate = date, DayForeground = GetDayForeBrush(tempNextDateTime, true), IsOtherMonth = true, DayStyle = DayStyle, IsInSelectedAero = isinselected };
                GeneralDay.Selected += daynew_Selected;
                Days.Add(GeneralDay);
            }
            LayoutRoot.DataContext = this;

            return days;
        }

        private void daynew_Selected(object sender, DaySelectArgs e)
        {
            GeneralDay.Selected -= daynew_Selected;

            Day day = e.day;
            if (lastDay != null)
            {
                if (lastDay != day)
                {
                    for (int i = 0; i < Days.Count; i++)
                    {
                        if (lastDay.SelectedDate.Month == Days[i].SelectedDate.Month)
                            if (lastDay.SelectedDate.Day == Days[i].SelectedDate.Day)
                            {
                                lastDay = Days[i];
                                Days.RemoveAt(i);
                                Days.Insert(i, lastDay);
                            }
                    }
                }
            }
            lastDay = day;
            OnDaySelected(day);
        }

        private ObservableCollection<Month> CreateMonth(DateTime date)
        {
            months.Clear();

            int monthIndex = 1;
            while (monthIndex < 13)
            {
                Month month = new Month() { SelectedDate = new DateTime(date.Year, monthIndex, date.AddMonths(monthIndex - 1).Day), DefaultDate = date, CurrentDate = date, Days = days == null ? CreateDay(date) : days };
                month.MonthSelected += month_MonthSelected;
                Months.Add(month);

                monthIndex++;
            }
            return months;
        }
        
        private void month_MonthSelected(object sender, MonthSelectArgs e)
        {
            Month month = e.Month;
            if (lastMonth != null)
            {
                for (int i = 0; i < Months.Count; i++)
                {
                    if (lastMonth.SelectedDate.Month == Months[i].SelectedDate.Month)
                        if (lastMonth.SelectedDate.Day == Months[i].SelectedDate.Day)
                        {
                            lastMonth = Months[i];
                            Months.RemoveAt(i);
                            Months.Insert(i, lastMonth);
                        }
                }
            }
            lastMonth = month;
            DateTime dateTime = DateTime.Now;
            try
            {
                dateTime = lastMonth.SelectedDate;
                SelectedDateTime = new DateTime(YearTitle, dateTime.Month, dateTime.Day);
            }
            catch (Exception)
            {
                SelectedDateTime = DateTime.Now;
            }
            finally
            {
                UpdateDays(SelectedDateTime);
                semanticzoomDate.ToggleActiveView();
            }

            this.header.MonthTitle = SelectedDateTime.Month.ToString();
            this.header.YearTitle = SelectedDateTime.Year.ToString();
            this.MonthTitle = SelectedDateTime.Month;
            this.YearTitle = SelectedDateTime.Year;
        }

        private ObservableCollection<Day> GoToNextMonth(DateTime date)
        {
            return CreateDay(date.AddMonths(1));
        }

        private ObservableCollection<Day> GotoPreviousMonth(DateTime date)
        {
            return CreateDay(date.AddMonths(-1));
        }

        private void OnMonthChanged_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag != null)
            {
                DateTime dateTime = DateTime.Now;
                try
                {
                    dateTime = Convert.ToDateTime(btn.Tag);
                    SelectedDateTime = new DateTime(YearTitle, dateTime.Month, dateTime.Day);
                }
                catch (Exception)
                {
                    SelectedDateTime = DateTime.Now;
                }
                finally
                {
                    UpdateDays(SelectedDateTime);
                    semanticzoomDate.ToggleActiveView();
                }
            }
            if (SelectedMonthStyle != null)
                btn.Style = SelectedMonthStyle;
        }

        private void UpdateDays(DateTime date)
        {
            CreateDay(date);
        }

        private void UpdateMonths(DateTime date)
        {
            CreateMonth(date);
        }

        private Brush GetDayForeBrush(DateTime date, bool isOtherMonth = false)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
                return FirstAndLastDayColumnForground;
            else
                if (isOtherMonth)
                    return OtherMonthDayBrush;
                else
                    return DayForground;
        }

        private Style GetDayForeBrush()
        {
            if (DaySelectedStyle != null)
                return DaySelectedStyle;
            return null;
        }

        private void OnDataSelected_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Update(btn);
            if (btn != null && btn.Tag != null)
            {
                DateTime datetime = DateTime.Now;
                try
                {
                    datetime = Convert.ToDateTime(btn.Tag);

                    Day day = btn.DataContext as Day;
                    if (day.IsOtherMonth)
                    {
                        SelectedDateTime = datetime;
                        UpdateDays(datetime);
                    }
                }
                catch (Exception)
                {
                    datetime = DateTime.Now;
                }
                finally
                {
                    OnDateTimeSelected(datetime);
                    SelectedDateTime = datetime;
                }
            }
            if (DaySelectedStyle != null)
                btn.Style = DaySelectedStyle;
        }

        private void OnDaySelected(Day day)
        {
            SelectedDateTime = day.SelectedDate;
            OnDateTimeSelected(SelectedDateTime);

            if (day.IsOtherMonth)
            {
                UpdateDays(SelectedDateTime);
                this.header.MonthTitle = day.SelectedDate.Month.ToString();
                this.header.YearTitle = day.SelectedDate.Year.ToString();
                this.MonthTitle = SelectedDateTime.Month;
                this.YearTitle = SelectedDateTime.Year;
            }
        }

        private void Update(Button btn)
        {
            if (lastTempButton != null)
            {
                Day day = lastTempButton.DataContext as Day;
                if (day != null)
                {
                    if (lastTempButton != null && !day.IsInSelectedAero)
                        lastTempButton.Style = null;
                    else if (lastTempButton != null && day.IsInSelectedAero)
                        lastTempButton.Style = SelectedAeroStyle; ;
                }
            }
            lastTempButton = btn;
        }

        private void OnMonthTitle_Click(object sender, RoutedEventArgs e)
        {
            semanticzoomDate.ToggleActiveView();
        }

        private void GoToPreviousDateTime_Click(object sender, RoutedEventArgs e)
        {
            if (semanticzoomDate.IsZoomedInViewActive)
            {
                if (MonthTitle == 1)
                    MonthTitle = 13;
                MonthTitle--;

                DateTime datetime = DateTime.Now;
                if (MonthTitle == 12)
                {
                    datetime = new DateTime(SelectedDateTime.Year - 1, 12, datetime.Day);
                    YearTitle--;
                }
                else
                {
                    if (SelectedDateTime.Month == 1)
                        SelectedDateTime = new DateTime(SelectedDateTime.Year, 12, SelectedDateTime.Day, SelectedDateTime.Hour, SelectedDateTime.Minute, SelectedDateTime.Second);
                    int count = SelectedDateTime.Month - MonthTitle;
                    datetime = new DateTime(SelectedDateTime.Year, MonthTitle, SelectedDateTime.AddMonths(-count).Day);
                }
                UpdateDays(datetime);
            }
            else
            {
                YearTitle--;

                CurrentDateTime = new DateTime(SelectedDateTime.Year - 1, MonthTitle, SelectedDateTime.AddMonths(MonthTitle).Day);
            }

        }

        private void GoToNextDateTime_Click(object sender, RoutedEventArgs e)
        {
            if (semanticzoomDate.IsZoomedInViewActive)
            {
                if (MonthTitle == 12)
                    MonthTitle = 0;
                MonthTitle++;

                DateTime datetime = DateTime.Now;
                if (MonthTitle == 1)
                {
                    datetime = new DateTime(SelectedDateTime.Year + 1, 1, datetime.Day);
                    YearTitle++;
                }
                else
                {
                    if (SelectedDateTime.Month == 12)
                        SelectedDateTime = new DateTime(SelectedDateTime.Year, 1, SelectedDateTime.Day, SelectedDateTime.Hour, SelectedDateTime.Minute, SelectedDateTime.Second);
                    int count = SelectedDateTime.Month - MonthTitle;
                    datetime = new DateTime(SelectedDateTime.Year, MonthTitle, SelectedDateTime.AddMonths(-count).Day);
                }


                UpdateDays(datetime);
            }
            else
            {
                YearTitle++;

                CurrentDateTime = new DateTime(SelectedDateTime.Year + 1, MonthTitle, SelectedDateTime.AddMonths(MonthTitle).Day);
            }
        }

        private void GeneralSelectedAero(out bool isinselectedaero, DateTime datetime = default(DateTime))
        {
            if (DayStyle != null && !isDefaultStyle)
            {
                isDefaultStyle = false;
                isinselectedaero = false;
            }
            else
            {
                if (datetime.Year == DateTime.Now.Year && datetime.Month == DateTime.Now.Month && datetime.Day == DateTime.Now.Day)
                {
                    if (SelectedDayStyle != null)
                        DayStyle = SelectedDayStyle;
                    isinselectedaero = true;
                }
                else if (SelectedDateTime == datetime)
                {
                    if (SelectedDayStyle != null)
                        DayStyle = SelectedDayStyle;
                    isinselectedaero = false;
                }
                else
                {
                    DayStyle = null;
                    isinselectedaero = false;
                }

                if (StartDate != DateTime.MaxValue && EndDate != DateTime.MinValue && datetime >= StartDate && datetime <= EndDate)
                {
                    if (SelectedAeroStyle != null)
                        DayStyle = SelectedAeroStyle;
                    isinselectedaero = true;
                }
            }
        }

        private void MonthHeader_Taped(object sender, TappedRoutedEventArgs e)
        {
            semanticzoomDate.ToggleActiveView();
        }

        private void SwitchState()
        {
            if (semanticzoomDate.IsZoomedInViewActive)
                VisualStateManager.GoToState(this, "ShowDaysState", false);
            else
                VisualStateManager.GoToState(this, "ShowMonthsState", false);
        }
    }
}
