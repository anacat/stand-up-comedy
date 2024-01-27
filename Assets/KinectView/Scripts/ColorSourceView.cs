using UnityEngine;
using UnityEngine.UI;

public class ColorSourceView : MonoBehaviour
{
    public ColorSourceManager colorSourceManager;

    private RawImage _image;
    
    private void Awake()
    {
        _image = GetComponent<RawImage>();
    }

    void Update()
    {
        if (colorSourceManager == null)
        {
            return;
        }
        
        _image.texture = colorSourceManager.GetColorTexture();
    }
}
