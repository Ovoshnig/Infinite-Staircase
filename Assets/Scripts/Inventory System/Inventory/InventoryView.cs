using UnityEngine;

public class InventoryView : MonoBehaviour
{
    private SlotView[] _slotViews;

    public SlotView[] SlotViews => _slotViews;

    [field: SerializeField] public RectTransform CanvasRectTransform { get; private set; }

    private void Awake() => _slotViews = GetComponentsInChildren<SlotView>();
}
