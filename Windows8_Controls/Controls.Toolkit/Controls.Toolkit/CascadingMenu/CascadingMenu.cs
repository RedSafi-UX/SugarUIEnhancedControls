using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using System.Reflection;
using System.Collections;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Diagnostics;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Animation;

namespace Controls.Toolkit
{

    [TemplatePart(Name = ElementMainMenuName, Type = typeof(GridView))]
    [TemplatePart(Name = ElementContainerName, Type = typeof(StackPanel))]
    [TemplatePart(Name = ElementUnfoldButtonName, Type = typeof(GridView))]
    public class CascadingMenu : Control
    {
        private const string ElementMainMenuName = "MainMenu";
        private const string ElementContainerName = "Container";
        private const string ElementUnfoldButtonName = "UnfoldButton";
        private const string ElementPathName = "path";

        private GridView _container;
        internal GridView Container
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;
            }
        }

        private GridView _mainMenu;
        internal GridView MainMenu
        {
            get
            {
                return _mainMenu;
            }
            set
            {
                if (_mainMenu != null)
                {
                    _mainMenu.LayoutUpdated -= OnmainMenuLayoutUpdated;
                    _mainMenu.ItemClick -= _mainMenu_ItemClick;
                }
                _mainMenu = value;
                if (_mainMenu != null)
                {
                    _mainMenu.LayoutUpdated += OnmainMenuLayoutUpdated;
                    _mainMenu.SelectionChanged += _mainMenu_SelectionChanged;
                    _mainMenu.ItemClick += _mainMenu_ItemClick;
                }
            }
        }

        void _mainMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainMenu.SelectedItem = e.ClickedItem;
            Type type = e.ClickedItem.GetType();
            IEnumerable<FieldInfo> fields = type.GetRuntimeFields();
            subItemSource = fields.First(i => i.Name.Contains(ItemsPath)).GetValue(e.ClickedItem);
            Container.ItemsSource = subItemSource;
        }

        private GridView _unfoldButton;

        internal GridView UnfoldButton
        {
            get
            {
                return _unfoldButton;
            }
            set
            {
                if (_unfoldButton != null)
                {
                    _unfoldButton.ItemClick -= OnUnfoldButtonItemClick;
                }
                _unfoldButton = value;
                if (_unfoldButton != null)
                {
                    _unfoldButton.ItemClick += OnUnfoldButtonItemClick;

                }
            }
        }

        void _mainMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnfoldButton.SelectedItem = MainMenu.SelectedItem;
        }

        private void OnmainMenuLayoutUpdated(object sender, object e)
        {
            IEnumerable<DependencyObject> enumerItem = FindGridView(MainMenu);
            var list = enumerItem.Where(o => o is GridViewItem);
            GridViewItem gvi = list.First() as GridViewItem;
            ButtonWidth = gvi.ActualWidth;
            var sgvitems = FindGridView(UnfoldButton).Where(o => o is GridViewItem);
            foreach (GridViewItem item in sgvitems)
            {
                if (item.Width != ButtonWidth)
                    item.Width = ButtonWidth;
            }
            UnfoldButton.SelectedItem = MainMenu.SelectedItem;
        }

        static object lastItem;
        void OnUnfoldButtonItemClick(object sender, ItemClickEventArgs e)
        {
            IList itemsource = ItemsSource as IList;
            object item = e.ClickedItem;
            if (MainMenu.SelectedItem != null)
            {
                if (MainMenu.SelectedItem.Equals(e.ClickedItem) || lastItem == item)
                {
                    Container.Visibility = Container.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    Container.Visibility = Visibility.Visible;

                }
            }
            MainMenu.SelectedItem = e.ClickedItem;
            lastItem = item;
            Type type = item.GetType();
            IEnumerable<FieldInfo> fields = type.GetRuntimeFields();
            subItemSource = fields.First(i => i.Name.Contains(ItemsPath)).GetValue(item);
            Container.ItemsSource = subItemSource;
            UnfoldButton.ItemTemplate = Container.Visibility == Visibility.Visible ? UnfoldButton.Resources["upDataTemplate"] as DataTemplate : UnfoldButton.Resources["downDataTemplate"] as DataTemplate;
            if (CascadingMenuSelectionChanged != null)
                CascadingMenuSelectionChanged(sender, e);

        }

        public event ItemClickEventHandler CascadingMenuSelectionChanged;

        public CascadingMenu()
        {
            this.DefaultStyleKey = typeof(CascadingMenu);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            MainMenu = this.GetTemplateChild(ElementMainMenuName) as GridView;
            UnfoldButton = this.GetTemplateChild(ElementUnfoldButtonName) as GridView;
            Container = this.GetTemplateChild(ElementContainerName) as GridView;
            UnfoldButton.ItemTemplate = UnfoldButton.Resources["downDataTemplate"] as DataTemplate;
        }


        public IEnumerable<DependencyObject> Descendents(DependencyObject root, int depth)
        {
            int count = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                if (child != null)
                    yield return child;
                if (depth > 0)
                {
                    foreach (var descendent in Descendents(child, --depth))
                        yield return descendent;
                }
            }
        }

        IEnumerable<DependencyObject> FindGridView(DependencyObject root)
        {
            return Descendents(root, 8);
        }
        void HasFocus(DependencyObject sender, ref bool hasfocus)
        {

            int length = VisualTreeHelper.GetChildrenCount(sender);
            for (int j = 0; j < length; j++)
            {
                FrameworkElement fe = VisualTreeHelper.GetChild(sender, j) as FrameworkElement;
                if (fe != null)
                {
                    Control ct = fe as Control;

                    if (ct != null && ct.FocusState != FocusState.Unfocused)
                    {
                        hasfocus = true;
                        return;
                    }

                    HasFocus(fe, ref hasfocus);
                }
            }
        }

        #region property


        internal double ButtonWidth
        {
            get { return (double)GetValue(ButtionWidthProperty); }
            set { SetValue(ButtionWidthProperty, value); }
        }

        internal static readonly DependencyProperty ButtionWidthProperty =
            DependencyProperty.Register("ButtonWidth", typeof(double), typeof(CascadingMenu), new PropertyMetadata(0.0));


        internal object subItemSource
        {
            get { return (object)GetValue(subItemSourceProperty); }
            set { SetValue(subItemSourceProperty, value); }
        }

        internal static readonly DependencyProperty subItemSourceProperty =
            DependencyProperty.Register("subItemSource", typeof(object), typeof(CascadingMenu), new PropertyMetadata(null));


        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(CascadingMenu), new PropertyMetadata(null));



        public string ItemsPath
        {
            get { return (string)GetValue(ItemsPathProperty); }
            set { SetValue(ItemsPathProperty, value); }
        }

        public static readonly DependencyProperty ItemsPathProperty =
            DependencyProperty.Register("ItemsPath", typeof(string), typeof(CascadingMenu), new PropertyMetadata(null));

        #endregion


    }

}
