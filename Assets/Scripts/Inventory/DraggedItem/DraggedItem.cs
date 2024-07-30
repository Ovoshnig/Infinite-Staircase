using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(RectTransform),
                  typeof(CanvasGroup))]
public class DraggedItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private const float TransparentValue = 0.75f;
    private const float OpaqueValue = 1f;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private DraggedItemHolder _draggedItemHolder;

    [Inject]
    private void Construct(DraggedItemHolder draggedItemSystem) => _draggedItemHolder = draggedItemSystem;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = TransparentValue;
        _canvasGroup.blocksRaycasts = false;
        _draggedItemHolder.SetDraggedItem(_rectTransform);
    }

    public void OnDrag(PointerEventData eventData) => transform.position = Mouse.current.position.ReadValue();

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = OpaqueValue;
        _canvasGroup.blocksRaycasts = true;
        _draggedItemHolder.ReleaseDraggedItem();
    }
}
