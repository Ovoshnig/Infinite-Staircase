using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Color _normalColor = new(255f, 255f, 255f, 255f);
    [SerializeField] private Color _draggedColor = new(255f, 255f, 255f, 170f);

    private Image _image;

    public event Action Started;
    public event Action Ended;

    private void Awake() => _image = GetComponent<Image>();

    public void OnBeginDrag(PointerEventData eventData)
    {
        _image.color = _draggedColor;
        _image.raycastTarget = false;
        Started?.Invoke();
    }

    public void OnDrag(PointerEventData eventData) => transform.position = Mouse.current.position.ReadValue();

    public void OnEndDrag(PointerEventData eventData)
    {
        _image.color = _normalColor;
        _image.raycastTarget = true;
        Ended?.Invoke();
    }
}
