using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace UIControls
{
    public class HintTextBox : TextBox
    {
        public static DependencyProperty HintTextProperty =
            DependencyProperty.Register(
                "HintText",
                typeof(String),
                typeof(HintTextBox));

        private static DependencyPropertyKey HasTextPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "HasText",
                typeof(bool),
                typeof(HintTextBox),
                new PropertyMetadata());
        public static DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        static HintTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(HintTextBox),
                new FrameworkPropertyMetadata(typeof(HintTextBox)));
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            HasText = Text.Length != 0;
        }

        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            private set { SetValue(HasTextPropertyKey, value); }
        }

        public String HintText
        {
            get { return (String)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

    }
}
