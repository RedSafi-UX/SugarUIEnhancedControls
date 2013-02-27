using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Controls.Toolkit
{
    [TemplatePart(Name = SemanticZoomOutColumn.ElementItemsCountName, Type = typeof(TextBlock))]
    [TemplatePart(Name= SemanticZoomOutColumn.ElementTextBlockTextName,Type=typeof(TextBlock))]
    [TemplatePart(Name=SemanticZoomOutColumn.ElementEdgePanelName,Type=typeof(EdgePanel))]
    public class SemanticZoomOutColumn : SemanticZoomOutItemBase
    {
        private const string ElementTextBlockTextName = "TextBlockText";
        private const string ElementItemsCountName = "ItemsCount";
        private const string ElementEdgePanelName = "EdgePanel";

        public SemanticZoomOutColumn()
        {
            this.DefaultStyleKey = typeof(SemanticZoomOutColumn);
        }

        private EdgePanel CurrentEdgePanel
        {
            get;
            set;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CurrentEdgePanel = this.GetTemplateChild(ElementEdgePanelName) as EdgePanel;           
        }

        public override void OnDrawEdgePanel()
        {
            base.OnDrawEdgePanel();
            if (this.ControlsOwner != null)
            {
                int maxCount = this.ControlsOwner.MaxGroupItemsCount;

                if (maxCount != 0)
                {
                    this.CurrentEdgePanel.Height = ((float)this.Count / (float)maxCount) * this.Height;
                }
            }
        }

    }
}
