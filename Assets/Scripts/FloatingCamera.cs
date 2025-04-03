using UnityEngine;
using DG.Tweening;

public class FloatingCamera : MonoBehaviour
{
    public float floatHeight = 0.2f; // Small movement for a natural feel
    public float duration = 2f; // Smooth breathing timing

    void Start()
    {

        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);

        }



        FloatAnimation();
    }

    void FloatAnimation()
    {
        // Moves the camera up and down continuously
        transform.DOMoveY(transform.position.y + floatHeight/100, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
