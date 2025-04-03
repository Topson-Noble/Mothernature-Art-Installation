using UnityEngine;

public class ColorLerpLoop : MonoBehaviour
{
    public Color colorA = Color.red;
    public Color colorB = Color.blue;
    public float duration = 2f; // Time to complete one lerp cycle

    private MeshRenderer meshRenderer;
    private float t = 0f;
    private bool isReversing = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        t += (isReversing ? -1 : 1) * Time.deltaTime / duration;

        // Lerp between colors
        meshRenderer.material.color = Color.Lerp(colorA, colorB, t);

        // Reverse direction when reaching the limits
        if (t >= 1f)
        {
            t = 1f;
            isReversing = true;
        }
        else if (t <= 0f)
        {
            t = 0f;
            isReversing = false;
        }
    }
}