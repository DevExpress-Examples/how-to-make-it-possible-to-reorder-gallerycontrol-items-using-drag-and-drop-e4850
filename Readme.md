# How to make it possible to reorder GalleryControl items using drag-and-drop


<p>This example demonstrates how to implement the capability to reorder GalleryItems in the GalleryControl by using drag-and-drop.</p><p>To implement this feature, we created the EnableGalleryItemDragging attached property. You can use this property in your project in the following manner:</p><br />


```xaml
<dxb:GalleryControl MouseRightButtonUp="GalleryControl_MouseRightButtonUp_1" Name="galleryControl1" 
                            <bold>AllowDrop="True" </bold>
                            <bold>local:GalleryItemDragDropHelper.EnableGalleryItemDragging="True"</bold> >
            <dxb:GalleryControl.Gallery>
                <dxb:Gallery .....

```

<p>                      </p><p>Also, it is necessary to set the GalleryControl.AllowDrop property to true.</p><p>Please note that the drag-and-drop operation succeeds if you drop a GalleryItem into another GalleryItem of the same GalleryItemGroup or another one. Otherwise, nothing will happen.</p>

<br/>


