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

    [SerializeField] Animator motherNatureAnimator;
    [SerializeField] Animator motherNatureAnimator2;
    [SerializeField] GameObject firstCam;
    [SerializeField] GameObject secondCam;

    [SerializeField] List<GameObject> treesList;
    [SerializeField] List<GameObject> scraperList;
    IEnumerator GameFlowForward()
    {


        motherNatureAnimator.SetBool("OpenScene",true);
        motherNatureAnimator2.SetBool("OpenScene", true);

        yield return new WaitForSeconds(4);
        firstCam.SetActive(false);
        yield return new WaitForSeconds(5);

        treesList[0].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[0].GetComponent<SkyScraperRise>().Appear();
        });
        yield return new WaitForSeconds(5);
        yield return StartCoroutine(WaitForFlagToBeFalse());

        scraperList[0].GetComponent<SkyScraperRise>().Disappear(() =>
        {
            treesList[0].GetComponent<TreeAnimation>().Reappear();
        });
        yield return new WaitForSeconds(5);
        yield return StartCoroutine(WaitForFlagToBeTrue());




        

        


        yield return new WaitForSeconds(6);
        OnLightDecay?.Invoke(10f);
        OnMaterialDecay?.Invoke(3f);
        yield return new WaitForSeconds(2);
        OnMaterialDecay?.Invoke(3f);
        yield return new WaitForSeconds(2);
        OnMaterialDecay?.Invoke(3f);
        yield return new WaitForSeconds(2);
        OnMaterialDecay?.Invoke(3f);
        yield return new WaitForSeconds(2);
        OnMaterialDecay?.Invoke(3f);




    }


    

    private IEnumerator WaitForFlagToBeFalse()
    {
        Debug.Log("waiting...");
        // While the flag is not false, keep waiting
        while (isPersonInfront)
        {
            yield return null; // Wait for the next frame
        }
        // Once the flag is false, proceed to the next step
        Debug.Log("Flag is false, proceeding...");
    }



    private IEnumerator WaitForFlagToBeTrue()
    {
        Debug.Log("waiting...");
        // While the flag is not false, keep waiting
        while (!isPersonInfront)
        {
            yield return null; // Wait for the next frame
        }
        // Once the flag is false, proceed to the next step
        Debug.Log("Flag is true, proceeding...");
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
        
    }
}
