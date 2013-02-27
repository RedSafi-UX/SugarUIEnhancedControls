using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Controls.Toolkit
{
    public abstract class SemanticZoomOutItemBase : Control, ISemanticZoomOutItem
    {
        private static int _MaxItemsCount;
        public static int MaxItemsCount
        {
            get
            {
                return _MaxItemsCount;
            }
            set
            {
                if (value > _MaxItemsCount)
                {
                    _MaxItemsCount = value;
                }
            }
        }

        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SemanticZoomOutItemBase), new PropertyMetadata(string.Empty));
        #endregion


        #region Count
        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set { SetValue(CountProperty, value); }
        }

        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(int), typeof(SemanticZoomOutItemBase), new PropertyMetadata(0, OnCountPropertyChanged));

        public static void OnCountPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender != null)
            {
                SemanticZoomOutItemBase currentControl = sender as SemanticZoomOutItemBase;
                currentControl.ItemsCount = args.NewValue.ToString();

                if (currentControl.ControlsOwner != null)
                {
                    currentControl.ControlsOwner.MaxGroupItemsCount = (int)args.NewValue;
                }
            }
        }
        #endregion


        #region ItemsCount
        public string ItemsCount
        {
            get { return (string)GetValue(ItemsCountProperty); }
            private set { SetValue(ItemsCountProperty, value); }
        }

        public static readonly DependencyProperty ItemsCountProperty =
            DependencyProperty.Register("ItemsCount", typeof(string), typeof(SemanticZoomOutItemBase), new PropertyMetadata(string.Empty));
        #endregion


        internal SemanticZoomOutView ControlsOwner = null;

        public SemanticZoomOutItemBase()
        {
            this.Loaded += SemanticZoomOutItemBase_Loaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.GetParenteControl();

            if (ControlsOwner != null)
            {
                ControlsOwner.MaxGroupItemsCount = this.Count;
            }
        }

        private void GetParenteControl()
        {
            SemanticZoomOutView ownerControl = null;

            this.GetControls(VisualTreeHelper.GetParent(this), ref ownerControl);

            ControlsOwner = ownerControl;
        }

        private void GetControls(DependencyObject parent, ref SemanticZoomOutView ownerControl)
        {
            if (parent is ISemanticZoomOutView)
            {
                ownerControl = parent as SemanticZoomOutView;
            }
            else if (ownerControl == null)
            {
                this.GetControls(VisualTreeHelper.GetParent(parent as FrameworkElement), ref ownerControl);
            }
        }

        void SemanticZoomOutItemBase_Loaded(object sender, RoutedEventArgs e)
        {
            this.OnDrawEdgePanel();
        }

        public virtual void OnDrawEdgePanel()
        {

        }

    }
}
