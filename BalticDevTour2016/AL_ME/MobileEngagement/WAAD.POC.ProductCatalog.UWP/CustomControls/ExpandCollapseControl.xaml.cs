using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace WAAD.POC.ProductCatalog.UWP.CustomControls
{
    public sealed partial class ExpandCollapseControl 
    {
        private readonly Guid _controlId;

        public ExpandCollapseControl()
        {
            InitializeComponent();
            _controlId = Guid.NewGuid();
        }

        public Guid ControlId => _controlId;



        public bool IsCollapsed
        {
            get
            {
                return (CollapsedSection.Visibility == Visibility.Collapsed);
            }
            set
            {
                SetCollapsed(value);
            }


        }

        // Using a DependencyProperty as the backing store for IsCollapsed.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty IsCollapsedProperty =
        //    DependencyProperty.Register("IsCollapsed", typeof(bool), typeof(ExpandCollapseControl), new PropertyMetadata(false));

        


        /// <summary>
        /// Represents the event to be raised whenever the control is expanded.
        /// </summary>
        public event EventHandler Expanded;

        /// <summary>
        /// Represents the boolean flag that indicates whether control is collapsed or not.
        /// </summary>
        //public bool IsCollapsed
        //{
        //    get
        //    {
        //        return (CollapsedSection.Visibility == Visibility.Collapsed);
        //    }
        //    set
        //    {
        //        this.SetCollapsed(value);
        //    }
        //}

        /// <summary>
        /// Represents the property to get/set the Header Content.
        /// </summary>
        public object HeaderContent
        {
            get
            {
                return HeaderContentControl.Content;
            }
            set
            {
                HeaderContentControl.Content = value;
            }
        }

        /// <summary>
        /// Represents the property to get/set the Body Content.
        /// </summary>
        public object BodyContent
        {
            get
            {
                return BodyContentControl.Content;
            }
            set
            {
                BodyContentControl.Content = value;
            }
        }

        /// <summary>
        /// Event handler for the header tapped event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollapseTabbed(object sender, TappedRoutedEventArgs e)
        {
            SetCollapsed(CollapsedSection.Visibility == Visibility.Visible);
        }

        public void SetCollapsed(bool isCollapsed)
        {
            //Raise the event
            if (Expanded != null && isCollapsed == false)
                Expanded(this, new EventArgs());
            
            //Update the state
            VisualStateManager.GoToState(this, isCollapsed ? "Up" : "Down", true);
        }
    }
    public class ExpandedEventArgs : EventArgs
    {
        public Guid ControlId { get; set; }
    }
}
