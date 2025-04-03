using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    Coroutine currentroutine;
    public AudioClip defaultAmbient;
    AudioSource track01, track02;
    bool isPlayingTrack01;
    public static AudioManager instance;
    public AudioSource treeSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        track01 = gameObject.AddComponent<AudioSource>();
        track02 = gameObject.AddComponent<AudioSource>();
        track01.loop = true;
        track02.loop = true;
        isPlayingTrack01 = true;
        track01.volume = 0.25f;
        track02.volume = 0.25f;
        SwapTrack(defaultAmbient);
    }


    public void SwapTrack(AudioClip newClip)
    {
        if (currentroutine!=null)
        {
            StopCoroutine(currentroutine);
        }
        

        currentroutine= StartCoroutine(FadeInTrack(newClip));
        isPlayingTrack01=!isPlayingTrack01; 
    }

    public void returnToDefault()
    {
        SwapTrack(defaultAmbient);
    }

    IEnumerator FadeInTrack(AudioClip newClip)
    {
        float timeToFade = 2f;
            float timeElapsed = 0;
        if (isPlayingTrack01)
        {
            track02.clip = newClip;
            track02.Play();
            while (timeElapsed < timeToFade)
            {
                track02.volume = Mathf.Lerp(0,1,timeElapsed/timeToFade);
                track01.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed+= Time.deltaTime;
                yield return null;
            }



            track01.Stop();
        }
        else
        {
            track01.clip = newClip;
            track01.Play();

            while (timeElapsed < timeToFade)
            {
                track02.volume = Mathf.Lerp(0.5f, 0, timeElapsed / timeToFade);
                track01.volume = Mathf.Lerp(0, 0.5f, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track02.Stop();
        }

    }
}
