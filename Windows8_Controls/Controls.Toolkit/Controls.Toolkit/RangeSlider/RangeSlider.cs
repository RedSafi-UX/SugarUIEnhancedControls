using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;


namespace Controls.Toolkit
{
    [TemplatePart(Name = HoriontalTemplate, Type = typeof(Grid))]
    [TemplatePart(Name = VerticalTemplate, Type = typeof(Grid))]
    [TemplatePart(Name = thumbVerticalFrom, Type = typeof(Thumb))]
    [TemplatePart(Name = thumbVerticalTo, Type = typeof(Thumb))]
    [TemplatePart(Name = thumbHoriontalTo, Type = typeof(Thumb))]
    [TemplatePart(Name = thumbHoriontalFrom, Type = typeof(Thumb))]
    [TemplatePart(Name = popLeftHoriontal, Type = typeof(Grid))]
    [TemplatePart(Name = popRightHoriontal, Type = typeof(Grid))]
    [TemplatePart(Name = popRightVertical, Type = typeof(Grid))]
    [TemplatePart(Name = popLeftVertical, Type = typeof(Grid))]
    [TemplatePart(Name = gridRightHoriontal, Type = typeof(Grid))]
    [TemplatePart(Name = gridLeftVertical, Type = typeof(Grid))]
    [TemplatePart(Name = gridRightVertical, Type = typeof(Grid))]
    [TemplatePart(Name = gridLeftHoriontal, Type = typeof(Grid))]
    [TemplatePart(Name = rectMiddleHoriontal, Type = typeof(Rectangle))]
    [TemplatePart(Name = rectMiddleVertical, Type = typeof(Rectangle))]

    public sealed class RangeSlider : Control
    {
        #region
        private const string HoriontalTemplate = "HoriontalTemplate";
        private const string VerticalTemplate = "VerticalTemplate";
        private const string thumbVerticalFrom = "thumbVerticalFrom";
        private const string thumbVerticalTo = "thumbVerticalTo";
        private const string thumbHoriontalFrom = "thumbHoriontalFrom";
        private const string thumbHoriontalTo = "thumbHoriontalTo";
        private const string popLeftHoriontal = "popLeftHoriontal";
        private const string rectMiddleHoriontal = "rectMiddleHoriontal";
        private const string popRightHoriontal = "popRightHoriontal";
        private const string popLeftVertical = "popLeftVertical";
        private const string rectMiddleVertical = "rectMiddleVertical";
        private const string popRightVertical = "popRightVertical";
        private const string gridRightHoriontal = "gridRightHoriontal";
        private const string gridLeftHoriontal = "gridLeftHoriontal";
        private const string gridLeftVertical = "gridLeftVertical";
        private const string gridRightVertical = "gridRightVertical";
        private Thumb thumbFrom;
        private Thumb thumbTo;
        private Point FoucsPoint;
        private double perValue;
        private double totalValue;
        private double fromLength, toLength, totalLength;
        #endregion

        #region IsSingleSlider
        public Boolean IsSingleSlider
        {
            get { return (Boolean)GetValue(IsSingleSliderProperty); }
            set { SetValue(IsSingleSliderProperty, value); }
        }

        public static readonly DependencyProperty IsSingleSliderProperty =
            DependencyProperty.Register("IsSingleSlider", typeof(Boolean), typeof(RangeSlider), new PropertyMetadata(false, (s, e) =>
            {
                RangeSlider rangeSlider = s as RangeSlider;
                rangeSlider.FromThumbVisbilty = Convert.ToBoolean(e.NewValue) == true ? Visibility.Collapsed : Visibility.Visible;
            }));
        #endregion

        #region FromThumbVisbilty
        public Visibility FromThumbVisbilty
        {
            get { return (Visibility)GetValue(FromThumbVisbiltyProperty); }
           internal set { SetValue(FromThumbVisbiltyProperty, value); }
        }

        public static readonly DependencyProperty FromThumbVisbiltyProperty =
            DependencyProperty.Register("FromThumbVisbilty", typeof(Visibility), typeof(RangeSlider), new PropertyMetadata(Visibility.Visible));
        #endregion

        #region FromValue
        public double FromValue
        {
            get { return (double)GetValue(FromValueProperty); }
            set { SetValue(FromValueProperty, value); }
        }

        public static readonly DependencyProperty FromValueProperty =
            DependencyProperty.Register("FromValue", typeof(double), typeof(RangeSlider), new PropertyMetadata(0.0, (s, e) =>
            {
                RangeSlider rangeSlider = s as RangeSlider;
                rangeSlider.MinValue = e.NewValue.ToString();
                if (Convert.ToDouble(e.NewValue) <= 0) rangeSlider.FromValue = 0;
                if (Convert.ToDouble(e.NewValue) >=rangeSlider.Value) rangeSlider.FromValue = rangeSlider.Value-1;
                if (rangeSlider.FromThumbVisbilty == Visibility.Collapsed)
                {
                    rangeSlider.RangeFrom = 0;
                }
                else
                {
                    rangeSlider.RangeFrom = (rangeSlider.totalValue / (rangeSlider.Maximum - rangeSlider.Minimum)) * (rangeSlider.FromValue - rangeSlider.Minimum) + rangeSlider.fromLength;
                }
                rangeSlider.OnFromValueChanged(e);
            }));

        #endregion

        #region RangeFrom
        public double RangeFrom
        {
            get { return (double)GetValue(RangeFromProperty); }
            internal set { SetValue(RangeFromProperty, value); }
        }

        public static readonly DependencyProperty RangeFromProperty =
            DependencyProperty.Register("RangeFrom", typeof(double), typeof(RangeSlider), new PropertyMetadata(0.0));

        #endregion

        #region RangeTo
        public double RangeTo
        {
            get { return (double)GetValue(RangeToProperty); }
            internal set { SetValue(RangeToProperty, value); }
        }

        public static readonly DependencyProperty RangeToProperty =
            DependencyProperty.Register("RangeTo", typeof(double), typeof(RangeSlider), new PropertyMetadata(0.0));

        #endregion

        #region Orientation
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(RangeSlider), new PropertyMetadata(Orientation.Horizontal, (s, e) =>
            {
                RangeSlider rangeSlider = s as RangeSlider;
                if (rangeSlider != null)
                    rangeSlider.InitRangeSlider();
            }));

        #endregion

        #region Minmum
        public string Minmum
        {
            get { return (string)GetValue(MinmumProperty); }
           internal set { SetValue(MinmumProperty, value); }
        }

        public static readonly DependencyProperty MinmumProperty =
            DependencyProperty.Register("Minmum", typeof(string), typeof(RangeSlider), new PropertyMetadata("0"));

        #endregion

        #region Maxmum
        public string Maxmum
        {
            get { return (string)GetValue(MaxmumProperty); }
            internal set { SetValue(MaxmumProperty, value); }
        }

        public static readonly DependencyProperty MaxmumProperty =
            DependencyProperty.Register("Maxmum", typeof(string), typeof(RangeSlider), new PropertyMetadata("120"));

        #endregion


        #region MinValue
        public string MinValue
        {
            get { return (string)GetValue(MinValueProperty); }
           internal set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(string), typeof(RangeSlider), new PropertyMetadata(""));
        #endregion

        #region MaxValue
        public string MaxValue
        {
            get { return (string)GetValue(MaxValueProperty); }
           internal set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(string), typeof(RangeSlider), new PropertyMetadata(""));
        #endregion

        #region SelectedBackground
        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SelectedBackgroundProperty =
            DependencyProperty.Register("SelectedBackground", typeof(Brush), typeof(RangeSlider), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(225, 248, 33, 106))));
        #endregion

        #region CircleBackgroud
        public Brush CircleBackgroud
        {
            get { return (Brush)GetValue(CircleBackgroudProperty); }
            set { SetValue(CircleBackgroudProperty, value); }
        }

        public static readonly DependencyProperty CircleBackgroudProperty =
            DependencyProperty.Register("CircleBackgroud", typeof(Brush), typeof(RangeSlider), new PropertyMetadata(Colors.White));
        #endregion

        #region CicleOuterBackground
        public Brush CicleOuterBackground
        {
            get { return (Brush)GetValue(CicleOuterBackgroundProperty); }
            set { SetValue(CicleOuterBackgroundProperty, value); }
        }

        public static readonly DependencyProperty CicleOuterBackgroundProperty =
            DependencyProperty.Register("CicleOuterBackground", typeof(Brush), typeof(RangeSlider), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(225, 51, 51, 51))));
        #endregion

        #region RangeFontForeground
        public Brush RangeFontForeground
        {
            get { return (Brush)GetValue(RangeFontForegroundProperty); }
            set { SetValue(RangeFontForegroundProperty, value); }
        }

        public static readonly DependencyProperty RangeFontForegroundProperty =
            DependencyProperty.Register("RangeFontForeground", typeof(Brush), typeof(RangeSlider), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(225, 51, 51, 51))));
        #endregion

        #region Minimum
        public Double Minimum
        {
            get { return (Double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(Double), typeof(RangeSlider), new PropertyMetadata(0.0, (s, e) =>
            {
                RangeSlider rangeSlider = s as RangeSlider;
                rangeSlider.Minmum = e.NewValue.ToString();
            }));
        #endregion

        #region Maximum
        public Double Maximum
        {
            get { return (Double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(Double), typeof(RangeSlider), new PropertyMetadata(120.0, (s, e) =>
            {
                RangeSlider rangeSlider = s as RangeSlider;
                rangeSlider.Maxmum = e.NewValue.ToString();
            }));
        #endregion

        #region Value
        public Double Value
        {
            get { return (Double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Double), typeof(RangeSlider), new PropertyMetadata(100.0, (s, e) =>
            {
                RangeSlider rangeSlider = s as RangeSlider;
                rangeSlider.MaxValue = e.NewValue.ToString();
                if (Convert.ToDouble(e.NewValue) > rangeSlider.Maximum) rangeSlider.Value = rangeSlider.Maximum;                            
                if (rangeSlider.FromThumbVisbilty == Visibility.Collapsed)
                {
                    if (Convert.ToDouble(e.NewValue) <= rangeSlider.Minimum) rangeSlider.Value = rangeSlider.Minimum;
                    rangeSlider.RangeFrom = 0;
                    rangeSlider.RangeTo = (rangeSlider.totalValue / (rangeSlider.Maximum - rangeSlider.Minimum)) * (rangeSlider.Maximum - rangeSlider.Value) + rangeSlider.toLength;

                }
                else
                {
                    if (Convert.ToDouble(e.NewValue) <= rangeSlider.FromValue + 1) rangeSlider.Value = rangeSlider.FromValue + 1;
                    rangeSlider.RangeTo = (rangeSlider.totalValue / (rangeSlider.Maximum - rangeSlider.Minimum)) * (rangeSlider.Maximum - rangeSlider.Value) + rangeSlider.toLength;

                }
                rangeSlider.OnValueChanged(e);
            }));
        #endregion

        #region Width
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double), typeof(RangeSlider), new PropertyMetadata(500.0));
        #endregion

        #region Background
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(RangeSlider), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(225, 235, 235, 235))));
        #endregion

        #region Foreground


        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(RangeSlider), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(225, 204, 204, 204))));

        
        #endregion

        #region Event
        public event DependencyPropertyChangedEventHandler ValueChanged;
        protected void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            DependencyPropertyChangedEventHandler handler = ValueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event DependencyPropertyChangedEventHandler FromValueChanged;
        protected void OnFromValueChanged(DependencyPropertyChangedEventArgs e)
        {
            DependencyPropertyChangedEventHandler handler = FromValueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        public RangeSlider()
        {
            this.DefaultStyleKey = typeof(RangeSlider);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitRangeSlider();
           
        }

        void InitRangeSlider()
        {
            try
            {
                (this.GetTemplateChild(HoriontalTemplate) as Grid).Visibility = this.Orientation == Orientation.Vertical ? Visibility.Collapsed : Visibility.Visible;
                (this.GetTemplateChild(VerticalTemplate) as Grid).Visibility = this.Orientation == Orientation.Vertical ? Visibility.Visible : Visibility.Collapsed;
                thumbFrom = this.Orientation == Orientation.Vertical ? this.GetTemplateChild(thumbVerticalFrom) as Thumb : this.GetTemplateChild(thumbHoriontalFrom) as Thumb;
                thumbTo = this.Orientation == Orientation.Vertical ? this.GetTemplateChild(thumbVerticalTo) as Thumb : this.GetTemplateChild(thumbHoriontalTo) as Thumb;
                fromLength = this.Orientation == Orientation.Vertical ? this.thumbFrom.Height : this.thumbFrom.Width;
                toLength = this.Orientation == Orientation.Vertical ? this.thumbTo.Height : this.thumbTo.Width;
                totalLength = this.Orientation == Orientation.Vertical ? this.Height : this.Width;
            }
            catch { }
            totalValue = FromThumbVisbilty == Visibility.Visible ? totalLength - fromLength - toLength - toLength / 2 : totalLength - toLength;

            if ((this.Maximum - this.Minimum) >= 0)
            {
                perValue = totalValue / (this.Maximum - this.Minimum);
                if (FromThumbVisbilty == Visibility.Collapsed)
                {
                    RangeFrom = 0;
                    RangeTo = perValue * (this.Maximum - this.Value) + toLength;
                }
                else
                {
                    RangeFrom = perValue * (FromValue - this.Minimum) + fromLength;
                    RangeTo = perValue * (this.Maximum - Value) + toLength;

                }

            }

            MinValue = FromValue.ToString();
            MaxValue = Value.ToString();
            if (thumbFrom != null)
            {
                thumbFrom.ManipulationStarted += fromThumb_ManipulationStarted;
                thumbFrom.ManipulationDelta += fromThumb_ManipulationDelta;
                thumbFrom.PointerEntered += thumbFrom_PointerEntered;
                thumbFrom.ManipulationCompleted += thumbFrom_ManipulationCompleted;
                thumbFrom.LostFocus += thumbTo_LostFocus;
                thumbFrom.PointerExited += thumbTo_PointerExited;
            }
            if (thumbTo != null)
            {
                thumbTo.ManipulationStarted += thumbTo_ManipulationStarted;
                thumbTo.ManipulationDelta += thumbTo_ManipulationDelta;
                thumbTo.PointerEntered += thumbTo_PointerEntered;
                thumbTo.ManipulationCompleted += thumbFrom_ManipulationCompleted;
                thumbTo.LostFocus += thumbTo_LostFocus;
                thumbTo.PointerExited += thumbTo_PointerExited;
            }

            try
            {
                Grid rectLeftHoriontal = this.GetTemplateChild(gridLeftHoriontal) as Grid;
                Rectangle MiddleHoriontal = this.GetTemplateChild(rectMiddleHoriontal) as Rectangle;
                Grid rectRightHoriontal = this.GetTemplateChild(gridRightHoriontal) as Grid;
                Grid rectLeftVertical = this.GetTemplateChild(gridLeftVertical) as Grid;
                Rectangle MiddleVertical = this.GetTemplateChild(rectMiddleVertical) as Rectangle;
                Grid rectRightVertical = this.GetTemplateChild(gridRightVertical) as Grid;

                rectLeftHoriontal.Tapped += rect_Tapped;
                MiddleHoriontal.Tapped += rect_Tapped;
                rectRightHoriontal.Tapped += rect_Tapped;
                rectLeftVertical.Tapped += rect_Tapped;
                MiddleVertical.Tapped += rect_Tapped;
                rectRightVertical.Tapped += rect_Tapped;
            }
            catch { }
        }

        void thumbTo_LostFocus(object sender, RoutedEventArgs e)
        {
            PopCollasped();
        }

        void thumbFrom_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            PopCollasped();
        }

        void thumbTo_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            PopCollasped();
        }

        void thumbTo_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            (GetTemplateChild(popRightHoriontal) as Grid).Visibility = Visibility.Visible;
            (GetTemplateChild(popRightVertical) as Grid).Visibility = Visibility.Visible;
        }


        void thumbFrom_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            (GetTemplateChild(popLeftHoriontal) as Grid).Visibility = Visibility.Visible;
            (GetTemplateChild(popLeftVertical) as Grid).Visibility = Visibility.Visible;
        }

        void PopCollasped()
        {
            (GetTemplateChild(popRightHoriontal) as Grid).Visibility = Visibility.Collapsed;
            (GetTemplateChild(popLeftHoriontal) as Grid).Visibility = Visibility.Collapsed;
            (GetTemplateChild(popRightVertical) as Grid).Visibility = Visibility.Collapsed;
            (GetTemplateChild(popLeftVertical) as Grid).Visibility = Visibility.Collapsed;
        }

        void rect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid grid = this.Orientation == Orientation.Horizontal ? this.GetTemplateChild(HoriontalTemplate) as Grid : this.GetTemplateChild(VerticalTemplate) as Grid;
            Point currentPoint = e.GetPosition(grid);
            double currentLength = this.Orientation == Orientation.Horizontal ? currentPoint.X : currentPoint.Y;
            if (currentLength > (totalLength - RangeTo) || FromThumbVisbilty == Visibility.Collapsed)
            {
                RangeTo = totalLength - currentLength <= toLength ? toLength : totalLength - currentLength;
                this.Value = this.Value = this.Maximum - Convert.ToInt32((RangeTo - toLength) / perValue) == this.FromValue ? this.FromValue + 1 : this.Maximum - Convert.ToInt32((RangeTo - toLength) / perValue);
                MaxValue = Value.ToString();
                ShowRightPop();
            }
            else
            {
                RangeFrom = currentLength <= fromLength ? fromLength : currentLength;
                this.FromValue = Convert.ToInt32((RangeFrom - fromLength) / perValue) == this.Value ? this.Value - 1 : Convert.ToInt32((RangeFrom - fromLength) / perValue);
                MinValue = FromValue.ToString();
                ShowLeftPop();
            }

        }


        void thumbTo_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            (GetTemplateChild(popRightHoriontal) as Grid).Visibility = Visibility.Visible;
            (GetTemplateChild(popRightVertical) as Grid).Visibility = Visibility.Visible;
            double limitLength = FromThumbVisbilty == Visibility.Visible ? toLength / 2 : 0; ;
            if (this.Orientation == Orientation.Vertical)
            {
                if ((RangeTo - (e.Position.Y - FoucsPoint.Y)) >= thumbTo.Height && (RangeTo - (e.Position.Y - FoucsPoint.Y)) <= this.Height - RangeFrom - limitLength)
                {
                    RangeTo = RangeTo - (e.Position.Y - FoucsPoint.Y);
                }
                else if (RangeTo - (e.Position.Y - FoucsPoint.Y) < thumbTo.Height)
                    RangeTo = thumbTo.Height;
                else if (RangeTo - (e.Position.Y - FoucsPoint.Y) > this.Height - RangeFrom - limitLength)
                    RangeTo = this.Height - RangeFrom - limitLength;

            }
            else
            {
                if ((RangeTo - (e.Position.X - FoucsPoint.X)) >= thumbTo.Width && (RangeTo - (e.Position.X - FoucsPoint.X)) <= this.Width - RangeFrom - limitLength)
                {
                    RangeTo = RangeTo - (e.Position.X - FoucsPoint.X);
                }
                else if (RangeTo - (e.Position.X - FoucsPoint.X) < thumbTo.Width)
                    RangeTo = thumbTo.Width;
                else if (RangeTo - (e.Position.X - FoucsPoint.X) > this.Width - RangeFrom - limitLength)
                    RangeTo = this.Width - RangeFrom - limitLength;
            }
            this.Value = this.Maximum - Convert.ToInt32((RangeTo - toLength) / perValue) == this.FromValue ? this.FromValue + 1 : this.Maximum - Convert.ToInt32((RangeTo - toLength) / perValue);
            MaxValue = this.Value.ToString();

        }

        void thumbTo_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            FoucsPoint = e.Position;
            ShowRightPop();
        }

        void fromThumb_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            (GetTemplateChild(popLeftHoriontal) as Grid).Visibility = Visibility.Visible;
            (GetTemplateChild(popLeftVertical) as Grid).Visibility = Visibility.Visible;
            if (this.Orientation == Orientation.Horizontal)
            {
                if (RangeFrom + (e.Position.X - FoucsPoint.X) >= thumbFrom.Width && RangeFrom + (e.Position.X - FoucsPoint.X) <= this.Width - RangeTo - thumbFrom.Width / 2)
                {
                    RangeFrom = RangeFrom + (e.Position.X - FoucsPoint.X);
                }
                else if (RangeFrom + (e.Position.X - FoucsPoint.X) < thumbFrom.Width)
                { RangeFrom = fromLength; }
                else if (RangeFrom + (e.Position.X - FoucsPoint.X) > this.Width - RangeTo - thumbFrom.Width / 2)
                    RangeFrom = this.Width - RangeTo - thumbFrom.Width / 2;

            }
            else
            {
                if (RangeFrom + (e.Position.Y - FoucsPoint.Y) >= thumbFrom.Height && RangeFrom + (e.Position.Y - FoucsPoint.Y) <= this.Height - RangeTo - thumbFrom.Height / 2)
                {
                    RangeFrom = RangeFrom + (e.Position.Y - FoucsPoint.Y);
                }
                else if (RangeFrom + (e.Position.Y - FoucsPoint.Y) < thumbFrom.Height)
                    RangeFrom = fromLength;
                else if (RangeFrom + (e.Position.Y - FoucsPoint.Y) > this.Height - RangeTo - thumbFrom.Height / 2)
                    RangeFrom = this.Height - RangeTo - thumbFrom.Height / 2;
            }

            this.FromValue = Convert.ToInt32((RangeFrom - fromLength) / perValue) + this.Minimum == this.Value ? this.Value - 1 : Convert.ToInt32((RangeFrom - fromLength) / perValue) + this.Minimum;
            MinValue = FromValue.ToString();

        }

        void fromThumb_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            FoucsPoint = e.Position;
            ShowLeftPop();
        }

        private void ShowLeftPop()
        {
            (GetTemplateChild(popLeftHoriontal) as Grid).Visibility = Visibility.Visible;
            (GetTemplateChild(popRightHoriontal) as Grid).Visibility = Visibility.Collapsed;
            (GetTemplateChild(popLeftVertical) as Grid).Visibility = Visibility.Visible;
            (GetTemplateChild(popRightVertical) as Grid).Visibility = Visibility.Collapsed;
        }
        private void ShowRightPop()
        {
            (GetTemplateChild(popLeftHoriontal) as Grid).Visibility = Visibility.Collapsed;
            (GetTemplateChild(popRightHoriontal) as Grid).Visibility = Visibility.Visible;
            (GetTemplateChild(popLeftVertical) as Grid).Visibility = Visibility.Collapsed;
            (GetTemplateChild(popRightVertical) as Grid).Visibility = Visibility.Visible;
        }
    }
}
