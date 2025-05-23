using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LightController : MonoBehaviour
{

    //MAKE AN ENUM OR LIST OF THE OBJECTS TO DECAY..MAKE A FUNCTION THAT TAKES THE ENUMS STATE..AND BASED ON THAT DISSOLVE THAT PARTICULAR OBJECT
    //THE GLOBE DECAYS FIRST..THEN MOTHERNATURE FIGURE..THEN THE SPACE AND THEN PLANET,,

    [SerializeField] List<LightData> lightData;
    [SerializeField] float time;

    [SerializeField] Material materialA; // Starting material (will be modified)
    [SerializeField] Material materialB; // Target material

    [SerializeField] List<GameObject> objectToBeFaded;
    [SerializeField] List<Material> materialsToBeFaded;
    [SerializeField] List<GameObjectMaterialData> gameObjectsTobeFaded; // the one we are using


    int gameObjCount = 0;



    private void OnEnable()
    {
        GameManager.OnLightDecay += StartLightDecay;
        GameManager.OnMaterialDecay += StartMaterialDecay;
    }

    private void OnDisable()
    {
        GameManager.OnLightDecay -= StartLightDecay;
        GameManager.OnMaterialDecay -= StartMaterialDecay;
    }


    private void Awake()
    {
        SetInitialState();
    }

    private void Start()
    {
        //StartDecay();
        SetMaterialsToMinDissolve();
    }
    void SetMaterialsToMinDissolve()
    {
        foreach (GameObjectMaterialData obj in gameObjectsTobeFaded)
        {
            foreach (Material material in obj.objectMaterials)
            {
                if (material.HasProperty("_Dissolve"))
                {
                    int propertyIndex = material.shader.FindPropertyIndex("_Dissolve");
                    Vector2 rangeLimits = material.shader.GetPropertyRangeLimits(propertyIndex);
                    float minDissolve = rangeLimits.x;

                    material.SetFloat("_Dissolve", minDissolve);
                }
            }
        }
    }
    public void StartDecay()
    {
        StopAllCoroutines();      
        StartLightDecay(5f);
        StartMaterialDecay(5f);
        
    }


    public void ReverseDecay()
    {
        StopAllCoroutines();
        ReverseLightDecay();
        ReverseMaterialDecay();
      
        
    }



    void StartLightDecay(float time)
    {
        foreach (LightData light in lightData)
        {
            SetLightIntensityOverTime(light.light, light.finalIntensity, time);
        }
    }

    void StartMaterialDecay(float time)
    {

        if (gameObjCount < gameObjectsTobeFaded.Count)
        {




            foreach (Material obj in gameObjectsTobeFaded[gameObjCount].objectMaterials)
            {
                StartDissolve(obj, time, false); // false -> Normal Dissolve (Disappear)
            }

            gameObjCount++;
        }
    }

    void ReverseLightDecay()
    {
        foreach (LightData light in lightData)
        {
            SetLightIntensityOverTime(light.light, light.initialIntensity, time); // Reverse light intensity
        }
    }

    void ReverseMaterialDecay()
    {

        if (gameObjCount > 0)
        {




            foreach (Material obj in gameObjectsTobeFaded[gameObjCount].objectMaterials)
            {
                StartDissolve(obj, time, false); // false -> Normal Dissolve (Disappear)
            }

            gameObjCount--;
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
            light.intensity = Mathf.Lerp(initialIntensity, targetIntensity, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        light.intensity = targetIntensity; // Ensure final value is exact
    }

    public static IEnumerator DissolveMaterialOverTime(Material material, float duration, bool reverse)
    {
        if (material == null || !material.HasProperty("_Dissolve"))
        {
            Debug.LogError("Material is null or doesn't have a Dissolve property");
            yield break;
        }

        int propertyIndex = material.shader.FindPropertyIndex("_Dissolve");
        Vector2 rangeLimits = material.shader.GetPropertyRangeLimits(propertyIndex);
        float minDissolve = rangeLimits.x;
        float maxDissolve = rangeLimits.y;
        float currentDissolve = material.GetFloat("_Dissolve");
        float startValue = currentDissolve;
        float endValue = reverse ? minDissolve : maxDissolve;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            material.SetFloat("_Dissolve", currentValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        material.SetFloat("_Dissolve", endValue); // Ensure final value
    }

    public void StartDissolve(Material materialToDissolve, float dissolveTime, bool reverse)
    {
        StartCoroutine(DissolveMaterialOverTime(materialToDissolve, dissolveTime, reverse));
    }

    void SetInitialState()
    {
        foreach (GameObjectMaterialData obj in gameObjectsTobeFaded)
        {
            foreach (Material mat in obj.objectMaterials)
            {
                mat.SetFloat("_Dissolve", 1);
            }

           
        }
    }
}
