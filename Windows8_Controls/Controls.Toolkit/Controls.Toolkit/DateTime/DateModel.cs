using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Controls.Toolkit
{
    public abstract class Date : INotifyPropertyChanged
    {
        private DateTime _date;
        public DateTime SelectedDate
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("SelectedDate");
            }
        }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set { _currentDate = value; }
        }

        private DateTime _defaultDate;
        public DateTime DefaultDate
        {
            get { return _defaultDate; }
            set { _defaultDate = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyname)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }

    public class DaySelectArgs : EventArgs
    {
        public Day day { get; set; }
    }

    public class MonthSelectArgs : EventArgs
    {
        public Month Month { get; set; }
    }

    public class Day : Date
    {
        public event EventHandler<DaySelectArgs> Selected;
        private Brush _dayForeground;
        public Brush DayForeground
        {
            get { return _dayForeground; }
            set { _dayForeground = value; }
        }

        private bool _isOtherMonth;
        public bool IsOtherMonth
        {
            get { return _isOtherMonth; }
            set { _isOtherMonth = value; }
        }

        private Style _dayStyle;
        public Style DayStyle
        {
            get { return _dayStyle; }
            set { _dayStyle = value; }
        }

        private bool _isInSelectedAero;
        public bool IsInSelectedAero
        {
            get { return _isInSelectedAero; }
            set { _isInSelectedAero = value; }
        }

        public void Call()
        {
            if (this.Selected != null)
            {
                this.Selected(this, new DaySelectArgs() { day = this });
            }
        }
    }

    public class Month : Date
    {
        private ObservableCollection<Day> _days;
        public ObservableCollection<Day> Days
        {
            get { return _days; }
            set { _days = value; }
        }

        public ObservableCollection<Day> GetDaysByMonth(int month)
        {
            return Days.Where(months => months.CurrentDate.Month == month).ToabservableCollection<Day>();
        }

        public event EventHandler<MonthSelectArgs> MonthSelected;
        public void OnMOnthSelected()
        {
            if (this.MonthSelected != null)
                this.MonthSelected(this, new MonthSelectArgs() { Month = this });
        }
    }

    public class Year : Date
    {
        private ObservableCollection<Month> _months;
        public ObservableCollection<Month> Months
        {
            get { return _months; }
            set { _months = value; }
        }

        public ObservableCollection<Month> GetMonthsByYear(int year)
        {
            return Months.Where(years => years.CurrentDate.Year == year).ToabservableCollection<Month>();
        }
    }
}
