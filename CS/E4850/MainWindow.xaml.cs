using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core.Native;
namespace E4850 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        private void Gallery_ItemClick(object sender, GalleryItemEventArgs e) {
            MessageBox.Show("The " + e.Item.Caption + " item has been clicked");
        }

        private void GalleryControl_MouseRightButtonUp_1(object sender, MouseButtonEventArgs e) {
            HitTestResult res = VisualTreeHelper.HitTest((GalleryControl)sender, e.GetPosition((GalleryControl)sender));
            if (res.VisualHit == null)
                return;
            GalleryItemControl galleryItemControl = LayoutHelper.FindParentObject<GalleryItemControl>(res.VisualHit);
            if (galleryItemControl != null) {
                object capt = galleryItemControl.ActualCaption;
                GalleryItem tmpIt = MyDict[capt];
                tmpIt.IsChecked = !tmpIt.IsChecked;
            }
        }
        Dictionary<object, GalleryItem> MyDict;

        private void gallery1_Loaded_1(object sender, RoutedEventArgs e) {
            MyDict = new Dictionary<object, GalleryItem>();
            foreach (GalleryItemGroup g in gallery1.Groups) {

                foreach (GalleryItem it in g.Items) {
                    MyDict.Add(it.Caption, it);
                }
            }
        } 
    }
}
