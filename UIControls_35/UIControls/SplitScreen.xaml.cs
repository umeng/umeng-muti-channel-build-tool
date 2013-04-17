using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xaml;
using System.Windows.Media.Animation;

namespace UIControls
{
    /// <summary>
    /// Interaction logic for SplitScreen.xaml
    /// </summary>
    public partial class SplitScreen : UserControl
    {
        private Point buttonNewPoint; // button's new place
        private Point buttonOldPoint; // button's old place

        
        private double splitLine;     // where the screen split
        private double interval = 10; // interval between splitline and button.bottom

        private Button button;
        private Storyboard sb = new Storyboard();
        private bool isEnd ; // animation start or end

        public SplitScreen()
        {
            InitializeComponent();

            sb.Completed += new EventHandler(sb_Completed);
            isEnd = false;
        }

        //private double newTop = 50;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin">which raise the event</param>
        /// <param name="dst">new postion for the original control</param>
        /// <param name="screen">backgroud image split</param>
        public SplitScreen(Button origin,Point dst, Grid screen): this()
        {
            //copy button
            buttonOldPoint = origin.TranslatePoint(new Point(0, 0), screen);

            //TO-DO add some logic
            buttonNewPoint.X = buttonOldPoint.X ;

            if (buttonNewPoint.Y > 50)
            {
                buttonNewPoint.Y = 50;
            }

            splitLine = buttonOldPoint.Y + interval + origin.Height;

            button = new Button();

            button.Content = origin.Content;
            button.Width = origin.Width;
            button.Height = origin.Height;
            button.Click += new RoutedEventHandler(button_Click);

            Canvas.SetLeft(button, buttonOldPoint.X);
            Canvas.SetTop(button, buttonOldPoint.Y);

            this.canvas.Children.Add(button);

            //copy background
            var background = new RenderTargetBitmap((int)screen.ActualWidth, (int)screen.ActualHeight, 
                                                    96d, 96d, PixelFormats.Pbgra32);
            background.Render(screen);

            int height = (int)background.Height;
            int width = (int)background.Width;

            var top = new CroppedBitmap(background, new Int32Rect(0, 0, width, (int)splitLine));
            var bottom = new CroppedBitmap(background, new Int32Rect(0, (int)splitLine, width, (int)(height - splitLine)));

            bg_top.Source = top;
            bg_bottom.Source = bottom;

            bg_top.Width = width;
            bg_top.Height = splitLine;

            bg_bottom.Width = width;
            bg_bottom.Height = height - splitLine;

            //generata container
            this.Container.Width = width;
            this.Container.Height = 200;

            //animation
            Storyboard sb = new Storyboard();
            Duration du = new Duration(TimeSpan.FromSeconds(1));
            double verticalMove = buttonOldPoint.Y - buttonNewPoint.Y;

            translate(bg_top, 0, -verticalMove);
            translate(button, buttonOldPoint.Y, buttonNewPoint.Y);
            translate(Container, splitLine, splitLine - verticalMove);
            translate(bg_bottom, splitLine, splitLine + (Container.Height - verticalMove));
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            isEnd = true;
            //rollback
            double verticalMove = buttonOldPoint.Y - buttonNewPoint.Y;

            translate(bg_top, buttonNewPoint.Y - buttonOldPoint.Y, 0);
            translate(button, buttonNewPoint.Y, buttonOldPoint.Y);
            translate(Container, splitLine - verticalMove, splitLine);
            translate(bg_bottom, splitLine + (Container.Height - verticalMove), splitLine);
        }

        private void translate(FrameworkElement element, double from, double to)
        {
            
            DoubleAnimation da = new DoubleAnimation( from, to, new Duration(TimeSpan.FromMilliseconds(500)));

            Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Top)")); //Do not miss the '(' and ')'
            sb.Children.Add(da);

            element.BeginStoryboard(sb);
        }

        void sb_Completed(object sender, EventArgs e)
        {
            if (!isEnd)
            {
                return;
            }
            //remove self
            var parent = this.Parent as WrapPanel;

            if (parent != null && parent.Children.Contains(this))
            {
                parent.Children.Remove(this);
                System.Diagnostics.Debug.WriteLine("animation end");

            }
        }

        public void setView(UserControl element)
        {
            this.Container.Children.Add(element);
        }
    }
}
