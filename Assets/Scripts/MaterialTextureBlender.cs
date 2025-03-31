using UnityEngine;
using System.Collections;

public class MaterialTextureBlender : MonoBehaviour
{
    public void BlendMaterials(
        Material fromMaterial,
        Material toMaterial,
        float duration,
        bool blendAlbedo = true,
        bool blendEmission = true,
        bool blendNormalMap = false,
        bool blendMetallicSmoothness = true
    )
    {
        StartCoroutine(BlendMaterialsCoroutine(
            fromMaterial,
            toMaterial,
            duration,
            blendAlbedo,
            blendEmission,
            blendNormalMap,
            blendMetallicSmoothness
        ));
    }

    private IEnumerator BlendMaterialsCoroutine(
        Material fromMat,
        Material toMat,
        float duration,
        bool blendAlbedo,
        bool blendEmission,
        bool blendNormalMap,
        bool blendMetallicSmoothness
    )
    {
        // Cache initial values
        Color fromAlbedo = blendAlbedo ? fromMat.color : Color.white;
        Color toAlbedo = blendAlbedo ? toMat.color : Color.white;

        Texture fromAlbedoTex = blendAlbedo ? fromMat.mainTexture : null;
        Texture toAlbedoTex = blendAlbedo ? toMat.mainTexture : null;

        Color fromEmission = blendEmission ? fromMat.GetColor("_EmissionColor") : Color.black;
        Color toEmission = blendEmission ? toMat.GetColor("_EmissionColor") : Color.black;

        Texture fromEmissionTex = blendEmission ? fromMat.GetTexture("_EmissionMap") : null;
        Texture toEmissionTex = blendEmission ? toMat.GetTexture("_EmissionMap") : null;

        Texture fromNormalMap = blendNormalMap ? fromMat.GetTexture("_BumpMap") : null;
        Texture toNormalMap = blendNormalMap ? toMat.GetTexture("_BumpMap") : null;

        float fromMetallic = blendMetallicSmoothness ? fromMat.GetFloat("_Metallic") : 0f;
        float toMetallic = blendMetallicSmoothness ? toMat.GetFloat("_Metallic") : 0f;

        float fromSmoothness = blendMetallicSmoothness ? fromMat.GetFloat("_Glossiness") : 0f;
        float toSmoothness = blendMetallicSmoothness ? toMat.GetFloat("_Glossiness") : 0f;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Blend Albedo Color & Texture
            if (blendAlbedo)
            {
                fromMat.color = Color.Lerp(fromAlbedo, toAlbedo, t);
                if (fromAlbedoTex != null && toAlbedoTex != null)
                {
                    // If using RenderTextures, blend them via a shader (see Advanced section)
                    fromMat.mainTexture = TextureLerp(fromAlbedoTex, toAlbedoTex, t);
                }
            }

            // Blend Emission Color & Texture
            if (blendEmission)
            {
                fromMat.SetColor("_EmissionColor", Color.Lerp(fromEmission, toEmission, t));
                if (fromEmissionTex != null && toEmissionTex != null)
                {
                    fromMat.SetTexture("_EmissionMap", TextureLerp(fromEmissionTex, toEmissionTex, t));
                }
                fromMat.EnableKeyword("_EMISSION");
            }

            // Blend Normal Map
            if (blendNormalMap && fromNormalMap != null && toNormalMap != null)
            {
                fromMat.SetTexture("_BumpMap", TextureLerp(fromNormalMap, toNormalMap, t));
            }

            // Blend Metallic & Smoothness
            if (blendMetallicSmoothness)
            {
                fromMat.SetFloat("_Metallic", Mathf.Lerp(fromMetallic, toMetallic, t));
                fromMat.SetFloat("_Glossiness", Mathf.Lerp(fromSmoothness, toSmoothness, t));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values match the target material
        if (blendAlbedo)
        {
            fromMat.color = toAlbedo;
            fromMat.mainTexture = toAlbedoTex;
        }

        if (blendEmission)
        {
            fromMat.SetColor("_EmissionColor", toEmission);
            fromMat.SetTexture("_EmissionMap", toEmissionTex);
        }

        if (blendNormalMap)
            fromMat.SetTexture("_BumpMap", toNormalMap);

        if (blendMetallicSmoothness)
        {
            fromMat.SetFloat("_Metallic", toMetallic);
            fromMat.SetFloat("_Glossiness", toSmoothness);
        }
    }

    // Basic texture blending (for demonstration; requires optimization)
    private Texture TextureLerp(Texture a, Texture b, float t)
    {
        // Note: This is a simplified version. For real use, use Compute Shaders or RenderTextures.
        // See the "Advanced Texture Blending" section below.
        return t < 0.5f ? a : b; // Placeholder: Crossfades abruptly at t=0.5
    }
}