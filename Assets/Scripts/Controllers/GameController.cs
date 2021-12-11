using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameController : MonoBehaviour
{
    private const int TargetFramerate = 30;

    [SerializeField]
    private ARSession session;
    [SerializeField]
    private ARSessionOrigin origin;

    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager;
    private ARPlaneManager planeManager;
    private ARTrackedImageManager imageManager;

    public ARSessionState State => ARSession.state;

    public Action<ARAnchor> OnAnchorAdded;
    public Action<ARTrackedImage> OnTrackedImageAdded;
    public Action<ARTrackedImage> OnTrackedImageUpdated;
    public Action<ARTrackedImage> OnTrackedImageRemoved;

    private void Awake()
    {
        Application.targetFrameRate = TargetFramerate;
    }

    private void Start()
    {
        raycastManager = origin.GetComponent<ARRaycastManager>();
        anchorManager = origin.GetComponent<ARAnchorManager>();
        planeManager = origin.GetComponent<ARPlaneManager>();
        imageManager = origin.GetComponent<ARTrackedImageManager>();

        anchorManager.anchorsChanged += OnAnchorsChanged;
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    public void ToggleRaycastTracking()
        => raycastManager.enabled = !raycastManager.enabled;

    public void ToggleAnchorTracking()
        => anchorManager.enabled = !anchorManager.enabled;

    public void TogglePlaneTracking()
        => planeManager.enabled = !planeManager.enabled;

    public void ToggleImageTracking()
        => imageManager.enabled = !imageManager.enabled;

    public void ToggleAR()
        => session.gameObject.SetActive(!session.gameObject.activeSelf);

    public void RaycastTo(Action<ARRaycastHit> callback = null)
    {
        Vector2 screenPosition = new Vector2(0.5f, 0.5f);
        List<ARRaycastHit> results = new List<ARRaycastHit>();
        Ray ray = origin.camera.ViewportPointToRay(screenPosition);

        if (raycastManager.Raycast(ray, results, TrackableType.PlaneWithinBounds))
            callback?.Invoke(results.OrderBy(hit => hit.pose.position.y).First());
    }

    public void AddAnchor(ARRaycastHit hit)
        => anchorManager.AttachAnchor(hit.trackable as ARPlane, hit.pose);
    

    private void OnAnchorsChanged(ARAnchorsChangedEventArgs anchorsChangedArgs)
    {
        foreach (ARAnchor trackable in anchorsChangedArgs.added)
        {
            OnAnchorAdded.Invoke(trackable);
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs trackedImagesChangedArgs)
    {
        foreach (ARTrackedImage trackable in trackedImagesChangedArgs.added)
        {
            OnTrackedImageAdded.Invoke(trackable);
        }

        foreach (ARTrackedImage trackable in trackedImagesChangedArgs.updated)
        {
            OnTrackedImageUpdated.Invoke(trackable);
        }

        foreach (ARTrackedImage trackable in trackedImagesChangedArgs.removed)
        {
            OnTrackedImageRemoved.Invoke(trackable);
        }
    }
}