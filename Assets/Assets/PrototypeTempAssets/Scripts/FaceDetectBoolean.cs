using Mediapipe.Tasks.Vision.FaceLandmarker;
using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDetectBoolean : MonoBehaviour
{
    private FaceLandmarkerResultAnnotationController _faceController;
    public Transform neckBone; // Assign this in the Inspector (Character’s neck bone)
    public float rotationSpeed = 5f; // Adjust rotation speed

    void Start()
    {
        _faceController = FindObjectOfType<FaceLandmarkerResultAnnotationController>();

        if (_faceController == null)
        {
            Debug.LogError("FaceLandmarkerResultAnnotationController not found in the scene!");
        }
    }

    void Update()
    {
        if (isFaceDetected())
        {
            RotateNeckToFace();
        }
    }

    public bool isFaceDetected()
    {
        if (_faceController == null) return false;
        FaceLandmarkerResult result = _faceController.GetCurrentTarget();

        if (result.faceLandmarks != null && result.faceLandmarks.Count > 0)
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

    void RotateNeckToFace()
    {
        // Ensure the face is detected
        FaceLandmarkerResult result = _faceController.GetCurrentTarget();

        if (result.faceLandmarks != null && result.faceLandmarks.Count > 0)
        {
            // Access the nose landmark (usually index 0, adjust according to your setup)
            var noseLandmark = result.faceLandmarks[0]; // Update the index if necessary

            // Normalize the position to world space (assuming you have normalized coordinates)
            Vector3 nosePosition = new Vector3(noseLandmark.landmarks[0].x, noseLandmark.landmarks[0].y, noseLandmark.landmarks[0].z);

            // Convert from normalized space (0 to 1) to world space, if necessary
            Vector3 worldNosePosition = Camera.main.ViewportToWorldPoint(new Vector3(nosePosition.x, nosePosition.y, Camera.main.nearClipPlane));

            // Now, calculate the direction from the neckBone to the nose in world space
            Vector3 direction = worldNosePosition - neckBone.position;

            // Invert the Y component of the direction to fix the rotation (for vertical tilt)
            direction.y = -direction.y;

            // If the direction is valid (to avoid unnecessary rotation)
            if (direction.magnitude > 0.1f)
            {
                // Create the desired rotation towards the nose
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Apply the X-axis offset (e.g., 12-13 degrees) to the target rotation
                targetRotation *= Quaternion.Euler(40f, 0f, 0f); // Adjust the X-axis rotation by 12-13 degrees

                // Calculate the local rotation difference (relative to current neck bone rotation)
                Quaternion localRotation = Quaternion.Inverse(neckBone.rotation) * targetRotation;

                // Smoothly rotate the neck from its current local rotation towards the target rotation
                neckBone.localRotation = Quaternion.Slerp(neckBone.localRotation, localRotation, Time.deltaTime * 5f); // Adjust speed as necessary
            }
        }
    }






}
