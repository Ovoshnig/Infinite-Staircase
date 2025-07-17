using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class InventoryView : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRectTransform;

    private SlotView[] _slotViews = null;
    private ItemView _draggedItemView;
    private Transform _draggedItemParentTransform;

    public SlotView[] SlotViews
    {
        get
        {
            if (_slotViews == null)
                _slotViews = GetComponentsInChildren<SlotView>(true);

            return _slotViews;
        }
    }

    public void MoveItemToMouse()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform,
            Mouse.current.position.ReadValue(),
            null,
            out var localPoint);

        _draggedItemView.SetAnchoredPosition(localPoint);
    }

    public void OnDragStarted(int index)
    {
        _draggedItemView = SlotViews[index].ItemView;

        if (_draggedItemView != null)
        {
            _draggedItemParentTransform = _draggedItemView.transform.parent;
            _draggedItemView.transform.SetParent(_canvasRectTransform, true);
            _draggedItemView.transform.SetAsLastSibling();
            _draggedItemView.SetDraggingState(true);
        }
    }

    public void OnDragEnded()
    {
        if (_draggedItemView != null)
        {
            _draggedItemView.transform.SetParent(_draggedItemParentTransform, false);
            _draggedItemView.SetDraggingState(false);
            _draggedItemView = null;
        }
    }
}
