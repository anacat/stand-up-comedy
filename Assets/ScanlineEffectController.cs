using UnityEngine;

public class ScanlineEffectController : MonoBehaviour
{
    public Material scanlineMat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, scanlineMat);
    }
}