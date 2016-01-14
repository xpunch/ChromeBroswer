// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using Common.Utils;
using log4net;
using Application = System.Windows.Application;
using Point = System.Drawing.Point;

namespace BrowserClient.Handlers
{
    internal class MenuHandler : IMenuHandler
    {
        private readonly ILog _loggor = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool OnBeforeContextMenu(IWebBrowser browserControl, IContextMenuParams parameters)
        {
            var chromiumWebBrowser = browserControl as ChromiumWebBrowser;
            if (chromiumWebBrowser != null)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        var customContextMenu = chromiumWebBrowser.ContextMenu;
                        customContextMenu.MenuItems.Clear();
                        if (parameters.IsEditable)
                        {
                            customContextMenu.MenuItems.Add(new MenuItem("撤销", (s, e) => { chromiumWebBrowser.Undo(); }));
                            customContextMenu.MenuItems.Add(new MenuItem("重做", (s, e) => { chromiumWebBrowser.Redo(); }));
                            customContextMenu.MenuItems.Add(new MenuItem("-"));
                        }
                        /*if (parameters.HasImageContents)
                        {
                            customContextMenu.MenuItems.Add(new MenuItem("图片另存为", (s, e) => { chromiumWebBrowser.Copy(); }));
                            customContextMenu.MenuItems.Add(new MenuItem("-"));
                        }*/
                        customContextMenu.MenuItems.Add(new MenuItem("剪切", (s, e) => { chromiumWebBrowser.Cut(); }));
                        customContextMenu.MenuItems.Add(new MenuItem("复制", (s, e) => { chromiumWebBrowser.Copy(); }));
                        customContextMenu.MenuItems.Add(new MenuItem("粘贴", (s, e) => { chromiumWebBrowser.Paste(); }));
                        customContextMenu.MenuItems.Add(new MenuItem("-"));
                        customContextMenu.MenuItems.Add(new MenuItem("后退", (s, e) => { chromiumWebBrowser.Back(); })
                        {
                            Enabled = chromiumWebBrowser.CanGoBack
                        });
                        customContextMenu.MenuItems.Add(new MenuItem("前进", (s, e) => { chromiumWebBrowser.Forward(); })
                        {
                            Enabled = chromiumWebBrowser.CanGoForward
                        });
                        customContextMenu.MenuItems.Add(new MenuItem("刷新", (s, e) => { chromiumWebBrowser.Reload(); })
                        {
                            Enabled = chromiumWebBrowser.CanReload
                        });
                        /*customContextMenu.MenuItems.Add(new MenuItem("-"));
                        customContextMenu.MenuItems.Add(new MenuItem("缩小",
                            (s, e) =>
                            {
                                chromiumWebBrowser.ZoomLevel /= 1.2;
                            }));
                        customContextMenu.MenuItems.Add(new MenuItem("放大",
                            (s, e) => { chromiumWebBrowser.ZoomLevel *= 1.2; }));
                        customContextMenu.MenuItems.Add(new MenuItem("默认显示大小",
                            (s, e) =>
                            {
                                chromiumWebBrowser.ZoomLevel = 1;
                            }));*/
                        customContextMenu.MenuItems.Add(new MenuItem("-"));
                        customContextMenu.MenuItems.Add(new MenuItem("打印", (s, e) => { chromiumWebBrowser.Print(); }));

                        Point point;
                        if (MouseHelper.GetCursorPos(out point))
                        {
                            int top = 0, left = 0;
                            if (WindowState.Normal.Equals(Application.Current.MainWindow.WindowState))
                            {
                                top = (int) Application.Current.MainWindow.Top + 20;
                                left = (int) Application.Current.MainWindow.Left + 8;
                            }
                            customContextMenu.Show(chromiumWebBrowser, new Point(point.X - left, point.Y - top - 62));
                        }
                    }
                    catch (Exception exception)
                    {
                        _loggor.Error(exception);
                    }
                }));
            }
            return false;
        }
    }
}