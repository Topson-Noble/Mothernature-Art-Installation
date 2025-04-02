using UnityEngine;
using DG.Tweening;
using System;

public class TreeAnimation : MonoBehaviour
{
    public float swayAngle = 5f; // Small angle for subtle sway
    public float duration = 2f; // Time for one sway cycle
    private Vector3 originalRotation;
    private Vector3 originalScale;



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


    public void Fall(Action onComplete = null)
    {
        StopSway();
        // Rotate the tree to simulate falling
        transform.DORotate(fallDirection * fallAngle, durationFall)
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => Disappear(onComplete));
    }


    private void Disappear(Action onComplete)
    {
        transform.DOScale(Vector3.zero, 1f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false); // Hide the tree
                onComplete?.Invoke(); // Call the callback function if provided
            });
    }
    void Start()
    {
        originalRotation = transform.eulerAngles; // Save original rotation
        originalScale = transform.localScale; //
        Sway();

        //Invoke(nameof(Fall), 10f);
    }

    public void Sway()
    {
        // Rotate left and then right in a loop
        swayTween= transform.DORotate(new Vector3(0, 0, swayAngle), duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Reappear()
    {
        gameObject.SetActive(true);
        transform.eulerAngles = originalRotation; // Reset rotation
        transform.localScale = Vector3.zero; // Start from zero

        transform.DOScale(originalScale, duration)
            .SetEase(Ease.OutBack); // Grow back smoothly
        Sway();
    }
}
