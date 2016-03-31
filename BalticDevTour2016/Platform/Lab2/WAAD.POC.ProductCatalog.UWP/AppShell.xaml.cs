using System;
using System.Collections.Generic;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WAAD.POC.ProductCatalog.UWP.Common;
using WAAD.POC.ProductCatalog.UWP.View;

namespace WAAD.POC.ProductCatalog.UWP
{
    /// <summary>
    /// Host Page for Application.
    /// </summary>
    public sealed partial class AppShell 
    {

        bool _isWindowsPhone = ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        private Frame _contentFrame;

        public AppShell(Frame frame)
        {
            _contentFrame = frame;
            InitializeComponent();
            ShellSplitView.Content = frame;            

            var update = new Action(() =>
            {
                // update radiobuttons after frame navigates
                var type = frame.CurrentSourcePageType;
                foreach (var radioButton in AllRadioButtons(this))
                {
                    var target = radioButton.CommandParameter as NavType;
                    if (target == null)
                        continue;
                        radioButton.IsChecked = target.Type == type;
                }
                if (ShellSplitView.DisplayMode!=SplitViewDisplayMode.CompactInline)
                    ShellSplitView.IsPaneOpen = false;
                BackCommand.RaiseCanExecuteChanged();
            });
            frame.Navigated += (s, e) => update();
            Loaded += delegate
            {
                Current = this;
                update();
                if (_isWindowsPhone)
                    btnHamburger.Background = null;
            };

            SizeChanged += AppShell_SizeChanged;

            rbBack.Visibility = Visibility.Collapsed;
            btnBack.Visibility = (_isWindowsPhone) ? Visibility.Collapsed : Visibility.Visible;
  
            if (_isWindowsPhone)
            {
                HardwareButtons.BackPressed += delegate (object sender, BackPressedEventArgs e) { if (CanBack()) { e.Handled = true; ExecuteBack(); } };
                ShellSplitView.DisplayMode = SplitViewDisplayMode.Overlay;
            }
            else
            {
                ShellSplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
            }

            if (_isWindowsPhone)
            {
                btnHamburger.Foreground = new SolidColorBrush(Colors.Black);
            }

            DataContext = this;
        }

        private void AppShell_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (_isWindowsPhone)
            {
                ShellSplitView.DisplayMode = SplitViewDisplayMode.Overlay;
            }
            else
            {
                if (e.NewSize.Width >= 1300)
                {
                    if (ShellSplitView.DisplayMode != SplitViewDisplayMode.CompactInline)
                    {
                        ShellSplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                    }
                }
                else
                {
                    ShellSplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                }
            }

            if (_isWindowsPhone)
            {
                btnHamburger.Foreground = new SolidColorBrush(Colors.Black);
            }
        }




        // back
        Command _backCommand;
        public Command BackCommand { get { return _backCommand ?? (_backCommand = new Command(ExecuteBack, CanBack)); } }
        private bool CanBack()
        {
            return _contentFrame.CanGoBack;
        }
        private void ExecuteBack()
        {
            _contentFrame.GoBack();
        }

        // menu
        Command _menuCommand;
        public Command MenuCommand { get { return _menuCommand ?? (_menuCommand = new Command(ExecuteMenu)); } }
        private void ExecuteMenu()
        {
            ShellSplitView.IsPaneOpen = !ShellSplitView.IsPaneOpen;
        }

        // nav
        Command<NavType> _navCommand;

        public static AppShell Current = null;

        public Command<NavType> NavCommand { get { return _navCommand ?? (_navCommand = new Command<NavType>(ExecuteNav)); } }
        private void ExecuteNav(NavType navType)
        {
            var type = navType.Type;

            _contentFrame.Navigate(navType.Type, navType.Parameter);
            // when we nav home, clear history
            if (type == typeof(HomePage))
            {
                _contentFrame.BackStack.Clear();
                BackCommand?.RaiseCanExecuteChanged();
            }
        }

        // utility
        public List<RadioButton> AllRadioButtons(DependencyObject parent)
        {
            var list = new List<RadioButton>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is RadioButton)
                {
                    list.Add(child as RadioButton);
                    continue;
                }
                list.AddRange(AllRadioButtons(child));
            }
            return list;
        }

        // prevent check
        private void DontCheck(object s, RoutedEventArgs e)
        {
            // don't let the radiobutton check
            (s as RadioButton).IsChecked = false;
        }
    }

    public class NavType
    {
        public Type Type { get; set; }
        public string Parameter { get; set; }
    }
}
