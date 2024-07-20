using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Item _item;

    public event Action<Item> ItemTaken;
    public event Action<Item> ItemPlaced;

    public void OnPointerDown(PointerEventData eventData)
    {

        ItemTaken?.Invoke(_item);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        ItemPlaced?.Invoke(_item);
    }
}
