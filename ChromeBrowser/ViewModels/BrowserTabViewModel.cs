// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CefSharp;
using CefSharp.Wpf;
using ChromeBrowser.Common;

namespace ChromeBrowser.ViewModels
{
    public class BrowserTabViewModel  : INotifyPropertyChanged, IDisposable
    {
        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        private string _addressEditable;
        public string AddressEditable
        {
            get { return _addressEditable; }
            set
            {
                _addressEditable = value;
                OnPropertyChanged("AddressEditable");
            }
        }

        private string _outputMessage;
        public string OutputMessage
        {
            get { return _outputMessage; }
            set { _outputMessage=value;OnPropertyChanged("OutputMessage"); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { _statusMessage=value; OnPropertyChanged("StatusMessage");}
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title=value; OnPropertyChanged("Title");}
        }

        private IWpfWebBrowser _webBrowser;
        public IWpfWebBrowser WebBrowser
        {
            get { return _webBrowser; }
            set { _webBrowser=value; OnPropertyChanged("WebBrowser");}
        }

        public ICommand JumpCommand { get; private set; }
        public ICommand HomePageCommand { get; private set; }

        public BrowserTabViewModel(string address)
        {
            AddressEditable = Address = address;
            Title = "新页面";

            JumpCommand = new RelayCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
            HomePageCommand = new RelayCommand(() => AddressEditable = Address = App.DefaultUrl);

            PropertyChanged += OnPropertyChanged;

            OutputMessage = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Address":
                    AddressEditable = Address;
                    break;

                case "WebBrowser":
                    if (WebBrowser != null)
                    {
                        WebBrowser.FrameLoadStart +=
                            delegate
                            {
                                Application.Current.Dispatcher.BeginInvoke((Action) (() => Title = "正在打开新页面..."));
                            };
                        WebBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
                        WebBrowser.StatusMessage += OnWebBrowserStatusMessage;
                        WebBrowser.LoadError += OnWebBrowserLoadError;
                        //WebBrowser.FrameLoadEnd += delegate { Application.Current.Dispatcher.BeginInvoke((Action)(() => _webBrowser.Focus())); };
                    }

                    break;
            }
        }

        private void OnWebBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            OutputMessage = e.Message;
        }

        private void OnWebBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
            StatusMessage = e.Value;
        }

        private void OnWebBrowserLoadError(object sender, LoadErrorEventArgs args)
        {
            // Don't display an error for downloaded files where the user aborted the download.
            if (args.ErrorCode == CefErrorCode.Aborted)
                return;

            var errorMessage = "<html><body><h2>Failed to load URL " + args.FailedUrl +
                  " with error " + args.ErrorText + " (" + args.ErrorCode +
                  ").</h2></body></html>";

            _webBrowser.LoadHtml(errorMessage, args.FailedUrl);
        }

        private void Go()
        {
            Address = AddressEditable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (null != WebBrowser)
            {
                _webBrowser.Dispose();
            }
        }
    }
}
