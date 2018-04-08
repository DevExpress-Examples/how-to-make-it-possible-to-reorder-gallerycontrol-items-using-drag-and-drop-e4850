Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports DevExpress.Xpf.Bars
Imports DevExpress.Xpf.Core.Native

Namespace vb_E4850
	Friend Class GalleryItemDragDropHelper
		Inherits DependencyObject
		Private Shared gallery As GalleryControl
		Private Shared dragableItem As GalleryItem
		Private Shared container As GalleryItemGroup
		Private Shared gic As GalleryItemControl
		Private Shared originalIndex As Integer = -1
		Private Shared clickPoint As Point


		Public Shared ReadOnly EnableGalleryItemDraggingProperty As DependencyProperty = DependencyProperty.RegisterAttached("EnableGalleryItemDragging", GetType(Boolean), GetType(GalleryItemDragDropHelper), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf EnableGalleryItemDraggingPropertyChanged)))

		Public Shared Function GetEnableGalleryItemDragging(ByVal d As DependencyObject) As Boolean
			Return CBool(d.GetValue(EnableGalleryItemDraggingProperty))
		End Function
		Public Shared Sub SetEnableGalleryItemDragging(ByVal d As DependencyObject, ByVal value As Boolean)
			d.SetValue(EnableGalleryItemDraggingProperty, value)
		End Sub
		Private Shared Sub EnableGalleryItemDraggingPropertyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
			If TypeOf d Is GalleryControl Then
				If CBool(e.NewValue) = True Then
					gallery = (TryCast(d, GalleryControl))
					AddHandler gallery.PreviewMouseLeftButtonDown, AddressOf Gallery_PreviewMouseLeftButtonDown
					AddHandler gallery.PreviewMouseLeftButtonUp, AddressOf GalleryItemDragDropHelper_PreviewMouseLeftButtonUp
					AddHandler gallery.MouseMove, AddressOf gallery_MouseMove
					AddHandler gallery.Drop, AddressOf gallery_Drop
					AddHandler gallery.DragOver, AddressOf gallery_DragOver

				Else
					If gallery IsNot Nothing Then
						RemoveHandler gallery.PreviewMouseLeftButtonDown, AddressOf Gallery_PreviewMouseLeftButtonDown
						RemoveHandler gallery.PreviewMouseLeftButtonUp, AddressOf GalleryItemDragDropHelper_PreviewMouseLeftButtonUp
						RemoveHandler gallery.MouseMove, AddressOf gallery_MouseMove
						RemoveHandler gallery.Drop, AddressOf gallery_Drop
						RemoveHandler gallery.DragOver, AddressOf gallery_DragOver
					End If
				End If
			End If
		End Sub

		Private Shared Sub GalleryItemDragDropHelper_PreviewMouseLeftButtonUp(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			dragableItem = Nothing
		End Sub

		Private Shared Sub gallery_DragOver(ByVal sender As Object, ByVal e As DragEventArgs)
			If (Not e.Data.GetDataPresent(GetType(GalleryItem))) Then
				e.Effects = DragDropEffects.None
				e.Handled = True
				Return
			End If
		End Sub

		Private Shared Sub gallery_Drop(ByVal sender As Object, ByVal e As DragEventArgs)
			e.Handled = True
			If dragableItem IsNot Nothing Then
				Dim res As HitTestResult = VisualTreeHelper.HitTest(CType(sender, GalleryControl), e.GetPosition(CType(sender, GalleryControl)))
				If res.VisualHit Is Nothing Then
					Return
				End If
				Dim target As GalleryItemControl = LayoutHelper.FindParentObject(Of GalleryItemControl)(res.VisualHit)
                If target IsNot Nothing AndAlso target.Item IsNot dragableItem Then
                    Dim targetCollection As GalleryItemCollection = target.Item.Group.Items
                    Dim sourceCollection As GalleryItemCollection = dragableItem.Group.Items
                    Dim targetIndex As Integer = targetCollection.IndexOf(target.Item)
                    If targetCollection Is sourceCollection Then
                        targetCollection.Move(originalIndex, targetIndex)
                    Else
                        sourceCollection.Remove(dragableItem)
                        targetCollection.Insert(targetIndex, dragableItem)
                    End If
                    dragableItem = Nothing
                End If
			End If
		End Sub

		Private Shared Sub gallery_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			If e.LeftButton = MouseButtonState.Pressed AndAlso IsDraggingStart(e.GetPosition(CType(sender, GalleryControl))) Then
				DragDrop.DoDragDrop(gallery, dragableItem, DragDropEffects.Move)
			End If
		End Sub

		Private Shared Sub Gallery_PreviewMouseLeftButtonDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			clickPoint = e.GetPosition(CType(sender, GalleryControl))
			Dim res As HitTestResult = VisualTreeHelper.HitTest(CType(sender, GalleryControl), clickPoint)
			If res.VisualHit Is Nothing Then
				Return
			End If
			gic = LayoutHelper.FindParentObject(Of GalleryItemControl)(res.VisualHit)
			If gic IsNot Nothing Then
				dragableItem = gic.Item
				container = dragableItem.Group
				originalIndex = container.Items.IndexOf(dragableItem)
			End If
		End Sub

		Private Shared Function IsDraggingStart(ByVal newPosition As Point) As Boolean
			Return (Math.Abs(clickPoint.X - newPosition.X) > 15 OrElse Math.Abs(clickPoint.Y - newPosition.Y) > 15) AndAlso dragableItem IsNot Nothing
		End Function
	End Class

End Namespace
