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
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
		End Sub
		Private Sub Gallery_ItemClick(ByVal sender As Object, ByVal e As GalleryItemEventArgs)
            MessageBox.Show("The " & e.Item.Caption.ToString() & " item has been clicked")
		End Sub

		Private Sub GalleryControl_MouseRightButtonUp_1(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			Dim res As HitTestResult = VisualTreeHelper.HitTest(CType(sender, GalleryControl), e.GetPosition(CType(sender, GalleryControl)))
			If res.VisualHit Is Nothing Then
				Return
			End If
			Dim galleryItemControl As GalleryItemControl = LayoutHelper.FindParentObject(Of GalleryItemControl)(res.VisualHit)
			If galleryItemControl IsNot Nothing Then
				Dim capt As Object = galleryItemControl.ActualCaption
				Dim tmpIt As GalleryItem = MyDict(capt)
				tmpIt.IsChecked = Not tmpIt.IsChecked
			End If
		End Sub
		Private MyDict As Dictionary(Of Object, GalleryItem)

		Private Sub gallery1_Loaded_1(ByVal sender As Object, ByVal e As RoutedEventArgs)
			MyDict = New Dictionary(Of Object, GalleryItem)()
			For Each g As GalleryItemGroup In gallery1.Groups

				For Each it As GalleryItem In g.Items
					MyDict.Add(it.Caption, it)
				Next it
			Next g
		End Sub
	End Class
End Namespace
