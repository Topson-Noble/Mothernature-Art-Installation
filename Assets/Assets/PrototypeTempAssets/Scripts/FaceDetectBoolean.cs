using Mediapipe.Tasks.Vision.FaceLandmarker;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using System.Collections.Generic;
using UnityEngine;

public class FaceDetectBoolean : MonoBehaviour
{
    private FaceLandmarkerResultAnnotationController _faceController;
    public GameObject[] motherNatureObjects; // Assign in Inspector
    private List<Transform> neckBones = new List<Transform>();
    public float rotationSpeed = 5f;
    [SerializeField] FaceLandmarkerRunner _faceLandmarkerRunner;
    void Start()
    {                           
        _faceController = FindObjectOfType<FaceLandmarkerResultAnnotationController>();

        if (_faceController == null)
        {
            Debug.LogError("FaceLandmarkerResultAnnotationController not found in the scene!");
        }
        else
        {
            Debug.Log(_faceController.gameObject.name);
        }

        // Find all neck03 bones in motherNatureObjects
        foreach (GameObject obj in motherNatureObjects)
        {
            Transform neck = FindChildByName(obj.transform, "neck03");
            if (neck != null)
            {
                neckBones.Add(neck);
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
            RotateNecksToFace();
        }
    }

    public bool isFaceDetected()
    {
        
        FaceLandmarkerResult result = _faceLandmarkerRunner.newResult;
        return result.faceLandmarks != null && result.faceLandmarks.Count > 0;
    }

    void RotateNecksToFace()
    {
        FaceLandmarkerResult result = _faceLandmarkerRunner.newResult;

        if (result.faceLandmarks != null && result.faceLandmarks.Count > 0)
        {
            var noseLandmark = result.faceLandmarks[0];
            Vector3 nosePosition = new Vector3(noseLandmark.landmarks[0].x, noseLandmark.landmarks[0].y, noseLandmark.landmarks[0].z);
            Vector3 worldNosePosition = Camera.main.ViewportToWorldPoint(new Vector3(nosePosition.x, nosePosition.y, Camera.main.nearClipPlane));

            foreach (Transform neckBone in neckBones)
            {
                if (neckBone == null) continue;

                Vector3 direction = worldNosePosition - neckBone.position;
                direction.y = -direction.y; // Adjust for correct vertical tilt

                if (direction.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    targetRotation *= Quaternion.Euler(20f, 0f, 0f); // Apply X-axis offset
                    neckBone.localRotation = Quaternion.Slerp(neckBone.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
                }
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