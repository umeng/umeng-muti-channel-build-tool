using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace FileTextBox
{
    class FileTextBox : TextBox
    {
        //Hint Text
        public static DependencyProperty LabelTextProperty =
            DependencyProperty.Register(
                "LabelText",
                typeof(string),
                typeof(FileTextBox));
        //Hint Text Color
        public static DependencyProperty LabelTextColorProperty =
           DependencyProperty.Register(
               "LabelTextColor",
               typeof(Brush),
               typeof(FileTextBox));
        //Browser Icon
        public static DependencyProperty BrowserIconProperty =
            DependencyProperty.Register(
                "BrowserIcon",
                typeof(string),
                typeof(FileTextBox));

        private static DependencyPropertyKey HasTextPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "HasText",
                typeof(bool),
                typeof(FileTextBox),
                new PropertyMetadata());
        public static DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        //Browser File Event
        public static readonly RoutedEvent BrowserEvent =
            EventManager.RegisterRoutedEvent(
                "Browser",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(FileTextBox));

        static FileTextBox() 
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(FileTextBox),
                new FrameworkPropertyMetadata(typeof(FileTextBox)));
        }

        public FileTextBox() : base() {}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Border iconBorder = GetTemplateChild("PART_BrowserIconBorder") as Border;

            if (iconBorder != null)
            {
                iconBorder.MouseLeftButtonDown += new MouseButtonEventHandler(IconBorder_MouseLeftButtonDown);
            }
        }

        private void IconBorder_MouseLeftButtonDown(object obj, MouseButtonEventArgs e)
        {
            //Raise browser file event
            RaiseSearchEvent();
        }

        private void RaiseSearchEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(BrowserEvent);
            RaiseEvent(args);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            HasText = Text.Length != 0;
        }

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public Brush LabelTextColor
        {
            get { return (Brush)GetValue(LabelTextColorProperty); }
            set { SetValue(LabelTextColorProperty, value); }
        }

        public string BrowserIcon
        {
            get { return (string)GetValue(BrowserIconProperty); }
            set { SetValue(BrowserIconProperty, value); }
        }

        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            private set { SetValue(HasTextPropertyKey, value); }
        }

        public event RoutedEventHandler Browser
        {
            add { AddHandler(BrowserEvent, value); }
            remove { RemoveHandler(BrowserEvent, value); }
        }
    }
}
