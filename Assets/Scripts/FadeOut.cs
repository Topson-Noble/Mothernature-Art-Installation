using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class FadeOut : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private bool _disableAfterFade = true;

    [Header("Advanced")]
    [SerializeField] private string _alphaProperty = "_Color"; // Standard: "_Color", URP: "_BaseColor"
    [SerializeField] private bool _changeRenderQueue = true;

    private Material[] _originalMaterials;
    private Material[] _fadeMaterials;
    private Coroutine _fadeRoutine;
    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        CacheOriginalMaterials();
    }

    // Call this to start fade
    public void StartFadeOut()
    {
        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeOutNow());
    }

    private IEnumerator FadeOutNow()
    {
        // Create temporary fade materials
        _fadeMaterials = new Material[_originalMaterials.Length];
        for (int i = 0; i < _originalMaterials.Length; i++)
        {
            _fadeMaterials[i] = new Material(_originalMaterials[i]);

            // Enable transparency
            _fadeMaterials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _fadeMaterials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _fadeMaterials[i].EnableKeyword("_ALPHABLEND_ON");

            if (_changeRenderQueue)
                _fadeMaterials[i].renderQueue = 3000; // Transparent queue
        }
        _renderer.materials = _fadeMaterials;

        // Fade over time
        float elapsedTime = 0f;
        while (elapsedTime < _fadeDuration)
        {
            float t = elapsedTime / _fadeDuration;
            for (int i = 0; i < _fadeMaterials.Length; i++)
            {
                Color color = _fadeMaterials[i].GetColor(_alphaProperty);
                color.a = Mathf.Lerp(1f, 0f, t);
                _fadeMaterials[i].SetColor(_alphaProperty, color);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Finalize
        if (_disableAfterFade)
        {
            _renderer.enabled = false; // More elegant than SetActive(false)
            ResetMaterials(); // Restore originals when re-enabled
        }
    }

    private void CacheOriginalMaterials()
    {
        _originalMaterials = _renderer.materials;
    }

    private void ResetMaterials()
    {
        if (_renderer != null)
            _renderer.materials = _originalMaterials;
    }

    void OnDisable()
    {
        ResetMaterials(); // Cleanup if disabled externally
    }
}