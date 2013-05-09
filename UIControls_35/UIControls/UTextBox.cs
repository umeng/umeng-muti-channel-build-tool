﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace UIControls
{
    public enum EditState
    {
        Normal,
        Locked,
        Editable,
    }

    public class UTextBox: TextBox
    {
        public static DependencyProperty EditStateProperty =
            DependencyProperty.Register(
                "EditState",
                typeof(EditState),
                typeof(UTextBox),
                new PropertyMetadata(EditState.Normal));

        public static DependencyProperty SharedTextProperty =
            DependencyProperty.Register(
                "SharedText",
                typeof(String),
                typeof(UTextBox));

        public static readonly RoutedEvent ItemEditCompleteEvent =
            EventManager.RegisterRoutedEvent(
                "ItemEditComplete",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(UTextBox));

        public static readonly RoutedEvent ItemDeleteEvent =
            EventManager.RegisterRoutedEvent(
                "ItemDelete",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(UTextBox));

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                var tb = GetTemplateChild("EditText") as TextBox;

                if (tb != null)
                {
                    SharedText = tb.Text;

                    ItemEditCompleteEventArgs args = new ItemEditCompleteEventArgs(ItemEditCompleteEvent, SharedText);
                    RaiseEvent(args);

                    tb.Text = "";
                }
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        /// <summary>
        /// DefaultStyleKeyProperty defines the key used to find the theme style of the control. 
        /// If you comment out the line, you will end up with the default theme style of the base class.
        /// </summary>
        static UTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(UTextBox),
                new FrameworkPropertyMetadata(typeof(UTextBox)));
        }

        public UTextBox(): base()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var button = GetTemplateChild("DeleteButton") as Button;

            if (button != null)
            {
                button.Click += new RoutedEventHandler(button_Click);
            }
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            ItemEditCompleteEventArgs args = new ItemEditCompleteEventArgs(ItemDeleteEvent, SharedText);
            RaiseEvent(args);
        }

        public EditState EditState
        {
            get { return (EditState)GetValue(EditStateProperty); }
            set { SetValue(EditStateProperty, value); }
        }

        public String SharedText
        {
            get { return (String)GetValue(SharedTextProperty); }
            set { SetValue(SharedTextProperty, value); }
        }

        public event RoutedEventHandler ItemEditComplete
        {
            add { AddHandler(ItemEditCompleteEvent, value); }
            remove { RemoveHandler(ItemEditCompleteEvent, value); }
        }

        public event RoutedEventHandler ItemDelete
        {
            add { AddHandler(ItemDeleteEvent, value); }
            remove { RemoveHandler(ItemDeleteEvent, value); }
        }
    }

    public class ItemEditCompleteEventArgs : RoutedEventArgs
    {
        public String Content { get; set; }

        public ItemEditCompleteEventArgs(RoutedEvent re, String content)
            : base(re)
        {
            Content = content;
        }
    }
}
