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

namespace TestUI {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {
        public Window1() {
            InitializeComponent();
        }

        private void SearchTextBox_Search(object sender, RoutedEventArgs e) {
            MessageBox.Show("On Every Key Press!");
        }

        private void SearchTextBox_Search_1(object sender, RoutedEventArgs e) {
            MessageBox.Show("On Every Key Press, well, not quite!");
        }
    }
}
