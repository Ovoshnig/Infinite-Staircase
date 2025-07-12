using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SkinnedMeshRendererView : MonoBehaviour
{
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    private SkinnedMeshRenderer SkinnedMeshRenderer
    {
        get
        {
            if (_skinnedMeshRenderer == null)
                _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

            return _skinnedMeshRenderer;
        }
    }

    public void ChangeShadowCastingMode(bool isFirstPerson)
    {
        SkinnedMeshRenderer.shadowCastingMode = isFirstPerson
            ? ShadowCastingMode.ShadowsOnly
            : ShadowCastingMode.On;
    }
}
