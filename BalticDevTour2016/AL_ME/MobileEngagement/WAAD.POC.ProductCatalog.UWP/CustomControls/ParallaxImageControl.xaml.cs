using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WAAD.POC.ProductCatalog.UWP.CustomControls
{
    public sealed partial class ParallaxImageControl
    {

        double _scrollMultiplierAmount = 0.5;
        double _contentHeight = 0.0;
        double _controlHeight = 0.0;
        ScrollViewer _parentScrollViewer;

        public ParallaxImageControl()
        {
            InitializeComponent();
            Loaded += ParallaxImageControl_Loaded;
            Unloaded += ParallaxImageControl_Unloaded;
            SizeChanged += (s, e) =>
            {
                if (e?.NewSize != null)
                    if (!e.NewSize.IsEmpty)
                        if (!double.IsNaN(e.NewSize.Height))
                        {
                            _controlHeight = Math.Max(e.NewSize.Height, 0.0);
                        }
            };
            ScrollContentPresenter.SizeChanged += (s, e) => 
            {
                if (e != null)
                    if (!double.IsNaN(ScrollContentPresenter.ActualHeight))
                            {
                                _contentHeight = Math.Max(ScrollContentPresenter.ActualHeight, 0.0);
                            }
            };
        }

        public UIElement ScrollContent
        {
            get { return (UIElement)GetValue(ScrollContentProperty); }
            set { SetValue(ScrollContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollContentProperty =
            DependencyProperty.Register("ScrollContent", typeof(UIElement), typeof(HeaderControl), new PropertyMetadata(DependencyProperty.UnsetValue));

        private void ParallaxImageControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_parentScrollViewer != null)
                {
                    _parentScrollViewer.ViewChanged -= _parentScrollViewer_ViewChanged;
                    _parentScrollViewer = null;
                }
            }
            catch
            {
            }
        }

        private void ParallaxImageControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _parentScrollViewer = LocateParentScrollViewer(this);
                _parentScrollViewer.ViewChanged += _parentScrollViewer_ViewChanged;
            }
            catch
            {
            }
        }

        private void _parentScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            try
            {
                if (_parentScrollViewer!=null)
                {
                    if (_contentHeight > 0.0)
                        if (_controlHeight > 0.0)
                        {
                            double vOffset = _parentScrollViewer.VerticalOffset;
                            if (vOffset > 0.0)
                            {
                                double maxScrollHeight = Math.Max(_contentHeight - _controlHeight, 0.0);
                                if (maxScrollHeight > 0.0)
                                {
                                    double translateHeight = _parentScrollViewer.VerticalOffset * _scrollMultiplierAmount;
                                    ScrollContentTransform.TranslateY = translateHeight <= maxScrollHeight ? translateHeight : maxScrollHeight;
                                    return;
                                }
                            }
                        }
                }
                ScrollContentTransform.TranslateY = 0.0;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private ScrollViewer LocateParentScrollViewer(DependencyObject element)
        {
            ScrollViewer result = null;
            try
            {
                DependencyObject obj = VisualTreeHelper.GetParent(element);
                bool bDone = obj == null;
                while (!bDone)
                {
                    if (obj is ScrollViewer)
                    {
                        result = (obj as ScrollViewer);
                        bDone = true;
                    }
                    else
                    {
                        obj = VisualTreeHelper.GetParent(obj);
                        bDone = (obj == null);
                    }
                }
            }
            catch
            { }
            return result;
        }

        
    }
}
