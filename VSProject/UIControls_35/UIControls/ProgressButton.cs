using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace UIControls
{
    public enum State
    {
        Normal,
        Working
    }

    public class ProgressButton : Button
    {
        public static DependencyProperty FirstImageProperty =
             DependencyProperty.Register(
                 "FirstImage",
                 typeof(String),
                 typeof(ProgressButton));

        public static DependencyProperty SecondImageProperty =
            DependencyProperty.Register(
                "SecondImage",
                typeof(String),
                typeof(ProgressButton));

        public static DependencyProperty RotatedImageProperty =
            DependencyProperty.Register(
                "RotatedImage",
                typeof(String),
                typeof(ProgressButton));

        public static DependencyProperty CoverImageProperty =
            DependencyProperty.Register(
                "CoverImage",
                typeof(String),
                typeof(ProgressButton));


        public static DependencyProperty StateProperty =
            DependencyProperty.Register(
                "State",
                typeof(State),
                typeof(ProgressButton),
                new PropertyMetadata(State.Normal));


        static ProgressButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ProgressButton),
                new FrameworkPropertyMetadata(typeof(ProgressButton)));
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

        public String RotatedImage
        {
            get { return (String)GetValue(RotatedImageProperty); }
            set { SetValue(RotatedImageProperty, value); }
        }

        public String CoverImage
        {
            get { return (String)GetValue(CoverImageProperty); }
            set { SetValue(CoverImageProperty, value); }
        }

        public State State
        {
            get { return (State)GetValue(StateProperty); }
            set { SetValue(StateProperty, value);}
        }
    }
}
