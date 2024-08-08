using System;
using UnityEngine;

public class DraggedItemHolder
{
    private RectTransform _draggedItem = null;

    public RectTransform DraggedItem 
    {
        get => _draggedItem;
        set
        {
            if (value == null && _draggedItem == null)
                throw new InvalidOperationException("Item has already been released");

            if (value != null && _draggedItem != null)
                throw new InvalidOperationException("Item is already dragging");

            _draggedItem = value;
        }
    }
}
