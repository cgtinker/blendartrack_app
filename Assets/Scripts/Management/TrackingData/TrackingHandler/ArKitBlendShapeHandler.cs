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
    //[RequireComponent(typeof(ARFace))]
    public class ArKitBlendShapeHandler : MonoBehaviour, IInit, IJson, IStop, IPrefix
    {
#if UNITY_IOS && !UNITY_EDITOR
        //accessing sub system & init blend shape dict for mapping
        ARKitFaceSubsystem m_ARKitFaceSubsystem;
#endif
        private List<BlendShapeData> blendShapeDataList = new List<BlendShapeData>();
        private bool recording = false;
        ARFace m_Face;

        private void Start()
        {
            TrackingDataManager dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
            dataManager.SetRecorderReference(this.gameObject);
            var referencer = GameObject.FindGameObjectWithTag("referencer").GetComponent<TrackerReferencer>();
            referencer.Init();
        }

        //might crash
        public void Init()
        {
            Debug.Log("searching for the ar face + face manager");

            SearchForArFace();

            m_Face.updated += OnUpdated;
            recording = true;
        }

        public void SearchForArFace()
        {
#if UNITY_IOS && !UNITY_EDITOR
            if(m_ARKitFaceSubsystem == null)
            {
                var faceManager = FindObjectOfType<ARFaceManager>();
                if (faceManager != null)
                {
                    m_ARKitFaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
                }
            }
#endif

            var face = GameObject.FindGameObjectWithTag("face");
            if (face != null)
            {
                m_Face = face.GetComponent<ARFace>();
            }

            if (m_Face == null)
            {
                SearchForArFace();
                return;
            }
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
            return "face";
        }

        //if face is lost
        void OnDisable()
        {
            if (m_Face == null)
            {
                SearchForArFace();
            }

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
            if(m_Face == null)
            {
                Destroy(tmpBlendShapes);
                SearchForArFace();
            }

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
