using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform),
                  typeof(CanvasGroup))]
public class DraggedItemView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField, Range(0f, 1f)] private float _transparentValue = 0.8f;
    [SerializeField, Range(0f, 1f)] private float _opaqueValue = 1f;

    private CanvasGroup _canvasGroup;

    private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsLeftButtonClicked(eventData))
            return;

        _canvasGroup.alpha = _transparentValue;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsLeftButtonClicked(eventData))
            return;

        transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsLeftButtonClicked(eventData))
            return;

        _canvasGroup.alpha = _opaqueValue;
        _canvasGroup.blocksRaycasts = true;
    }

    private bool IsLeftButtonClicked(PointerEventData eventData) => 
        eventData.button == PointerEventData.InputButton.Left;
}
