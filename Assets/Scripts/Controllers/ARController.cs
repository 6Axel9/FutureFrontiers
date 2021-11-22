using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARController : MonoBehaviour
{
    private const string CoreImageTrackable = "Syncer";

    [SerializeField]
    private ARSession session;
    [SerializeField]
    private ARSessionOrigin origin;
    [SerializeField]
    private Transform contentRoot;
    [SerializeField]
    private GameObject contentPrefab;

    private ARAnchorManager anchorManager;
    private ARRaycastManager raycastManager;
    private ARTrackedImageManager imageManager;

    public bool IsRootAnchored => contentRoot.parent != null;
    public ARSessionState State => ARSession.state;

    private bool hasAnchor;
    private int elapsedFrames;
    private int maxFrames = 30;

    private void Awake()
    {
        Application.targetFrameRate = maxFrames;
    }

    private void Start()
    {
        anchorManager = origin.GetComponent<ARAnchorManager>();
        raycastManager = origin.GetComponent<ARRaycastManager>();
        imageManager = origin.GetComponent<ARTrackedImageManager>();

        anchorManager.anchorsChanged += OnAnchorsChanged;
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    public void EnableImageTracking()
    {
        imageManager.enabled = true;

        Debug.Log("ImageTracking Enabled");
    }

    public void DisableImageTracking()
    {
        imageManager.enabled = false;
        elapsedFrames = 0;

        Debug.Log("ImageTracking Disabled");
    }

    public void EnableAR()
    {
        session.gameObject.SetActive(true);
    }

    public void DisableAR()
    {
        session.gameObject.SetActive(false);
    }

    private void OnAnchorsChanged(ARAnchorsChangedEventArgs anchorsChangedArgs)
    {
        foreach (ARAnchor anchor in anchorsChangedArgs.added)
        {
            Debug.Log("Anchor Added");

            AnchorContent(anchor);
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs trackedImagesChangedArgs)
    {
        foreach (ARTrackedImage image in trackedImagesChangedArgs.updated)
        {
            if (image.referenceImage.name == CoreImageTrackable)
                SyncContent(image);
        }
    }

    private void SyncContent(ARTrackable syncer)
    {
        Debug.Log("Content Placed");

        if (contentRoot.childCount == 0)
            Instantiate(contentPrefab, contentRoot);

        contentRoot.rotation = syncer.transform.rotation;
        contentRoot.position = syncer.transform.position;
        elapsedFrames++;

        if (IsRootAnchored && elapsedFrames > maxFrames)
            DisableImageTracking();

        if (hasAnchor)
            return;

        RaycastTo(syncer.transform, AddAnchor);
        hasAnchor = true;
    }

    private void AnchorContent(ARAnchor newAnchor)
    {
        if (IsRootAnchored)
            RemoveAnchor();

        contentRoot.SetParent(newAnchor.transform);

        Debug.Log("Content Activeted");

        contentRoot.gameObject.SetActive(true);
    }

    private void AddAnchor(ARRaycastHit hit)
    {
        Debug.Log("Anchor Created");

        anchorManager.AttachAnchor(hit.trackable as ARPlane, hit.pose);
    }

    private void RemoveAnchor()
    {
        Debug.Log("Anchor Removed");

        GameObject oldAnchor = contentRoot.parent.gameObject;
        contentRoot.SetParent(null);
        Destroy(oldAnchor);
    }

    public void RaycastTo(Transform target, Action<ARRaycastHit> callback = null)
    {
        List<ARRaycastHit> results = new List<ARRaycastHit>();
        Vector3 direction = target.position - origin.camera.transform.position;
        Ray ray = origin.camera.ScreenPointToRay(origin.camera.ViewportToScreenPoint(new Vector2(0.5f, 0.5f)));

        Debug.Log("Raycast Peformed");

        if (!raycastManager.Raycast(ray, results, TrackableType.PlaneWithinBounds))
            return;

        Debug.Log("Raycast Successful");

        callback?.Invoke(results.OrderBy(hit => hit.pose.position.y).First());
    }
}