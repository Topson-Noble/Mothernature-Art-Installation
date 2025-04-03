using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    public Image image;  // Assign in Inspector
    public float fadeDuration = 2f;

    void Start()
    {
        decayNotStarted=true;
    }
    IEnumerator GameFlowForward()
    {
        NarrationManager.instance.PlayNarration(NarrationManager.NarrationState.Intro);
        yield return new WaitForSeconds(21);
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
        yield return new WaitForSeconds(15);



        





        treesList[7].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[7].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }
        yield return new WaitForSeconds(20);
        treesList[4].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[4].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }
        yield return new WaitForSeconds(4);
        treesList[5].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[5].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }
        yield return new WaitForSeconds(15);
        treesList[11].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[11].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }

        yield return new WaitForSeconds(15);


        treesList[9].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[9].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }
        yield return new WaitForSeconds(22);

        treesList[8].GetComponent<TreeAnimation>().Fall(() =>
        {
            scraperList[8].GetComponent<SkyScraperRise>().Appear();
        });
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<lb_Bird>().flyFunc();

        }
        yield return new WaitForSeconds(14);




        yield return StartCoroutine(WaitForFlagToBeFalse());

        NarrationManager.instance.PlayNarration(NarrationManager.NarrationState.Observe);









        scraperList[0].GetComponent<SkyScraperRise>().Disappear(() =>
        {
            treesList[0].GetComponent<TreeAnimation>().Reappear();
        });
        yield return new WaitForSeconds(5);


        scraperList[11].GetComponent<SkyScraperRise>().Disappear(() =>
        {
            treesList[11].GetComponent<TreeAnimation>().Reappear();
        });

        yield return new WaitForSeconds(5);

        scraperList[5].GetComponent<SkyScraperRise>().Disappear(() =>
        {
            treesList[5].GetComponent<TreeAnimation>().Reappear();
        });

        yield return new WaitForSeconds(5);

        scraperList[7].GetComponent<SkyScraperRise>().Disappear(() =>
        {
            treesList[7].GetComponent<TreeAnimation>().Reappear();
        });

        scraperList[9].GetComponent<SkyScraperRise>().Disappear(() =>
        {
            treesList[9].GetComponent<TreeAnimation>().Reappear();
        });

        yield return new WaitForSeconds(5);

        yield return StartCoroutine(WaitForFlagToBeTrue());
        NarrationManager.instance.PlayNarration(NarrationManager.NarrationState.Final);

        yield return new WaitForSeconds(10);

        for(int i = 0; i<2; i++)
        {
            int index = i;
            treesList[index].GetComponent<TreeAnimation>().Fall(() =>
            {
                scraperList[index].GetComponent<SkyScraperRise>().Appear();
            });
        }



        yield return new WaitForSeconds(10);

        for (int i = 2; i < 4; i++)
        {
            int index = i;
            treesList[index].GetComponent<TreeAnimation>().Fall(() =>
            {
                scraperList[index].GetComponent<SkyScraperRise>().Appear();
            });
        }
        yield return new WaitForSeconds(10);


        for (int i = 4; i < scraperList.Count; i++)
        {
            int index = i;
            treesList[index].GetComponent<TreeAnimation>().Fall(() =>
            {
                scraperList[index].GetComponent<SkyScraperRise>().Appear();
            });
        }

        yield return new WaitForSeconds(16);
        AudioManager.instance.SwapTrack(futuristicClip);
        secondCam.GetComponent<FloatingCamera>().StopBreathingAndStartMildShake();
        OnLightDecay?.Invoke(10f);
        OnMaterialDecay?.Invoke(5f);
        yield return new WaitForSeconds(15);
        OnMaterialDecay?.Invoke(5f);
        yield return new WaitForSeconds(15);
        OnMaterialDecay?.Invoke(5f);
        yield return new WaitForSeconds(10);
        OnMaterialDecay?.Invoke(5f);
        yield return new WaitForSeconds(5);
        OnMaterialDecay?.Invoke(5f);

        yield return new WaitForSeconds(20);

        StartCoroutine("FadeIn");




    }


    

    private IEnumerator WaitForFlagToBeFalse()
    {
        Debug.Log("waiting...");
        NarrationManager.instance.PlayNarration(NarrationManager.NarrationState.MoveOut);
        Debug.Log("valueee "+ fbool.isFaceDetected());
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

        NarrationManager.instance.PlayNarration(NarrationManager.NarrationState.ComeBack);
        Debug.Log("waiting...");
        // While the flag is not false, keep waiting
        while ( !fbool.isFaceDetected())
        {
            yield return null; // Wait for the next frame
        }
        // Once the flag is false, proceed to the next step
        Debug.Log("Flag is true, proceeding...");
    }



    private void Update()
    {
        if ( fbool.isFaceDetected()&& decayNotStarted)
        {
            print("Check")
;
            decayNotStarted = false;
            ReverseDecayNotStarted = true;
            StopAllCoroutines();
            StartCoroutine(GameFlowForward());

        }
        
    }



    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = image.color;
        color.a = 0f;  // Start with fully transparent
        image.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }
    }
}
