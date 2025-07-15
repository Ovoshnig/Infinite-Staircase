using UnityEngine;

[RequireComponent(typeof(Canvas))]
public abstract class CanvasView : MonoBehaviour
{
    private Canvas _canvas;

    private Canvas Canvas
    {
        get
        {
            if (_canvas == null)
                _canvas = GetComponent<Canvas>();

            return _canvas;
        }
    }
}
