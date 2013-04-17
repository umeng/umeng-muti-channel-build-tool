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

using UmengWidget.Tools;
using UmengWidget.Model;

namespace UmengWidget
{
    /// <summary>
    /// Interaction logic for AdsDetails.xaml
    /// </summary>
    public partial class AdsDetails : UserControl
    {
        public AdsDetails()
        {
            InitializeComponent();

            DataContext = new UmengMeta();
        }
    }
}
