using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerScopeView : MonoBehaviour
{
    private Image _scopeImage;

    private Image ScopeImage
    {
        get
        {
            if (_scopeImage == null)
                _scopeImage = GetComponent<Image>();

            return _scopeImage;
        }
    }

    public void SetActive(bool value) => gameObject.SetActive(value);
}
