
using UnityEngine;
using UnityEngine.UI;

public class UIBackGroundLoop : MonoBehaviour
{
    public float speed = 0.1f;

    private RawImage _image;
    private Rect _uvRect;

    private void Awake()
    {
        _image = GetComponent<RawImage>();
        _uvRect = _image.uvRect;
     
    }

    private void Update()
    {
        _uvRect.y += speed * Time.unscaledDeltaTime;
        _image.uvRect = _uvRect;
    }



}
