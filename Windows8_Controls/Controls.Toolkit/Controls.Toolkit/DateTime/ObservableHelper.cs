using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.Toolkit
{
   public static  class ObservableHelper
    {
       public static ObservableCollection<T> ToabservableCollection<T>(this IEnumerable<T> enumerable)
       {
           ObservableCollection<T> transferTarget=null;
           if (enumerable != null)
           {
               transferTarget = new ObservableCollection<T>();
               foreach (var item in enumerable)
               {
                   transferTarget.Add(item);
               }
           }
           return transferTarget;
       }
    }
}
