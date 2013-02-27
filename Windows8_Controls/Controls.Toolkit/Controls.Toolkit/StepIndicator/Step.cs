using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;



namespace Controls.Toolkit
{
    public sealed class Step : Control
    {
        #region IsSelected
        internal bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        internal static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(Step), new PropertyMetadata(null, (s, e) =>
            {
                Step step = s as Step;
                if (e.NewValue.ToString().ToUpper() == "TRUE")
                {
                    VisualStateManager.GoToState(step, "Selected", true);
                }

                else if (step.IsFinished==true)
                {
                    VisualStateManager.GoToState(step, "Finished", true);
                }
                else
                {
                    VisualStateManager.GoToState(step, "Basic", true);
                }
            }));

        #endregion

        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Step), new PropertyMetadata(""));
        #endregion

        #region IsFinished
        public bool IsFinished
        {
            get { return (bool)GetValue(IsFinishedProperty); }
            set { SetValue(IsFinishedProperty, value); }
        }

        public static readonly DependencyProperty IsFinishedProperty =
            DependencyProperty.Register("IsFinished", typeof(bool), typeof(Step), new PropertyMetadata(null, (s, e) =>
            {
                Step step = s as Step;
                if (e.NewValue.ToString().ToUpper() == "TRUE")
                {
                    VisualStateManager.GoToState(step, "Finished", true);
                }
               else if (step.IsSelected == true)
                {
                    VisualStateManager.GoToState(step, "Selected", true);
                }
                else
                {
                    VisualStateManager.GoToState(step, "Basic", true);
                }
            }));
        #endregion     

        #region IsInconformity
        public bool IsInconformity
        {
            get { return (bool)GetValue(IsInconformityProperty); }
            set { SetValue(IsInconformityProperty, value); }
        }

        public static readonly DependencyProperty IsInconformityProperty =
            DependencyProperty.Register("IsInconformity", typeof(bool), typeof(Step), new PropertyMetadata(false));
        #endregion

        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SelectedBackgroundProperty =
            DependencyProperty.Register("SelectedBackground", typeof(Brush), typeof(Knob), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(225, 248, 33, 106))));



        public Brush FinishedBackground
        {
            get { return (Brush)GetValue(FinishedBackgroundProperty); }
            set { SetValue(FinishedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty FinishedBackgroundProperty =
            DependencyProperty.Register("FinishedBackground", typeof(Brush), typeof(Knob), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(225, 204, 204, 204))));

        

        public Step()
        {
            this.DefaultStyleKey = typeof(Step);

        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.IsSelected)
                VisualStateManager.GoToState(this, "Selected", true);
            if(this.IsFinished)
                VisualStateManager.GoToState(this, "Finished", true);
            try
            {
                if (this.IsInconformity)
                {
                    (GetTemplateChild("StepTemplateFront") as Grid).Visibility = Visibility.Visible;
                    (GetTemplateChild("StepTemplateRegular") as Grid).Visibility = Visibility.Collapsed;
                }
                else
                {
                    (GetTemplateChild("StepTemplateFront") as Grid).Visibility = Visibility.Collapsed;
                    (GetTemplateChild("StepTemplateRegular") as Grid).Visibility = Visibility.Visible;
                }
            }
            catch { }
        }
    }
}
