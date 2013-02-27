using System;
using System.Collections.Generic;
using System.Diagnostics;
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


namespace Controls.Toolkit
{
    [TemplatePart(Name = grid, Type = typeof(Grid))]
    [TemplatePart(Name = composite, Type = typeof(CompositeTransform))]
    [TemplatePart(Name = textComposite, Type = typeof(CompositeTransform))]
    [TemplatePart(Name = gridComposite, Type = typeof(CompositeTransform))]
    [TemplatePart(Name = rotateGrid, Type = typeof(Grid))]
    [TemplatePart(Name = gridText, Type = typeof(Grid))]
    [TemplatePart(Name = offsetFirst1, Type = typeof(GradientStop))]
    [TemplatePart(Name = offsetFirst2, Type = typeof(GradientStop))]
    [TemplatePart(Name = offsetSecond1, Type = typeof(GradientStop))]
    [TemplatePart(Name = offsetSecond2, Type = typeof(GradientStop))]
    [TemplatePart(Name = offsetThird1, Type = typeof(GradientStop))]
    [TemplatePart(Name = offsetThird2, Type = typeof(GradientStop))]

    public sealed class Knob : Control
    {
        #region
        private const string grid = "grid";
        private const string composite = "composite";
        private const string textComposite = "textComposite";
        private const string gridComposite = "gridComposite";
        private const string rotateGrid = "rotateGrid";
        private const string gridText = "gridText";
        private const string offsetFirst1 = "offsetFirst1";
        private const string offsetFirst2 = "offsetFirst2";
        private const string offsetFirst3 = "offsetFirst3";
        private const string offsetSecond1 = "offsetSecond1";
        private const string offsetSecond2 = "offsetSecond2";
        private const string offsetSecond3 = "offsetSecond3";
        private const string offsetThird1 = "offsetThird1";
        private const string offsetThird2 = "offsetThird2";
        private const string offsetThird3 = "offsetThird3";
        Point oldPoint, centerPoint;
        double rotate, controlWidth, marginLeft, rotateHeight, perValue;
        CompositeTransform compositTransform, textCompositTransform, gridCompositeTransform;
        #endregion

        #region TextValue
        public string TextVlaue
        {
            get { return (string)GetValue(TextVlaueProperty); }
            internal set { SetValue(TextVlaueProperty, value); }
        }

        public static readonly DependencyProperty TextVlaueProperty =
            DependencyProperty.Register("TextVlaue", typeof(string), typeof(Knob), new PropertyMetadata(""));

        #endregion

        #region Maximum
        public Double Maximum
        {
            get { return (Double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(Double), typeof(Knob), new PropertyMetadata(120.0, (s, e) =>
            {
                Knob knob = s as Knob;
                if (knob.Maximum < knob.Minimum) knob.Maximum = knob.Minimum;
                knob.MaxValue = knob.Maximum.ToString();
            }));
        #endregion

        #region FillBackground
        public Color FillBackground
        {
            get { return (Color)GetValue(FillBackgroundProperty); }
            set { SetValue(FillBackgroundProperty, value); }
        }

        public static readonly DependencyProperty FillBackgroundProperty =
            DependencyProperty.Register("FillBackground", typeof(Color), typeof(Knob), new PropertyMetadata(Color.FromArgb(225, 248, 33, 106), (s, e) =>
            {
                Knob knob = s as Knob;
                knob.SetGradientStopOffset();
                knob.PopColor = new SolidColorBrush(knob.FillBackground);
            }));
        #endregion

        #region PopColor
        public Brush PopColor
        {
            get { return (Brush)GetValue(PopColorProperty); }
            internal set { SetValue(PopColorProperty, value); }
        }

        public static readonly DependencyProperty PopColorProperty =
            DependencyProperty.Register("PopColor", typeof(Brush), typeof(Knob), new PropertyMetadata(null, (s, e) =>
            {

            }));
        #endregion

        #region Background
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(Knob), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(225, 235, 235, 235))));
        #endregion

        #region Value
        public Double Value
        {
            get { return (Double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Double), typeof(Knob), new PropertyMetadata(100.0, (s, e) =>
            {
                Knob knob = s as Knob;
                knob.TextVlaue = e.NewValue.ToString();
                if (Convert.ToDouble(e.NewValue) < 0) { knob.Value = 0; return; }
                if (Convert.ToDouble(e.NewValue) > knob.Maximum) { knob.Value = knob.Maximum; return; }
                if (knob.compositTransform != null)
                    knob.compositTransform.Rotation = -knob.Value * (270 / (Convert.ToDouble(knob.Maximum) - Convert.ToDouble(knob.Minimum)));
                if (knob.textCompositTransform != null)
                    knob.textCompositTransform.Rotation = -knob.compositTransform.Rotation;
                if (knob.gridCompositeTransform != null)
                    knob.gridCompositeTransform.Rotation = -knob.compositTransform.Rotation;
                knob.SetGradientStopOffset();
                knob.OnValueChanged(e);
            }));
        #endregion

        #region Minimum
        public Double Minimum
        {
            get { return (Double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(Double), typeof(Knob), new PropertyMetadata(null, (s, e) =>
            {
                Knob knob = s as Knob;
                if (knob.Maximum < knob.Minimum) knob.Minimum = knob.Maximum;
                if (knob.Minimum < 0) knob.Minimum = 0;
                knob.MinValue = knob.Minimum.ToString();
            }));
        #endregion

        #region MinValue
        public String MinValue
        {
            get { return (String)GetValue(MinValueProperty); }
            internal set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(String), typeof(Knob), new PropertyMetadata(0));
        #endregion

        #region MaxValue
        public String MaxValue
        {
            get { return (String)GetValue(MaxValueProperty); }
            internal set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(String), typeof(Knob), new PropertyMetadata("120"));
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
        #endregion

        public Knob()
        {
            this.DefaultStyleKey = typeof(Knob);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            controlWidth = (this.GetTemplateChild(grid) as Grid).Width;
            compositTransform = this.GetTemplateChild(composite) as CompositeTransform;
            textCompositTransform = this.GetTemplateChild(textComposite) as CompositeTransform;
            gridCompositeTransform = this.GetTemplateChild(gridComposite) as CompositeTransform;
            if (Convert.ToDouble(Maximum) - Convert.ToDouble(Minimum) > 0&&(Value>=Minimum&&Value<=Maximum))
            {
                perValue = 270 / (Convert.ToDouble(Maximum) - Convert.ToDouble(Minimum));

                if (compositTransform != null)
                    compositTransform.Rotation = -Value * perValue;
                if (textComposite != null)
                    textCompositTransform.Rotation = -compositTransform.Rotation;
                if (gridCompositeTransform != null)
                    gridCompositeTransform.Rotation = -compositTransform.Rotation;
                TextVlaue = Value.ToString();
                MaxValue = Maximum.ToString();
                SetGradientStopOffset();
                PopColor = new SolidColorBrush(FillBackground); ;
                Grid rotate = this.GetTemplateChild(rotateGrid) as Grid;
                if (rotate != null)
                {
                    marginLeft = rotate.Margin.Left;
                    rotateHeight = rotate.Height;
                    centerPoint = new Point(-(marginLeft - controlWidth / 2), rotateHeight / 2);
                    rotate.ManipulationStarted += rotateGrid_ManipulationStarted;
                    rotate.ManipulationDelta += rotateGrid_ManipulationDelta;
                    rotate.PointerCaptureLost += rotateGrid_PointerCaptureLost;
                    rotate.PointerEntered += rotate_PointerEntered;
                    rotate.PointerExited += rotate_PointerExited;
                }
            }
        }

        void rotate_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            (this.GetTemplateChild(gridText) as Grid).Visibility = Visibility.Collapsed;
        }

        void rotate_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            (this.GetTemplateChild(gridText) as Grid).Visibility = Visibility.Visible;
        }

        void rotateGrid_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Completed = false;
            (this.GetTemplateChild(gridText) as Grid).Visibility = Visibility.Collapsed;
        }

        bool Completed = false;

        void rotateGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            try
            {
                if (Completed)
                {
                    (this.GetTemplateChild(gridText) as Grid).Visibility = Visibility.Visible;
                    double a, b, c;
                    a = Math.Sqrt((centerPoint.X - oldPoint.X) * (centerPoint.X - oldPoint.X) + (centerPoint.Y - oldPoint.Y) * (centerPoint.Y - oldPoint.Y));
                    b = Math.Sqrt((centerPoint.X - e.Position.X) * (centerPoint.X - e.Position.X) + (centerPoint.Y - e.Position.Y) * (centerPoint.Y - e.Position.Y));
                    c = Math.Sqrt((oldPoint.X - e.Position.X) * (oldPoint.X - e.Position.X) + (oldPoint.Y - e.Position.Y) * (oldPoint.Y - e.Position.Y));
                    rotate = (Math.Acos((a * a + b * b - c * c) / (2 * a * b))) / Math.PI * 180;

                    if (oldPoint.X > centerPoint.X)
                    {
                        if (oldPoint.Y < e.Position.Y)
                        {
                            compositTransform.Rotation = compositTransform.Rotation - rotate;
                        }
                        else
                        {
                            compositTransform.Rotation = compositTransform.Rotation + rotate;
                        }
                    }
                    else
                    {
                        if (oldPoint.Y > e.Position.Y)
                            compositTransform.Rotation = compositTransform.Rotation + rotate;
                        else
                            compositTransform.Rotation = compositTransform.Rotation - rotate;
                    }
                    if (compositTransform.Rotation < -270)
                        compositTransform.Rotation = -270;
                    else if (compositTransform.Rotation > 0)
                        compositTransform.Rotation = 0;
                    textCompositTransform.Rotation = -compositTransform.Rotation;
                    gridCompositeTransform.Rotation = -compositTransform.Rotation;
                    TextVlaue = (Convert.ToInt32(-compositTransform.Rotation / perValue)).ToString();
                    this.Value = Convert.ToInt32(-compositTransform.Rotation / perValue);
                    SetGradientStopOffset();
                }
                else
                    e.Complete();
            }
            catch { }

        }

        void rotateGrid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            oldPoint = e.Position;
            Completed = true;
        }

        private void SetGradientStopOffset()
        {
            try
            {

                if (Convert.ToInt32(TextVlaue) < Convert.ToDouble(Maximum) / 3)
                {
                    (this.GetTemplateChild(offsetFirst1) as GradientStop).Offset = (this.GetTemplateChild(offsetFirst2) as GradientStop).Offset = Convert.ToInt32(TextVlaue) / (Convert.ToDouble(Maximum) / 3);
                    (this.GetTemplateChild(offsetSecond1) as GradientStop).Offset = (this.GetTemplateChild(offsetSecond2) as GradientStop).Offset = 0;
                    (this.GetTemplateChild(offsetThird1) as GradientStop).Offset = (this.GetTemplateChild(offsetThird2) as GradientStop).Offset = 0;
                }
                else if (Convert.ToInt32(TextVlaue) > (Convert.ToDouble(Maximum) / 3 * 2))
                {
                    (this.GetTemplateChild(offsetFirst1) as GradientStop).Offset = (this.GetTemplateChild(offsetFirst2) as GradientStop).Offset = 1;
                    (this.GetTemplateChild(offsetSecond1) as GradientStop).Offset = (this.GetTemplateChild(offsetSecond2) as GradientStop).Offset = 1;
                    (this.GetTemplateChild(offsetThird1) as GradientStop).Offset = (this.GetTemplateChild(offsetThird2) as GradientStop).Offset = (Convert.ToInt32(TextVlaue) - (Convert.ToDouble(Maximum) / 3 * 2)) / (Convert.ToDouble(Maximum) / 3);
                }
                else
                {
                    (this.GetTemplateChild(offsetFirst1) as GradientStop).Offset = (this.GetTemplateChild(offsetFirst2) as GradientStop).Offset = 1;
                    (this.GetTemplateChild(offsetSecond1) as GradientStop).Offset = (this.GetTemplateChild(offsetSecond2) as GradientStop).Offset = (Convert.ToInt32(TextVlaue) - (Convert.ToDouble(Maximum) / 3)) / (Convert.ToDouble(Maximum) / 3);
                    (this.GetTemplateChild(offsetThird1) as GradientStop).Offset = (this.GetTemplateChild(offsetThird2) as GradientStop).Offset = 0;

                }
                (this.GetTemplateChild(offsetFirst1) as GradientStop).Color = (this.GetTemplateChild(offsetFirst3) as GradientStop).Color = FillBackground;
                (this.GetTemplateChild(offsetSecond1) as GradientStop).Color = (this.GetTemplateChild(offsetSecond3) as GradientStop).Color = FillBackground;
                (this.GetTemplateChild(offsetThird1) as GradientStop).Color = (this.GetTemplateChild(offsetThird3) as GradientStop).Color = FillBackground;
            }
            catch { }
        }
    }
}
