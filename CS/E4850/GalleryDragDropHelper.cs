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
    class GalleryItemDragDropHelper : DependencyObject {
        static GalleryControl gallery;
        static GalleryItem dragableItem;
        static GalleryItemGroup container;
        static GalleryItemControl gic;
        static int originalIndex = -1;
        static Point clickPoint;

        public static readonly DependencyProperty EnableGalleryItemDraggingProperty = DependencyProperty.RegisterAttached("EnableGalleryItemDragging",
            typeof(bool), typeof(GalleryItemDragDropHelper), new PropertyMetadata(false, new PropertyChangedCallback(EnableGalleryItemDraggingPropertyChanged)));

        public static bool GetEnableGalleryItemDragging(DependencyObject d) {
            return (bool)d.GetValue(EnableGalleryItemDraggingProperty);
        }
        public static void SetEnableGalleryItemDragging(DependencyObject d, bool value) {
            d.SetValue(EnableGalleryItemDraggingProperty, value);
        }
        static void EnableGalleryItemDraggingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is GalleryControl) {
                if ((bool)e.NewValue == true) {
                    gallery = (d as GalleryControl);
                    gallery.PreviewMouseLeftButtonDown += Gallery_PreviewMouseLeftButtonDown;
                    gallery.PreviewMouseLeftButtonUp += GalleryItemDragDropHelper_PreviewMouseLeftButtonUp;
                    gallery.MouseMove += gallery_MouseMove;
                    gallery.Drop += gallery_Drop;
                    gallery.DragOver += gallery_DragOver;

                } else {
                    if (gallery != null) {
                        gallery.PreviewMouseLeftButtonDown -= Gallery_PreviewMouseLeftButtonDown;
                        gallery.PreviewMouseLeftButtonUp -= GalleryItemDragDropHelper_PreviewMouseLeftButtonUp;
                        gallery.MouseMove -= gallery_MouseMove;
                        gallery.Drop -= gallery_Drop;
                        gallery.DragOver -= gallery_DragOver;
                    }
                }
            }
        }

        static void GalleryItemDragDropHelper_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            dragableItem = null;
        }
        
        static void gallery_DragOver(object sender, DragEventArgs e) {
            if (!e.Data.GetDataPresent(typeof(GalleryItem))) {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
        }

        static void gallery_Drop(object sender, DragEventArgs e) {
            e.Handled = true;
            if (dragableItem != null) {
                HitTestResult res = VisualTreeHelper.HitTest((GalleryControl)sender, e.GetPosition((GalleryControl)sender));
                if (res.VisualHit == null)
                    return;
                GalleryItemControl target = LayoutHelper.FindParentObject<GalleryItemControl>(res.VisualHit);
                if (target != null && target.Item != dragableItem) {
                    GalleryItemCollection targetCollection = target.Item.Group.Items;
                    GalleryItemCollection sourceCollection = dragableItem.Group.Items;
                    int targetIndex = targetCollection.IndexOf(target.Item);
                    if (targetCollection == sourceCollection) {
                        targetCollection.Move(originalIndex, targetIndex);
                    } else {
                        sourceCollection.Remove(dragableItem);
                        targetCollection.Insert(targetIndex, dragableItem);
                    }
                    dragableItem = null;
                }
            }
        }

        static void gallery_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed && IsDraggingStart(e.GetPosition((GalleryControl)sender))) {
                DragDrop.DoDragDrop(gallery, dragableItem, DragDropEffects.Move);
            }
        }

        static void Gallery_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            clickPoint = e.GetPosition((GalleryControl)sender);
            HitTestResult res = VisualTreeHelper.HitTest((GalleryControl)sender, clickPoint);
            if (res.VisualHit == null)
                return;
            gic = LayoutHelper.FindParentObject<GalleryItemControl>(res.VisualHit);
            if (gic != null) {
                dragableItem = gic.Item;
                container = dragableItem.Group;
                originalIndex = container.Items.IndexOf(dragableItem);
            }
        }

        static bool IsDraggingStart(Point newPosition) {
            return (Math.Abs(clickPoint.X - newPosition.X) > 15 || Math.Abs(clickPoint.Y - newPosition.Y) > 15) && dragableItem != null;
        }
    }

}
