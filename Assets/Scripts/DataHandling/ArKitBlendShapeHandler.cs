﻿using System.Collections.Generic;
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
    public class ArKitBlendShapeHandler : MonoBehaviour, IInit, IJson, IStop, IPrefix
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
            TrackingDataManager dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
            dataManager.TrackingReference(this.gameObject);
        }

        //might crash
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

        //generating json string
        public string GetJsonString()
        {
            BlendShapeContainter tmp = new BlendShapeContainter()
            {
                blendShapeData = blendShapeDataList
            };

            var json = JsonUtility.ToJson(tmp);
            return json;
        }

        //json file prefix
        public string GetJsonPrefix()
        {
            return "BS";
        }

        //if face is lost
        void OnDisable()
        {
            Debug.Log("Disabled Manager, stop referencing");
            m_Face.updated -= OnUpdated;
        }

        public void StopTracking()
        {
            recording = false;
        }

        //receiving update
        void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
        {
            UpdateFaceFeatures();
        }

        private int frame;
        //while recoring updating facial features and store them as blendshapedata as soon the subsystem update occurs
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

            //getting blend shape data
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