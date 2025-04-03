using Mediapipe.Tasks.Vision.FaceLandmarker;
using Mediapipe.Unity;
using System.Collections.Generic;
using UnityEngine;

public class FaceDetectBoolean : MonoBehaviour
{
    private FaceLandmarkerResultAnnotationController _faceController;
    public GameObject[] motherNatureObjects; // Assign in Inspector
    private List<Transform> neckBones = new List<Transform>();
    public float rotationSpeed = 5f; // Adjust rotation speed

    void Start()
    {
        _faceController = FindObjectOfType<FaceLandmarkerResultAnnotationController>();

        if (_faceController == null)
        {
            Debug.LogError("FaceLandmarkerResultAnnotationController not found in the scene!");
        }

        // Find all neck03 bones in motherNatureObjects
        foreach (GameObject obj in motherNatureObjects)
        {
            Transform neck = FindChildByName(obj.transform, "neck03");
            if (neck != null)
            {
                neckBones.Add(neck);
                Debug.Log($"Neck bone found in {obj.name}");
            }
            else
            {
                Debug.LogWarning($"'neck03' not found in {obj.name}");
            }
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

    void RotateNeckToFace()
    {
        if (_faceController == null) return;

        // Ensure the face is detected
        FaceLandmarkerResult result = _faceController.GetCurrentTarget();
        if (result.faceLandmarks == null || result.faceLandmarks == null || result.faceLandmarks.Count == 0)
        {
            Debug.Log("No face landmarks detected.");
            return;
        }

        // Access the nose landmark (index 0, adjust if necessary)
        var noseLandmark = result.faceLandmarks[0];

        // Convert normalized coordinates (0 to 1) to world space
        Vector3 screenPoint = new Vector3(
            noseLandmark.landmarks[0].x * UnityEngine.Screen.width,
            (1 - noseLandmark.landmarks[0].y) * UnityEngine.Screen.height, // Flip Y-axis
            Camera.main.farClipPlane * 0.3f // Adjust depth to avoid near plane issues
        );

        Vector3 worldNosePosition = Camera.main.ScreenToWorldPoint(screenPoint);
        Debug.Log($"World Nose Position: {worldNosePosition}");

        foreach (Transform neckBone in neckBones)
        {
            if (neckBone == null)
            {
                Debug.LogError("Neck bone is null!");
                continue;
            }

            // Calculate direction towards the nose
            Vector3 direction = new Vector3(
                -(worldNosePosition.x - neckBone.position.x), // Fix left-right inversion
                -(worldNosePosition.y - neckBone.position.y), // Fix top-bottom inversion
                worldNosePosition.z - neckBone.position.z
            );

            if (direction.magnitude > 0.1f)
            {
                // Target rotation towards the nose
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Offset by 20 degrees on X-axis
                targetRotation *= Quaternion.Euler(20f, 180f, 0f);

                // Smoothly rotate the neck
                neckBone.rotation = Quaternion.Slerp(neckBone.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    Transform FindChildByName(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform found = FindChildByName(child, childName);
            if (found != null)
                return found;
        }
        return null;
    }
}
