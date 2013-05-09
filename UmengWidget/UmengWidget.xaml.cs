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

using UIControls;
using CommonTools;
using System.Threading;
using UmengWidget.Tools;
using UmengWidget.Model;

namespace UmengWidget
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UmengWidget : UserControl
    {
        private string apk;

        public UmengWidget()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(apk))
            {
                return;
            }

            (sender as ProgressButton).State = State.Working;

            new Thread(checkUmeng).Start();
        }

        private void checkUmeng()
        {
            var meta = new ShowMeta().run( apk );

            Dispatcher.BeginInvoke(new Action(() =>
            {
                check_button.State = State.Normal;

                if (meta != null)
                {
                    showMeta(meta);
                }
            }));
        }

        private void showMeta(UmengMeta meta)
        {
            SplitScreen split = new SplitScreen(check_button, new Point(0, 0), this.bg);

            split.setView(new AdsDetails( meta ));

            this.float_layer.Children.Add(split);  
        }

        private void dragDrop_Event(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length == 1 && files[0].isApkFile())
                {
                    e.Effects = DragDropEffects.Copy;
                    e.Handled = true;

                    if (e.RoutedEvent == DragDrop.DropEvent)
                    {
                        apk = files[0];

                        file_name.Text = System.IO.Path.GetFileName(files[0]).ToLower();
                        file_size.Text = files[0].formatFileSize();
                    }

                    return;
                }
            }

            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

    }
}
