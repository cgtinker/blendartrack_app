using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ArRetarget;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif

namespace ArRetarget
{

    [RequireComponent(typeof(ARFace))]
    public class ArKitBlendShapeHandler : MonoBehaviour, IInit, IJson, IStop
    {
#if UNITY_IOS && !UNITY_EDITOR
        //accessing sub system & init blend shape dict for mapping
        ARKitFaceSubsystem m_ARKitFaceSubsystem;
#endif
        private List<BlendShapeData> blendShapeDataList = new List<BlendShapeData>();
        private bool recording = false;
        ARFace m_Face;

        void Awake()
        {
            m_Face = GetComponent<ARFace>();
        }

        private void Start()
        {
            DataManager dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<DataManager>();
            dataManager.TrackingReference(this.gameObject);
        }

        //previously was "OnEnable" - might goging to crash?
        public void Init()
        {
            Debug.Log("searching for the ar face manager");

#if UNITY_IOS && !UNITY_EDITOR
            var faceManager = FindObjectOfType<ARFaceManager>();
            if (faceManager != null)
            {
                m_ARKitFaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
            }
#endif
            m_Face.updated += OnUpdated;
            recording = true;
        }

        public string GetJsonString()
        {
            BlendShapeContainter tmp = new BlendShapeContainter()
            {
                blendShapeData = blendShapeDataList
            };

            var json = JsonUtility.ToJson(tmp);
            return json;
        }

        void OnDisable()
        {
            Debug.Log("Disabled Manager, stop referencing");
            m_Face.updated -= OnUpdated;
        }

        public void StopTracking()
        {
            recording = false;
        }

        void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
        {
            UpdateFaceFeatures();
        }

        private int frame;
        //updating facial features as soon the subsystem update occurs
        void UpdateFaceFeatures()
        {
            if (recording)
            {
#if UNITY_IOS && !UNITY_EDITOR
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
    }
}
