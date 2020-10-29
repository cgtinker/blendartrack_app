using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ArRetarget;
#if UNITY_IOS && UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif

[RequireComponent(typeof(ARFace))]
public class ArKitBlendShapeHandler : MonoBehaviour
{
#if UNITY_IOS && UNITY_EDITOR
    //accessing sub system & init blend shape dict for mapping
    ARKitFaceSubsystem m_ARKitFaceSubsystem;
    private List<BlendShapeData> blendShapeDataList = new List<BlendShapeData>();
#endif

    ARFace m_Face;

    void Awake()
    {
        m_Face = GetComponent<ARFace>();
    }

    void OnEnable()
    {
        Debug.Log("searching for the ar face manager, planning referencing");

#if UNITY_IOS && UNITY_EDITOR
        var faceManager = FindObjectOfType<ARFaceManager>();
        if (faceManager != null)
        {
            m_ARKitFaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
        }
#endif
        m_Face.updated += OnUpdated;
    }

    void OnDisable()
    {
        Debug.Log("Disabled Manager, stop referencing");
        m_Face.updated -= OnUpdated;
    }

    void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        UpdateFaceFeatures();
    }

    private int frame = 0;
    void UpdateFaceFeatures()
    {
#if UNITY_IOS && UNITY_EDITOR
        List<BlendShape> tmpBlendShapes = new List<BlendShape>();
        frame++;

        using (var m_blendShapes = m_ARKitFaceSubsystem.GetBlendShapeCoefficients(m_Face.trackableId, Allocator.Temp))
        {
            foreach (var featureCoefficient in m_blendShapes)
            {
                BlendShape blendShape = new BlendShape()
                {
                    shapeKey = featureCoefficient.blendShapeLocation.ToString(),
                    value = featureCoefficient.coefficient
                };

                tmpBlendShapes.Add(blendShape);
            }
        }

        BlendShapeData blendShapeData = new BlendShapeData()
        {
            blendShapes = tmpBlendShapes,
            frame = frame
        };

        blendShapeDataList.Add(blendShapeData);
#endif
    }
}
