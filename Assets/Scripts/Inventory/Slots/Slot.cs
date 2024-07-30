using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private const string MouseBinding = "<Mouse>/leftButton";

    private RectTransform _draggedItem;
    private DraggedItemHolder _draggedItemHolder;
    private InputAction _mouseClickAction;
    private bool _isPointerOver = false;

    public event Action<RectTransform> ItemTaken;
    public event Action<RectTransform> ItemPlaced;

    [Inject]
    private void Construct(DraggedItemHolder draggedItemHolder) => _draggedItemHolder = draggedItemHolder;

    private void Awake()
    {
        if (transform.childCount > 0)
            _draggedItem = transform.GetChild(0).GetComponent<RectTransform>();
        else
            _draggedItem = null;

        _mouseClickAction = new InputAction(type: InputActionType.Button, binding: MouseBinding);
        _mouseClickAction.canceled += OnMouseClickCanceled;
    }

    private void OnEnable() => _mouseClickAction.Enable();

    private void OnDisable() => _mouseClickAction.Disable();

    public void OnPointerEnter(PointerEventData eventData) => _isPointerOver = true;

    public void OnPointerExit(PointerEventData eventData) => _isPointerOver = false;

    private void OnMouseClickCanceled(InputAction.CallbackContext context)
    {
        if (_isPointerOver)
            OnPointerClickEnd();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_draggedItem != null)
        {
            _draggedItem.transform.SetParent(transform.parent);
            ItemTaken?.Invoke(_draggedItem);
            _draggedItem = null;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    private void OnPointerClickEnd()
    {
        if (_draggedItemHolder.DraggedItem != null)
        {
            _draggedItem = _draggedItemHolder.DraggedItem;
            _draggedItem.transform.SetParent(transform);
            _draggedItem.anchoredPosition = Vector2.zero;
            ItemPlaced?.Invoke(_draggedItem);
        }
    }
}
