using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool isPersonInfront;
    [SerializeField] bool decayNotStarted;
    [SerializeField] bool ReverseDecayNotStarted;
    public static Action<float> OnLightDecay;
    public static Action<float> OnMaterialDecay;
    public static Action<float> OnLightReverse;
    public static Action<float> OnMaterialReverse;
    IEnumerator GameFlowForward()
    {

        yield return new WaitForSeconds(10);
        OnLightDecay?.Invoke(10f);
        OnMaterialDecay?.Invoke(3f);
        yield return new WaitForSeconds(2);
        OnMaterialDecay?.Invoke(3f);
        yield return new WaitForSeconds(2);
        OnMaterialDecay?.Invoke(3f);
        yield return new WaitForSeconds(2);
        OnMaterialDecay?.Invoke(3f);




    }


    IEnumerator GameFlowBackword()
    {

        yield return new WaitForSeconds(10);
        OnLightReverse?.Invoke(5f);
        yield return new WaitForSeconds(10);
        OnMaterialReverse?.Invoke(3f);
    }



    private void Update()
    {
        if (isPersonInfront && decayNotStarted)
        {
            decayNotStarted = false;
            ReverseDecayNotStarted = true;
            StopAllCoroutines();
            StartCoroutine(GameFlowForward());

        }
        else if (!isPersonInfront && ReverseDecayNotStarted) {
            ReverseDecayNotStarted = false;
            decayNotStarted = true;
        
        
        }
    }
}
