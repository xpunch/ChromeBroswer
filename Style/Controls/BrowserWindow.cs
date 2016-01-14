using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Style.Common;

namespace Style.Controls
{
    /// <summary>
    ///     Class which provides the implementation of a custom window
    /// </summary>
    [TemplatePart(Name = "PART_Minimize", Type = typeof (Button))]
    [TemplatePart(Name = "PART_Maximize", Type = typeof (Button))]
    [TemplatePart(Name = "PART_Close", Type = typeof (Button))]
    public class BrowserWindow : System.Windows.Window
    {
        private Button _closeButton;
        private Button _maximizeButton;
        private Button _minimizeButton;
        private Border _captionControl;

        /// <summary>
        /// Override which is called when the template is applied
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            // Get all the controls in the template
            GetTemplateParts();
        }

        protected void GetTemplateParts()
        {
            // PART_Minimize
            _minimizeButton = GetChildControl<Button>("PART_Minimize");
            if (_minimizeButton != null)
            {
                _minimizeButton.Click += OnMinimize;
            }

            // PART_Maximize
            _maximizeButton = GetChildControl<Button>("PART_Maximize");
            if (_maximizeButton != null)
            {
                _maximizeButton.Click += OnMaximize;
            }

            // PART_Close
            _closeButton = GetChildControl<Button>("PART_Close");
            if (_closeButton != null)
            {
                _closeButton.Click += OnClose;
            }

            //Caption
            _captionControl = GetChildControl<Border>("PART_Caption");
            if (null != _captionControl)
            {
                _captionControl.MouseDown += CaptionControlMouseDown;
            }
        }

        void CaptionControlMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
            }
        }

        /// <summary>
        /// Adjusts the WindowSize to correct parameters when Maximize button is clicked
        /// </summary>
        private void AdjustWindowSize()
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        /// <summary>
        ///     Generic method to get a control from the template
        /// </summary>
        /// <typeparam name="T">Type of the control</typeparam>
        /// <param name="ctrlName">Name of the control in the template</param>
        /// <returns>Control</returns>
        protected T GetChildControl<T>(string ctrlName) where T : DependencyObject
        {
            var ctrl = GetTemplateChild(ctrlName) as T;
            return ctrl;
        }

        /// <summary>
        ///     Overridable event handler for the event raised when Minimize button is clicked
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">RoutedEventArgs</param>
        protected virtual void OnMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        ///     Overridable event handler for the event raised when Maximize button is clicked
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">RoutedEventArgs</param>
        protected virtual void OnMaximize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        /// <summary>
        ///     Overridable event handler for the event raised when Close button is clicked
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">RoutedEventArgs</param>
        protected virtual void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            GlassHelper.ExtendGlassFrame(this, new Thickness(-1));
        }
    }
}