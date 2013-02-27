using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;


namespace Controls.Toolkit
{

    public sealed class StepIndicator : Control
    {
        public StackPanel stepIndicatoryPanel;

        #region SelectedIndex
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(StepIndicator), new PropertyMetadata(null, (s, e) =>
            {
                StepIndicator stepIndicator = s as StepIndicator;
                if (stepIndicator.childStep != null)
                {
                    stepIndicator.InitStep();
                    if (Convert.ToInt32(e.NewValue) > stepIndicator.childStep.Count - 1) { stepIndicator.SelectedIndex = stepIndicator.childStep.Count - 1; return; }
                    if (Convert.ToInt32(e.NewValue) < 0) { stepIndicator.SelectedIndex = 0; return; }
                }
                stepIndicator.OnSelectIndexChanged(e);
            }));
        #endregion

        #region DifferentTemplateIndex
        public int DifferentTemplateIndex
        {
            get { return (int)GetValue(DifferentTemplateIndexProperty); }
            set { SetValue(DifferentTemplateIndexProperty, value); }
        }

        public static readonly DependencyProperty DifferentTemplateIndexProperty =
            DependencyProperty.Register("DifferentTemplateIndex", typeof(int), typeof(StepIndicator), new PropertyMetadata(0));
        #endregion

        #region Event
        public event DependencyPropertyChangedEventHandler SelectIndexChanged;
        protected void OnSelectIndexChanged(DependencyPropertyChangedEventArgs e)
        {
            DependencyPropertyChangedEventHandler handler = SelectIndexChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        private List<Step> childStep;
        public StepIndicator()
        {
            this.DefaultStyleKey = typeof(StepIndicator);

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            childStep = new List<Step>();
            stepIndicatoryPanel = this.GetTemplateChild("stepIndicatorPanel") as StackPanel;
            childStep = GetChildern(this);
            InitStep();
        }
        void InitStep()
        {
            if (SelectedIndex >= childStep.Count) SelectedIndex = childStep.Count - 1;
            for (int i = 0; i < childStep.Count; i++)
            {
                Step stepChild = childStep[i];
                if (i == DifferentTemplateIndex)
                    stepChild.IsInconformity = true;
                if (i == SelectedIndex)
                { stepChild.IsSelected = true; stepChild.IsFinished = false; }
                else if (i < SelectedIndex)
                {
                    stepChild.IsSelected = false;
                    stepChild.IsFinished = true;
                }
                else
                {
                    stepChild.IsSelected = false;
                    stepChild.IsFinished = false;
                }
            }
            foreach (Step step in childStep)
            {
                step.Tapped += step_Tapped;
            }
        }

        void step_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Step step = sender as Step;
            for (int i = 0; i < childStep.Count; i++)
            {
                Step stepChild = childStep[i];
                if (stepChild == step)
                {
                    SelectedIndex = i; break;
                }
            }

        }

        private List<Step> GetChildern(DependencyObject root)
        {
            if (root == null)
                return null;
            int count = VisualTreeHelper.GetChildrenCount(root);
            if (count <= 0)
                return null;

            for (int i = 0; i < count; i++)
            {
                DependencyObject dependencyObject = VisualTreeHelper.GetChild(root, i);
                if (dependencyObject != null)
                {
                    if (VisualTreeHelper.GetChildrenCount(root) > 0)
                    {
                        GetChildern(dependencyObject);
                    }
                    if (dependencyObject.GetType() == typeof(Step))
                        childStep.Add(dependencyObject as Step);
                }
            }
            return childStep;
        }

        public void GotoNext()
        {
            SelectedIndex = SelectedIndex == childStep.Count - 1 ? childStep.Count - 1 : SelectedIndex + 1;
        }

        public void GotoBack()
        {
            SelectedIndex = SelectedIndex == 0 ? 0 : SelectedIndex - 1;
        }
    }
}
