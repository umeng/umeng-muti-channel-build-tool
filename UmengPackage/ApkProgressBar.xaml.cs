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
using System.Windows.Media.Animation;

namespace UmengPackage
{
    /// <summary>
    /// Interaction logic for ApkProgressBar.xaml
    /// </summary>
    public partial class ApkProgressBar : UserControl
    {
        public ApkProgressBar()
        {
            InitializeComponent();

            //Timeline.DesiredFrameRateProperty.OverrideMetadata(
            //        typeof(Timeline),
            //        new FrameworkPropertyMetadata { DefaultValue = 20 } );
        }
    }
}
