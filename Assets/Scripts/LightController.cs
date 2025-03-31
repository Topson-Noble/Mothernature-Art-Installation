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

        foreach(GameObject obj in objectToBeFaded)
        {
            obj.GetComponent<FadeOut>().StartFadeOut();
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
}
