using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform),
                  typeof(CanvasGroup))]
public class DraggedItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private const float TransparentValue = 0.8f;
    private const float OpaqueValue = 1f;

    private CanvasGroup _canvasGroup;

    private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

    public void OnBeginDrag(PointerEventData _)
    {
        _canvasGroup.alpha = TransparentValue;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData _) => MoveToMousePosition();

    public void OnEndDrag(PointerEventData _)
    {
        _canvasGroup.alpha = OpaqueValue;
        _canvasGroup.blocksRaycasts = true;
    }

    public void MoveToMousePosition() => transform.position = Mouse.current.position.ReadValue();
}
