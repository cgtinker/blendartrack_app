using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif

namespace ArRetarget
{
	[RequireComponent(typeof(ARFace))]
	public class ArKitBlendShapeHandler : MonoBehaviour, IInit<string, string>, IStop, IPrefix
	{
#if UNITY_IOS && !UNITY_EDITOR
        //accessing sub system & init blend shape dict for mapping
        ARKitFaceSubsystem m_ARKitFaceSubsystem;
#endif
		private List<BlendShapeData> blendShapeDataList = new List<BlendShapeData>();
		private bool recording = false;
		ARFace m_Face;
		private bool lastFrame = false;

		void Awake()
		{
			m_Face = this.gameObject.GetComponent<ARFace>();
		}

		private void Start()
		{
			TrackingDataManager dataManager = GameObject.FindGameObjectWithTag(TagManager.TrackingDataManager).GetComponent<TrackingDataManager>();
			dataManager.SetRecorderReference(this.gameObject);
		}

		private string filePath;
		public void Init(string path, string title)
		{
			filePath = $"{path}{title}_{j_Prefix()}.json";
			JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "blendShapeData", lastFrame: false);

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
			m_Face.updated += OnUpdated;
			recording = true;
			lastFrame = false;
		}

		//json file prefix
		public string j_Prefix()
		{
			return "face";
		}

		//if face is lost
		void OnDisable()
		{
			Debug.Log("Disabled Manager, stop referencing");
			m_Face.updated -= OnUpdated;
		}

		public void StopTracking()
		{
			lastFrame = true;
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

            string json = JsonUtility.ToJson(blendShapeData);

                if (lastFrame)
                {
                    string par = "]}";
                    json += par;
                    recording = false;
                }
             JsonFileWriter.WriteDataToFile(path: filePath, text: json, title: "", lastFrame: lastFrame);
            if (lastFrame)
            {
                filePath = null;
            }
#endif
			}
		}
	}
}
