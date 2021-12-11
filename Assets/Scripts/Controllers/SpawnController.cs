using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[Serializable]
public class SpawnableDictionary : SerializableDictionary<string, GameObject> { }

public class SpawnController : MonoBehaviour
{
    private const string AnchorCode = "Core";

    [SerializeField]
    private SpawnableDictionary spawnables;

    private List<ARTrackedImage> trackedTargets;
    private GameController controller;
    private Transform content;
    private ARAnchor anchor;

    private void Start()
    {
        controller = GetComponent<GameController>();

        controller.OnAnchorAdded += AnchorContent;
        controller.OnTrackedImageAdded += ScanContent;
        controller.OnTrackedImageUpdated += PositionContent;
    }

    public void EnableContent()
        => content.gameObject.SetActive(true);

    private void AnchorContent(ARAnchor anchor)
    {
        content.SetParent(anchor.transform);
        AddContent();
    }

    private void ScanContent(ARTrackedImage trackable)
    {
        if (trackedTargets.Contains(trackable))
            return;

        trackedTargets.Add(trackable);

        if (anchor == null && trackable.referenceImage.name == AnchorCode)
            controller.RaycastTo(controller.AddAnchor);
        else
            controller.RaycastTo(AddContent);

        Debug.Log("Scanning Image");
    }

    private void PositionContent(ARTrackedImage trackable)
    {
        if (trackable.referenceImage.name != AnchorCode)
            return;

        if (content == null)
            content = new GameObject("ARContent").transform;

        content.rotation = trackable.transform.rotation;
        content.position = trackable.transform.position;

        content.gameObject.SetActive(false);
    }

    private void AddContent(ARRaycastHit hit)
    {
        Debug.Log("Content Created");

        Instantiate(spawnables[trackedTargets[trackedTargets.Count - 1].referenceImage.name], hit.pose.position, content.rotation, content);
    }

    private void AddContent()
    {
        Debug.Log("Content Created");

        Instantiate(spawnables[trackedTargets[trackedTargets.Count - 1].referenceImage.name], content);
    }
}
