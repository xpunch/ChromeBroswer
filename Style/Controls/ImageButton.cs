using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Style.Controls
{
    /// <summary>
    ///     图片切换的按钮
    /// </summary>
    public class ImageButton : Button
    {
        /// <summary>
        ///     默认图片路径
        /// </summary>
        public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register("DefaultImage",
            typeof (ImageSource), typeof (ImageButton), new PropertyMetadata(null));

        /// <summary>
        ///     图片路径
        /// </summary>
        public static readonly DependencyProperty HoverImageProperty = DependencyProperty.Register("HoverImage",
            typeof (ImageSource), typeof (ImageButton), new PropertyMetadata(null));

        /// <summary>
        ///     默认图片路径
        /// </summary>
        public ImageSource DefaultImage
        {
            get { return (ImageSource) GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        /// <summary>
        ///     图片路径
        /// </summary>
        public ImageSource HoverImage
        {
            get { return (ImageSource) GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }
    }
}