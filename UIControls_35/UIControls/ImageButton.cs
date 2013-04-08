using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace UIControls
{
    public class ImageButton : Button
    {
        public static DependencyProperty FirstImageProperty =
            DependencyProperty.Register(
                "FirstImage",
                typeof(String),
                typeof(ImageButton));

        public static DependencyProperty SecondImageProperty =
            DependencyProperty.Register(
                "SecondImage",
                typeof(String),
                typeof(ImageButton));

        public static DependencyProperty StretchProperty =
            DependencyProperty.Register(
                "Stretch",
                typeof(System.Windows.Media.Stretch),
                typeof(ImageButton));

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ImageButton),
                new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public String FirstImage
        {
            get { return (String)GetValue(FirstImageProperty); }
            set { SetValue(FirstImageProperty, value); }
        }

        public String SecondImage
        {
            get { return (String)GetValue(SecondImageProperty); }
            set { SetValue(SecondImageProperty, value); }
        }

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
    }
}
