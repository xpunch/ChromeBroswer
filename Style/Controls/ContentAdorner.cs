using System.Collections;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Style.Controls
{
    public class ContentAdorner : Adorner
    {
        private readonly FrameworkElement _child;

        public ContentAdorner(FrameworkElement adornedElement, FrameworkElement adornedContent)
            : base(adornedElement)
        {
            this._child = adornedContent;

            adornedElement.SizeChanged += (_, __) => this.InvalidateMeasure();

            this.AddLogicalChild(this._child);
            this.AddVisualChild(this._child);
        }

        public new FrameworkElement AdornedElement
        {
            get
            {
                return (FrameworkElement)base.AdornedElement;
            }
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                var list = new ArrayList { this._child };
                return list.GetEnumerator();
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        public void DisconnectChild()
        {
            this.RemoveLogicalChild(this._child);
            this.RemoveVisualChild(this._child);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this._child.Arrange(new Rect(0, 0, this.AdornedElement.ActualWidth, this.AdornedElement.ActualHeight));

            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return this._child;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this._child.Measure(constraint);
            return this._child.DesiredSize;
        }
    }
}