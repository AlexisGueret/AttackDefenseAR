using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.AI;

[RequireComponent(typeof(ARRaycastManager))]
public class TapToPlaceGameField : MonoBehaviour
{
    [SerializeField]
    NavMeshSurface[] fieldSurfaces;
    [SerializeField]
    private GameObject footballField;

    private ARRaycastManager aRRaycastManager;
    private PlaneDetectionController planeDetectionController;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();


    public bool InitializeField(Vector2 touchPosition)
    {
        if (aRRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            footballField.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            var hitPose = hits[0].pose;
            footballField.transform.position = hitPose.position;
            footballField.transform.rotation = hitPose.rotation;
            footballField.transform.Rotate(-90, 0, 0);
            foreach(NavMeshSurface surface in fieldSurfaces)
            {
                surface.BuildNavMesh();
            }
            planeDetectionController.TogglePlaneDetection();
            return true;

        }
        return false;
    }

    void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        planeDetectionController = GetComponent<PlaneDetectionController>();
    }

}
