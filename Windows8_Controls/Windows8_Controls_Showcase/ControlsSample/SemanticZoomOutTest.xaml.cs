using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows8_Controls_Showcase.Views;

namespace Windows8_Controls_Showcase.ControlsSample
{
    public class SecondaryType
    {
        public int TypeID { set; get; }

        public int ProID { set; get; }

        public string TypeName { set; get; }

        public Uri ImageUrl { set; get; }
    }

    public class MainType
    {
        public int ProID { set; get; }

        public string Name { set; get; }
    }

    public sealed partial class SemanticZoomOutTest : Page
    {
        public SemanticZoomOutTest()
        {
            this.InitializeComponent();
        }
      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainType[] MainTypes = { 
                                    new MainType{ProID=1,Name="News"},
                                    new MainType{ProID=2,Name="Movie"},
                                    new MainType{ProID=3,Name="Sports"},
                                    new MainType{ProID=4,Name="Games"}
                              };

            SecondaryType[] SecondaryTypes = { 
                                new SecondaryType{TypeID=11,ProID=1,TypeName="News 1"},
                                new SecondaryType{TypeID=12,ProID=1,TypeName="News 2"},
                                new SecondaryType{TypeID=13,ProID=1,TypeName="News 3"},
                                new SecondaryType{TypeID=21,ProID=2,TypeName="Batman"},
                                new SecondaryType{TypeID=22,ProID=2,TypeName="SpiderMan"},
                                new SecondaryType{TypeID=31,ProID=3,TypeName="Basketball"},
                                new SecondaryType{TypeID=32,ProID=3,TypeName="Swimming"},
                                new SecondaryType{TypeID=33,ProID=3,TypeName="Football"},
                                new SecondaryType{TypeID=34,ProID=3,TypeName="Fencing"},
                                new SecondaryType{TypeID=35,ProID=3,TypeName="Weightlifting"},
                                new SecondaryType{TypeID=41,ProID=4,TypeName="WOW"},
                                new SecondaryType{TypeID=42,ProID=4,TypeName="StarCraft"}
                           };
           
            var res = (from p in MainTypes
                       join c in SecondaryTypes on p.ProID equals c.ProID
                       into g
                       select new
                       {
                           p.Name,
                           TypeList = g.ToList()
                       }).ToList<dynamic>();
           
            CollectionViewSource cvs = new CollectionViewSource();
            cvs.IsSourceGrouped = true;

            cvs.ItemsPath = new PropertyPath("TypeList");
             
            cvs.Source = res;
            
            gvList.ItemsSource = cvs.View.CollectionGroups;
            lvlist.ItemsSource = cvs.View;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            App.NavigateToPage(typeof(MainHub));
        }
    }
}
