using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class ItemView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private InventorySettings _inventorySettings;

    public void Render(ItemData itemData)
    {
        _image.sprite = itemData.Icon;
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        _image.sprite = null;
        gameObject.SetActive(false);
    }

    public void SetDraggingState(bool isDragging)
    {
        _canvasGroup.alpha = isDragging
            ? _inventorySettings.ItemTransparentValue
            : _inventorySettings.ItemOpaqueValue;
        _canvasGroup.blocksRaycasts = !isDragging;
    }

    public void SetAnchoredPosition(Vector2 position) => 
        (transform as RectTransform).anchoredPosition = position;
}
