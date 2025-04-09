using UnityEngine;

public class WindowView : MonoBehaviour
{
    private GameObject _window;

    private void Awake() => _window = gameObject;

    public void Activate() => _window.SetActive(true);

    public void Deactivate() => _window.SetActive(false);
}
