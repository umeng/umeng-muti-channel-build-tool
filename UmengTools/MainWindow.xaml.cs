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

namespace UmengTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            WindowStyle = System.Windows.WindowStyle.None;

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (ex != null)
            {
                var sb = new StringBuilder();
                sb.Append(ex.Message);
                sb.Append("\n\r");
                sb.Append(ex.StackTrace);

                if (ex.InnerException != null)
                {
                    sb.Append("\n\rInnerException");
                    sb.Append(ex.InnerException.Message);
                    sb.Append("\n\r");
                    sb.Append(ex.InnerException.StackTrace);
                }

                MessageBox.Show(sb.ToString(),"程序异常退出");
            }
        }
        private void closebutton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void minibutton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void menubutton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("友盟渠道打包工具\n"+
                            "源码地址：\n"+
                            "https://github.com/umeng");
        }

    }
}
