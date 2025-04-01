using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{


    [SerializeField] List<LightData> lightData;

    [SerializeField] float time;



    [SerializeField] Material materialA; // Starting material (will be modified)
    [SerializeField] Material materialB; // Target material


    [SerializeField] List<GameObject> objectToBeFaded;

    [SerializeField] List<Material> materialsToBeFaded;

    private void Awake()
    {
        SetInitialState();
    }

    private void Start()
    {
        StartDecay();

       // GetComponent<MaterialTextureBlender>().BlendMaterials(
       //    materialA,
       //    materialB,
       //    time,
       //    blendAlbedo: true,
       //    blendEmission: true,
       //    blendNormalMap: false,
       //    blendMetallicSmoothness: true
       //);
       
    }

    public void StartDecay()
    {
        foreach (LightData light in lightData)
        {
            SetLightIntensityOverTime(light.light,light.finalIntensity,time);
        }

        

        foreach (Material obj in materialsToBeFaded)
        {
            StartDissolve(obj,3);
        }
    }




    public void SetLightIntensityOverTime(Light targetLight, float targetIntensity, float duration)
    {
        StartCoroutine(ChangeLightIntensity(targetLight, targetIntensity, duration));
    }

    private IEnumerator ChangeLightIntensity(Light light, float targetIntensity, float duration)
    {
        float initialIntensity = light.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Smoothly interpolate intensity over time
            light.intensity = Mathf.Lerp(initialIntensity, targetIntensity, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure final intensity is exact
        light.intensity = targetIntensity;
    }



    public static IEnumerator DissolveMaterialOverTime(Material material, float duration)
    {
        // Check if material and shader are valid
        if (material == null || !material.HasProperty("_Dissolve"))
        {
            Debug.LogError("Material is null or doesn't have a Dissolve property");
            yield break;
        }
        Debug.Log("Yay" );
        float startValue = material.GetFloat("_Dissolve");
        float endValue = 0f;
        float elapsedTime = 0f;

        Debug.Log("Start" + material.GetFloat("_Dissolve"));
        while (elapsedTime < duration)
        {
            // Calculate lerp progress (0 to 1)
            float t = elapsedTime / duration;

            // Lerp the dissolve value
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            material.SetFloat("_Dissolve", currentValue);

            // Increment time and wait for next frame
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure we end exactly at 1
        material.SetFloat("_Dissolve", endValue);
        Debug.Log("Yay" + material.GetFloat("_Dissolve"));
    }

    // Example usage function
    public void StartDissolve(Material materialToDissolve, float dissolveTime)
    {
        StartCoroutine(DissolveMaterialOverTime(materialToDissolve, dissolveTime));
    }

    void SetInitialState()
    {
        foreach(Material obj in materialsToBeFaded)
        {
            obj.SetFloat("_Dissolve", 1);
        }
    }
}
