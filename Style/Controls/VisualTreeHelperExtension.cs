using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Style.Controls
{
    /// <summary>
    /// The custom extensions.
    /// </summary>
    public static class VisualTreeHelperExtension
    {
        /// <summary>
        /// Help you find the child element from specified Type of one control in visula tree.
        /// </summary>
        /// <param name="reference">
        /// The Source control.
        /// </param>
        /// <typeparam name="T">
        /// The specified Type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IList{T}"/> child Elements founded.
        /// </returns>
        public static IList<T> FindChildElements<T>(this DependencyObject reference) where T : FrameworkElement
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }

            var childList = new List<T>();

            /*for (var index = 0; index < VisualTreeHelper.GetChildrenCount(reference); index++)
            {
                var child = VisualTreeHelper.GetChild(reference, index);
                if (child is T)
                {
                    childList.Add((T)child);
                }

                childList.AddRange(child.FindChildElements<T>());
            }*/

            childList.Clear();
            foreach (var child in LogicalTreeHelper.GetChildren(reference))
            {
                if (child is T)
                {
                    childList.Add((T)child);

                    childList.AddRange(FindChildElements<T>(((T)child)));
                }
            }

            return childList;
        }

        /// <summary>
        /// Help you find the child element with speified Type and name in visual tree.
        /// </summary>
        /// <param name="reference">
        /// The source control.
        /// </param>
        /// <param name="targetName">
        /// The target child element's name.
        /// </param>
        /// <typeparam name="T">
        /// The specified Type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/> child elment founded, if not founded return null.
        /// </returns>
        public static T FindChildElement<T>(this DependencyObject reference, string targetName)
            where T : FrameworkElement
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }

            for (var index = 0; index < VisualTreeHelper.GetChildrenCount(reference); index++)
            {
                var child = VisualTreeHelper.GetChild(reference, index);

                if (child is T && ((T)child).Name.Equals(targetName ?? string.Empty))
                {
                    return (T)child;
                }

                var grandChild = FindChildElement<T>(child, targetName);
                if (grandChild != null)
                {
                    return grandChild;
                }
            }

            return null;
        }

        /// <summary>
        /// Help you find ancestor element with specified Type and name.
        /// </summary>
        /// <param name="sourceElement">
        /// The source control.
        /// </param>
        /// <param name="parentName">
        /// The specified ancestor control's name.
        /// </param>
        /// <typeparam name="T">
        /// The specified Type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/> ancestor element that founded, if not found return null.
        /// </returns>
        public static T FindAncestorElement<T>(this DependencyObject sourceElement, string parentName = null)
            where T : FrameworkElement
        {
            if (sourceElement == null)
            {
                throw new ArgumentNullException("sourceElement");
            }

            var parent = GetParent(sourceElement);

            if (parent != null)
            {
                if (parent is T && (parentName == null || parentName.Equals(((T)parent).Name)))
                {
                    return (T)parent;
                }

                return FindAncestorElement<T>(parent, parentName);
            }

            return null;
        }

        private static DependencyObject GetParent(DependencyObject sourceElement)
        {
            DependencyObject parent = null;
#if SILVERLIGHT
            FrameworkElement frameworkElement = source as FrameworkElement;
            if (frameworkElement != null)
            {
                parent = frameworkElement.Parent ?? System.Windows.Media.VisualTreeHelper.GetParent(frameworkElement);
            }
#else
            parent = LogicalTreeHelper.GetParent(sourceElement);
#endif
            return parent;
        }
    }
}