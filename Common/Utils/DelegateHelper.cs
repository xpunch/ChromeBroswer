using System;
using System.Windows;
using System.Windows.Threading;

namespace Common.Utils
{
    /// <summary>
    ///     用于执行委托操作（同步、异步）
    /// </summary>
    public class DelegateHelper
    {
        public static Dispatcher UiDispatcher = Application.Current.Dispatcher;

        public static void Invoke(DispatcherPriority dispatcherPriority, Action action)
        {
            if (null != action)
            {
                UiDispatcher.Invoke(dispatcherPriority, action);
            }
        }

        public static void RunAsyncAction(Action action, Action callbackAction)
        {
            if (null != action && null != callbackAction)
            {
                action.BeginInvoke(ar => UiDispatcher.BeginInvoke(callbackAction, null), null);
            }
        }

        public static void RunAsyncAction<T>(T parameter, Action<T> action, Action<T> callbackAction)
        {
            if (null != action && null != callbackAction)
            {
                action.BeginInvoke(parameter, ar => UiDispatcher.BeginInvoke(callbackAction, ar.AsyncState),
                    parameter);
            }
        }

        public static void RunAsyncAction<T1, T2>(T1 parameter1, T2 parameter2, Action<T1, T2> action,
            Action<T1> callbackAction)
        {
            if (null != action && null != callbackAction)
            {
                action.BeginInvoke(parameter1, parameter2,
                    ar => UiDispatcher.BeginInvoke(callbackAction, ar.AsyncState),
                    parameter1);
            }
        }

        public static void RunAsyncAction<T1, T2, T3>(T1 parameter1, T2 parameter2, T3 parameter3,
            Action<T1, T2, T3> action, Action<T1> callbackAction)
        {
            if (null != action && null != callbackAction)
                action.BeginInvoke(parameter1, parameter2, parameter3,
                    ar => UiDispatcher.BeginInvoke(callbackAction, ar.AsyncState),
                    parameter1);
        }
    }
}