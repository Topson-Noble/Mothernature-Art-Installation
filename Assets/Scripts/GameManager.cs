using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] FaceDetectBoolean fbool;
    [SerializeField] bool isPersonInfront;
    [SerializeField] bool decayNotStarted = true;
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
    [SerializeField] List<GameObject> birds;

    [SerializeField] AudioClip futuristicClip;
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
        foreach (GameObject bird in birds) {
            bird.GetComponent<lb_Bird>().flyFunc();
        
        }
        yield return new WaitForSeconds(5);


        treesList[1].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[1].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }



        yield return new WaitForSeconds(5);


        treesList[2].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[2].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }

        treesList[3].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[3].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }

        treesList[4].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[4].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }
        treesList[11].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[11].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }


        print("Please disappear");
        yield return StartCoroutine(WaitForFlagToBeFalse());

        scraperList[0].GetComponent<SkyScraperRise>().Disappear(() =>
        {
            treesList[0].GetComponent<TreeAnimation>().Reappear();
        });
        yield return new WaitForSeconds(5);
        print("Please Reappear");

        yield return StartCoroutine(WaitForFlagToBeTrue());


        scraperList[11].GetComponent<SkyScraperRise>().Disappear(() =>
        {
            treesList[11].GetComponent<TreeAnimation>().Reappear();
        });






        yield return new WaitForSeconds(6);
        AudioManager.instance.SwapTrack(futuristicClip);
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
        while (fbool.isFaceDetected())
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
        while (!fbool.isFaceDetected())
        {
            yield return null; // Wait for the next frame
        }
        // Once the flag is false, proceed to the next step
        Debug.Log("Flag is true, proceeding...");
    }



    private void Update()
    {
        if (fbool.isFaceDetected() && decayNotStarted)
        {
            decayNotStarted = false;
            ReverseDecayNotStarted = true;
            StopAllCoroutines();
            StartCoroutine(GameFlowForward());

        }
        
    }
}
