using UnityEngine;
using DG.Tweening;

public class TreeAnimation : MonoBehaviour
{
    public float swayAngle = 5f; // Small angle for subtle sway
    public float duration = 2f; // Time for one sway cycle




    public float fallAngle = 80f; // How much the tree tilts when falling
    public float durationFall = 2f; // Time for the fall animation
    public Vector3 fallDirection = Vector3.forward; // Direction the tree falls





   

    private Tween swayTween; // Store the tween to control it

   

   

    public void StopSway()
    {
        if (swayTween != null && swayTween.IsActive())
        {
            swayTween.Kill();
        }
    }


    void Fall()
    {
        StopSway();
        // Rotate the tree to simulate falling
        transform.DORotate(fallDirection * fallAngle, durationFall)
            .SetEase(Ease.InOutQuad);
    }
    void Start()
    {
        Sway();
        Invoke(nameof(Fall), 10f);
    }

    void Sway()
    {
        // Rotate left and then right in a loop
        swayTween= transform.DORotate(new Vector3(0, 0, swayAngle), duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
