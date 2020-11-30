using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class AnchorCreator : MonoBehaviour
{
    [SerializeField]
    GameObject m_Prefab;
    int TapCount;
    public float MaxDubbleTapTime = 0.35f;
    float NewTime;

    public GameObject prefab
    {
        get => m_Prefab;
        set => m_Prefab = value;
    }

    public void RemoveAllAnchors()
    {
        Debug.Log($"Removing all anchors ({m_Anchors.Count})");
        foreach (var anchor in m_Anchors)
        {
            Destroy(anchor.gameObject);
        }
        m_Anchors.Clear();
    }

    void Awake()
    {
        TapCount = 0;
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
    }

    ARAnchor CreateAnchor(in ARRaycastHit hit)
    {
        if (m_Anchors.Count < 3)
        {
            ARAnchor anchor = null;

            // If we hit a plane, try to "attach" the anchor to the plane
            if (hit.hitType == TrackableType.PlaneWithinBounds)
            {
                var planeManager = GetComponent<ARPlaneManager>();
                if (planeManager)
                {
                    Debug.Log("Creating anchor attachment.");
                    var oldPrefab = m_AnchorManager.anchorPrefab;
                    m_AnchorManager.anchorPrefab = prefab;
                    var plane = planeManager.planePrefab.GetComponent<ARPlane>();
                    anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);
                    m_AnchorManager.anchorPrefab = oldPrefab;
                    return anchor;
                }
            }

            // Otherwise, just create a regular anchor at the hit pose
            Debug.Log("Creating regular anchor.");

            // Note: the anchor can be anywhere in the scene hierarchy
            var gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);
            AnchorObjects.Add(gameObject);
            // Make sure the new GameObject has an ARAnchor component
            anchor = gameObject.GetComponent<ARAnchor>();
            if (anchor == null)
            {
                anchor = gameObject.AddComponent<ARAnchor>();
            }

            Debug.Log("Set Anchor from " + hit.hitType);

            return anchor;
        }

        else
        {
            ARAnchor anchor = m_Anchors[0];

            // If we hit a plane, try to "attach" the anchor to the plane
            if (hit.hitType == TrackableType.PlaneWithinBounds)
            {
                var planeManager = GetComponent<ARPlaneManager>();
                if (planeManager)
                {
                    Debug.Log("Creating anchor attachment.");
                    var oldPrefab = m_AnchorManager.anchorPrefab;
                    m_AnchorManager.anchorPrefab = prefab;
                    var plane = planeManager.planePrefab.GetComponent<ARPlane>();
                    anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);
                    m_AnchorManager.anchorPrefab = oldPrefab;
                    return anchor;
                }
            }

            // Otherwise, just create a regular anchor at the hit pose
            Debug.Log("Creating regular anchor.");

            // Note: the anchor can be anywhere in the scene hierarchy
            AnchorObjects[0].transform.position = hit.pose.position;
            AnchorObjects[0].transform.rotation = hit.pose.rotation;

            // Make sure the new GameObject has an ARAnchor component
            anchor = AnchorObjects[0].GetComponent<ARAnchor>();
            if (anchor == null)
            {
                anchor = gameObject.AddComponent<ARAnchor>();
            }

            Debug.Log("Set Anchor from " + hit.hitType);

            return anchor;
        }
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                TapCount += 1;
            }

            if (TapCount == 1)
            {
                NewTime = Time.time + MaxDubbleTapTime;
            }

            else if (TapCount == 2 && Time.time <= NewTime)
            {
                // Raycast against planes and feature points
                const TrackableType trackableTypes =
                        TrackableType.FeaturePoint |
                        TrackableType.PlaneWithinPolygon;

                // Perform the raycast
                if (m_RaycastManager.Raycast(touch.position, s_Hits, trackableTypes))
                {
                    // Raycast hits are sorted by distance, so the first one will be the closest hit.
                    var hit = s_Hits[0];

                    // Create a new anchor
                    var anchor = CreateAnchor(hit);
                    if (anchor)
                    {
                        // Remember the anchor so we can remove it later.
                        m_Anchors.Add(anchor);
                    }
                    else
                    {
                        Debug.Log("Error creating anchor");
                    }
                }
                TapCount = 0;
            }

        }
        if (Time.time > NewTime)
        {
            TapCount = 0;
        }
    }



    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public List<ARAnchor> m_Anchors = new List<ARAnchor>();
    public List<GameObject> AnchorObjects = new List<GameObject>();

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_AnchorManager;
}