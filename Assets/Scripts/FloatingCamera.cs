using UnityEngine;
using DG.Tweening;

public class FloatingCamera : MonoBehaviour
{
    public float floatHeight = 0.2f; // Small movement for a natural feel
    public float duration = 2f; // Smooth breathing timing

   // [SerializeField] private float shakeStrength = 0.05f; // Small movement for mild shake
    [SerializeField] private float durationshake = 1.5f;
    [SerializeField] private float shakeDuration = 1.5f;
    [SerializeField] private float shakeStrength = 0.1f; // Increase for stronger shake
    [SerializeField] private int vibrato = 10; // More vibrato = more shakiness
    [SerializeField] private float randomness = 90f;
    private Tween floatingTween; // Store the breathing tween

    void Start()
    {
        StartBreathing();
       // StopBreathingAndStartMildShake();
    }

    void StartBreathing()
    {
        floatingTween = transform.DOMoveY(transform.position.y + floatHeight / 100, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopBreathingAndStartMildShake()
    {
        if (floatingTween != null)
        {
            floatingTween.Kill(); // Stop the breathing effect
        }

        // Apply a randomized shake effect
        transform.DOShakePosition(shakeDuration, shakeStrength, vibrato, randomness, false, true)
            .SetLoops(-1, LoopType.Restart);
    }
}
