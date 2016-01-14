using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Style.Controls
{
    public class BusyLoading : ContentControl
    {
        public static readonly DependencyProperty AdornerdContentProperty = DependencyProperty.Register(
            "AdornedContent",
            typeof (FrameworkElement),
            typeof (BusyLoading),
            new PropertyMetadata(null, OnAdornedContentPropertyChangedCallback));

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy", typeof (bool), typeof (BusyLoading),
            new PropertyMetadata(false, OnIsBusyPropertyChangedCallback));

        private ContentAdorner _adorner;

        public BusyLoading()
        {
            AdornedContent = new Loading();
        }

        public FrameworkElement AdornedContent
        {
            get { return (FrameworkElement) GetValue(AdornerdContentProperty); }

            set { SetValue(AdornerdContentProperty, value); }
        }

        public bool IsBusy
        {
            get { return (bool) GetValue(IsBusyProperty); }

            set { SetValue(IsBusyProperty, value); }
        }

        private static void OnAdornedContentPropertyChangedCallback(
            DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
        }

        private static void OnIsBusyPropertyChangedCallback(
            DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var self = (BusyLoading) sender;
            var busy = (bool) args.NewValue;

            if (busy)
            {
                self.ShowAdorner();
            }
            else
            {
                self.HideAdorner();
            }
        }

        private void HideAdorner()
        {
            if (_adorner == null)
            {
                return;
            }

            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(_adorner);
                _adorner.DisconnectChild();
                _adorner = null;
            }
        }

        private void ShowAdorner()
        {
            var parent = Parent as Panel;

            _adorner = new ContentAdorner(parent, AdornedContent);
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (adornerLayer != null)
            {
                adornerLayer.Add(_adorner);
            }
        }
    }
}