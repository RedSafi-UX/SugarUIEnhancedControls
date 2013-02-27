using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;


namespace Controls.Toolkit
{
    [TemplatePart(Name = ElementTextBoxName, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementOkButtonName, Type = typeof(ContentTextBoxButton))]
    [TemplatePart(Name = ElementCancelButtonName, Type = typeof(ContentTextBoxButton))]
    [TemplatePart(Name = ElementAddFileButtonName, Type = typeof(Button))]
    [TemplatePart(Name = ElementBackgroundRectangleName, Type = typeof(Rectangle))]
    public class ContentTextBox : ContentControl
    {
        #region Template Parts Name Constants
        /// <summary>
        /// Name constant for MainTextBox template part.
        /// </summary>
        private const string ElementTextBoxName = "MainTextBox";
        /// <summary>
        ///  Name constant for OKButton template part.
        /// </summary>
        private const string ElementOkButtonName = "OKButton";
        /// <summary>
        ///  Name constant for CancelButton template part.
        /// </summary>
        private const string ElementCancelButtonName = "CancelButton";
        /// <summary>
        ///  Name constant for BackgroundRectangle template part.
        /// </summary>
        private const string ElementBackgroundRectangleName = "BackgroundRectangle";
        /// <summary>
        ///  Name constant for AddFileButton template part.
        /// </summary>
        private const string ElementAddFileButtonName = "AddFileButton";

        #endregion

        public ContentTextBox()
        {
            this.DefaultStyleKey = typeof(ContentTextBox);

            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        }

        public new Visibility Visibility
        {
            get { return base.Visibility; }
            set
            {
                base.Visibility = value;
                if (value == Windows.UI.Xaml.Visibility.Visible)
                {
                    this.IsOpen = true;
                }
                else
                {
                    this.IsOpen = false;
                }
            }
        }

        #region TextLength
        public string TextLength
        {
            get { return (string)GetValue(TextLengthProperty); }
            private set { SetValue(TextLengthProperty, value); }
        }

        public static readonly DependencyProperty TextLengthProperty =
            DependencyProperty.Register("TextLength", typeof(string), typeof(ContentTextBox), new PropertyMetadata("0"));
        #endregion

        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ContentTextBox), new PropertyMetadata(string.Empty));
        #endregion


        #region IsOpen
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(ContentTextBox), new PropertyMetadata(false, IsOpenPropertyChangedCallback));

        public static void IsOpenPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender != null)
            {
                ContentTextBox txtbox = sender as ContentTextBox;
                txtbox.SetState((bool)args.NewValue);
            }
        }

        #endregion

        #region
        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        public static readonly DependencyProperty BackgroundOpacityProperty =
            DependencyProperty.Register("BackgroundOpacity", typeof(double), typeof(ContentTextBox), new PropertyMetadata(0.5));

        #endregion



        #region Event

        /// <summary>
        /// Ok Button Click Event
        /// </summary>
        public event RoutedEventHandler OkButtonClick;
        /// <summary>
        /// Cancel Button Click Event
        /// </summary>
        public event RoutedEventHandler CancelButtonClick;

        /// <summary>
        /// control closed event
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// control opend event
        /// </summary>
        public event EventHandler Opend;

        /// <summary>
        /// AddFile button Click Event
        /// </summary>
        public event RoutedEventHandler AddFileButtonClick;

        #endregion

        #region TextBox
        private TextBox _textBox;

        internal TextBox TextBox
        {
            get { return _textBox; }
            private set
            {
                if (_textBox != null)
                {
                    _textBox.TextChanged -= OnTextBoxTextChanged;
                }
                _textBox = value;
                if (_textBox != null)
                {
                    _textBox.TextChanged += OnTextBoxTextChanged;
                }
            }
        }

        #endregion

        #region OkButton
        private ContentTextBoxButton _okButton;

        internal ContentTextBoxButton OkButton
        {
            get { return _okButton; }
            private set
            {
                if (_okButton != null)
                {
                    _okButton.Click -= OnOkButtonClick;
                }

                _okButton = value;

                if (_okButton != null)
                {
                    _okButton.Click += OnOkButtonClick;
                }
            }
        }
        #endregion

        #region CancelButton
        private ContentTextBoxButton _cancelButton;
        internal ContentTextBoxButton CancelButton
        {
            get { return _cancelButton; }
            private set
            {
                if (_cancelButton != null)
                {
                    _cancelButton.Click -= OnCancelButtonClick;
                }
                _cancelButton = value;
                if (_cancelButton != null)
                {
                    _cancelButton.Click += OnCancelButtonClick;
                }
            }
        }
        #endregion

        private Button _addFileButton;
        internal Button AddFileButton
        {
            get { return _addFileButton; }
            private set
            {
                if (_addFileButton != null)
                {
                    _addFileButton.Click -= OnAddFileButtonClick;
                }

                _addFileButton = value;

                if (_addFileButton != null)
                {
                    _addFileButton.Click += OnAddFileButtonClick;
                }
            }
        }



        #region BackgroundRectangle

        internal Rectangle BackgroundRectangle
        {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Event handle for TextBox Template part's TextChanged Event
        /// </summary>
        /// <param name="sender">Template parts</param>
        /// <param name="e">event args</param>
        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            this.TextLength = _textBox.Text.Length.ToString();
            this.Text = _textBox.Text;
        }

        /// <summary>
        /// Event handle for CancelButton Template part's click Event.
        /// </summary>
        /// <param name="sender">Template parts</param>
        /// <param name="e">event args</param>
        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.IsOpen = false;

            if (CancelButtonClick != null)
            {
                CancelButtonClick(sender, e);
            }
        }
        /// <summary>
        /// Event handle for OkButton Template part's click Event.
        /// </summary>
        /// <param name="sender">Template parts</param>
        /// <param name="e">event args</param>
        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            this.IsOpen = false;

            if (OkButtonClick != null)
            {
                OkButtonClick(sender, e);
            }
        }
        /// <summary>
        /// Event handle for AddFileButton Template part's click Event.
        /// </summary>
        /// <param name="sender">Template parts</param>
        /// <param name="e">event args</param>
        private void OnAddFileButtonClick(object sender, RoutedEventArgs e)
        {
            if (AddFileButtonClick != null)
            {
                AddFileButtonClick(sender, e);
            }
        }

        /// <summary>
        /// Builds the visual tree for the ContentTextBox control when a new 
        /// template is applied.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TextBox = GetTemplateChild(ElementTextBoxName) as TextBox;
            OkButton = GetTemplateChild(ElementOkButtonName) as ContentTextBoxButton;
            CancelButton = GetTemplateChild(ElementCancelButtonName) as ContentTextBoxButton;
            BackgroundRectangle = GetTemplateChild(ElementBackgroundRectangleName) as Rectangle;
            AddFileButton = GetTemplateChild(ElementAddFileButtonName) as Button;

            this.SetFocusOnTextBox();
        }

        private void SetState(bool isOpen)
        {
            if (isOpen)
            {
                Canvas.SetZIndex(this, 1000000);
                if (this.Visibility != Windows.UI.Xaml.Visibility.Visible)
                {
                    this.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }

                this.SetFocusOnTextBox();

                if (this.Opend != null)
                {
                    this.Opend(this, null);
                }
            }
            else
            {
                Canvas.SetZIndex(this, -1);
                if (this.Visibility != Windows.UI.Xaml.Visibility.Collapsed)
                {
                    this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }

                if (this.Closed != null)
                {
                    this.Closed(this, null);
                }
            }
        }
        /// <summary>
        ///  //Set focus on TextBox Element
        /// </summary>
        private void SetFocusOnTextBox()
        {
            if (TextBox != null)
            {
                TextBox.Focus(FocusState.Keyboard);
            }
        }
    }
}
