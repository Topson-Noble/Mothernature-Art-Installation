using UnityEngine;
using DG.Tweening;
using System;

public class SkyScraperRise : MonoBehaviour
{
    private Vector3 originalScale; // Store the original scale
    
    public float duration = 2f; // Animation time

    void Start()
    {
        originalScale = new Vector3(1f,1f,1f); // Save original size
        transform.localScale = new Vector3(originalScale.x, 0, originalScale.z); // Start at height 0
    }

    public void Appear()
    {
        this.gameObject.SetActive(true);
        transform.DOScale(originalScale, duration)
            .SetEase(Ease.OutBack) // Dramatic pop-up effect
            .OnComplete(() => Debug.Log("Skyscraper has fully appeared!"));
    }

    

    public void Disappear(Action onComplete = null)
    {
        transform.DOScale(0, duration)
            .SetEase(Ease.InBack) // Dramatic collapse effect
            .OnComplete(() =>
            {
                Debug.Log("Skyscraper has disappeared!");
                onComplete?.Invoke(); // Invoke the callback once animation is complete
            });
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Appear();
        }
        if (Input.GetKey(KeyCode.F))
        {
            Disappear();
        }
    }
}
