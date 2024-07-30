using System;
using UnityEngine;

public class DraggedItemHolder
{
    public RectTransform DraggedItem { get; private set; } = null;

    public void SetDraggedItem(RectTransform draggedItem)
    {
        if (DraggedItem != null)
            throw new InvalidOperationException("Item is already dragging");

        DraggedItem = draggedItem;
    }

    public void ReleaseDraggedItem()
    {
        if (DraggedItem == null)
            throw new InvalidOperationException("Item has already been released");

        DraggedItem = null;
    }
}
