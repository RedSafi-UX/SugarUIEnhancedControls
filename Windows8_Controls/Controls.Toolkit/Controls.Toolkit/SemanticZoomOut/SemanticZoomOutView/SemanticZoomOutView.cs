using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Controls.Toolkit
{
    public class SemanticZoomOutView : GridView, ISemanticZoomOutView
    {
        #region
        private int _MaxGroupItemsCount = 0;
        public int MaxGroupItemsCount
        {
            get
            {
                return _MaxGroupItemsCount;
            }
            set
            {
                if (value > _MaxGroupItemsCount)
                {
                    _MaxGroupItemsCount = value;
                }
            }
        }
        #endregion

        public SemanticZoomOutView()
        {
           this.DefaultStyleKey = typeof(SemanticZoomOutView);
        }
    }
}
