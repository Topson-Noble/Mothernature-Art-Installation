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
    public FaceDetectBoolean faceDetectBool;
    public NarrationState currentState = NarrationState.None;
    public AudioSource audioSource;
    public AudioClip introClip, moveOutClip, observeClip, comeBackClip, finalClip;
    private float pausedTime = 0f;
    public static NarrationManager instance;

    void Start()
    {
       // PlayNarration(NarrationState.Intro);

        instance= this;
    }

    public void PlayNarration(NarrationState state)
    {
        /*if (audioSource.isPlaying)
            audioSource.Stop();*/

        currentState = state;

        switch (state)
        {
            case NarrationState.Intro:
                audioSource.clip = introClip;
                break;
            case NarrationState.MoveOut:
                audioSource.clip = moveOutClip;
                //StartCoroutine(LoopWhileConditionTrue(() => /* Add condition here */ true));
                break;
            case NarrationState.Observe:
                audioSource.clip = observeClip;
                break;
            case NarrationState.ComeBack:
                audioSource.clip = comeBackClip;
                // StartCoroutine(LoopWhileConditionTrue(() => /* Add condition here */ true));
                break;
            case NarrationState.Final:
                audioSource.clip = finalClip;
                // StartCoroutine(LoopWhileConditionTrue(() => /* Add condition here */ true));
                break;
            case NarrationState.None:
                audioSource.Stop();
                return;
        }
        //print("Play");
        audioSource.Play();
        //StartCoroutine(CheckAndChangeState());
    }

    IEnumerator LoopWhileConditionTrue(System.Func<bool> condition)
    {
        while (condition())
        {
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
        }
    }
    bool isPaused;
    private void Update()
    {
        //if (!faceDetectBool.isFaceDetected())
        //{
        //    isPaused=true;
        //    PauseNarration();
        //}
        //else if (isPaused && faceDetectBool.isFaceDetected())
        //{
        //    ResumeNarration();
        //    isPaused=false;
        //}
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
  
    IEnumerator CheckAndChangeState()
    {
        yield return new WaitUntil(() => IsCurrentClipFinished());
        //print("ConditionMet");
        ChangeToNextState();
    }

    public bool IsCurrentClipFinished()
    {
        //print("checking");
        return audioSource.clip != null && !audioSource.isPlaying && audioSource.time >= audioSource.clip.length;
    }

    void ChangeToNextState()
    {
        if (currentState < NarrationState.Final) 
        {
            //print("currentState 0 : "+currentState);

            currentState++;
            //print("currentState : "+currentState);

            PlayNarration(currentState);
        }
    }
}
