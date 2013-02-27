using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Controls.Toolkit
{
    public class MonthConverter : IValueConverter
    {
        Dictionary<int, string> dicMonths = new Dictionary<int, string>() {
        { 1, "January" }, 
        { 2, "February" }, 
        { 3, "March" },
        { 4, "April" },
        { 5, "May" },
        { 6, "June" },
        { 7, "July" },
        { 8, "August" },
        { 9, "September" },
        { 10, "October" }, 
        { 11, "November" },
        { 12, "December" } };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null)
                return Convert(value);
            else
                return Convert(value, true);
        }

        private object Convert(object obj, bool isMonthShot = false)
        {
            try
            {
                int index = System.Convert.ToInt32(obj);
                if (dicMonths.ContainsKey(index))
                    return !isMonthShot ? dicMonths[index] : dicMonths[index].Substring(0, 3);
                else
                {
                    if (index == 0)
                        return !isMonthShot ? dicMonths[dicMonths.Count - 1] : dicMonths[dicMonths.Count - 1].Substring(0, 3);
                    else if (index == 13)
                        return !isMonthShot ? dicMonths[1] : dicMonths[1].Substring(0, 3);
                    else
                        throw new InvalidOperationException("Error:The value should be int32!");
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Error:The value should be int32!");
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
