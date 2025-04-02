using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public enum NarrationState
    {
        None,
        Intro,
        MoveOut,
        Observe,
        ComeBack,
        Final
    }

    public NarrationState currentState = NarrationState.None;
    public AudioSource audioSource;
    public AudioClip introClip, moveOutClip, observeClip, comeBackClip, finalClip;
    private float pausedTime = 0f;
    public static NarrationManager instance;

    void Start()
    {
        PlayNarration(NarrationState.Intro); // Start with intro narration
        instance= this;
    }

    public void PlayNarration(NarrationState state)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        currentState = state;

        switch (state)
        {
            case NarrationState.Intro:
                audioSource.clip = introClip;
                break;
            case NarrationState.MoveOut:
                audioSource.clip = moveOutClip;
                StartCoroutine(LoopWhileConditionTrue(() => /* Add condition here */ true));
                return;
            case NarrationState.Observe:
                audioSource.clip = observeClip;
                break;
            case NarrationState.ComeBack:
                audioSource.clip = comeBackClip;
                StartCoroutine(LoopWhileConditionTrue(() => /* Add condition here */ true));
                return;
            case NarrationState.Final:
                audioSource.clip = finalClip;
                StartCoroutine(LoopWhileConditionTrue(() => /* Add condition here */ true));
                return;
            case NarrationState.None:
                audioSource.Stop();
                return;
        }
        audioSource.Play();
    }

    IEnumerator LoopWhileConditionTrue(System.Func<bool> condition)
    {
        while (condition())
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }

    public void PauseNarration()
    {
        if (audioSource.isPlaying)
        {
            pausedTime = audioSource.time;
            audioSource.Pause();
        }
    }

    public void ResumeNarration()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.time = pausedTime;
            audioSource.Play();
        }
    }
}
