using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace UIControls
{
    public enum SelectedState
    {
        YES,
        NO
    }

    public class StateTextBlock : ContentControl
    {
        public static DependencyProperty SelectedStateProperty =
            DependencyProperty.Register(
                "SelectedState",
                typeof(SelectedState),
                typeof(StateTextBlock),
                new PropertyMetadata(SelectedState.NO));

        public static DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(String),
                typeof(StateTextBlock));


        static StateTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(StateTextBlock),
                new FrameworkPropertyMetadata(typeof(StateTextBlock)));
        }

        public SelectedState SelectedState
        {
            get { return (SelectedState)GetValue(SelectedStateProperty); }
            set { SetValue(SelectedStateProperty, value); }
        }

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

    }
}
