using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WAAD.POC.ProductCatalog.UWP.Common
{
    public static class UIHelper
    {
        public static ScrollViewer FindChildScrollViewer(DependencyObject toSearch)
        {
            Queue<DependencyObject> children = new Queue<DependencyObject>();
            children.Enqueue(toSearch);
            while (children.Count != 0)
            {
                DependencyObject dequeued = children.Dequeue();
                if (dequeued is ScrollViewer)
                {
                    return (ScrollViewer)dequeued;
                }
                else
                {
                    for (int index = 0; index < VisualTreeHelper.GetChildrenCount(dequeued); index++)
                    {
                        children.Enqueue(VisualTreeHelper.GetChild(dequeued, index));
                    }
                }
            }
            return null;
        }

        public static object FindChildControl<T>(DependencyObject toSearch, string name)
        {
            Queue<DependencyObject> children = new Queue<DependencyObject>();
            children.Enqueue(toSearch);
            while (children.Count != 0)
            {
                DependencyObject dequeued = children.Dequeue();
                if (dequeued is T)
                {
                    if (dequeued is FrameworkElement)
                        if ((dequeued as FrameworkElement).Name == name)
                            return dequeued;
                }
                if (1 == 1)//else
                {
                    for (int index = 0; index < VisualTreeHelper.GetChildrenCount(dequeued); index++)
                    {
                        children.Enqueue(VisualTreeHelper.GetChild(dequeued, index));
                    }
                }
            }
            return null;
        }
    }
}
