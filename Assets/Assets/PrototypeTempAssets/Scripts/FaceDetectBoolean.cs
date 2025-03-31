using Mediapipe.Tasks.Vision.FaceLandmarker;
using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class FaceDetectBoolean : MonoBehaviour
    {
    private FaceLandmarkerResultAnnotationController _faceController;

    void Start()
    {
        _faceController = FindObjectOfType<FaceLandmarkerResultAnnotationController>();

        if (_faceController == null)
        {
            print("FaceLandmarkerResultAnnotationController not found in the scene!");
        }
    }

    void Update()
    {
        isFaceDetected();
    }
    public bool isFaceDetected()
    {
        if (_faceController == null) return false;
        FaceLandmarkerResult result = _faceController.GetCurrentTarget();

        if (result.faceLandmarks != null && result.faceLandmarks != null && result.faceLandmarks.Count > 0)
        {
            Debug.Log("Face detected!");
            return true;
        }
        else
        {
            Debug.Log("No face detected.");
            return false;
        }
    }

}