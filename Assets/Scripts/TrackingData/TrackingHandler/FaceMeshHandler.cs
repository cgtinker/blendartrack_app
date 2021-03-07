using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;

namespace ArRetarget
{
    public class FaceMeshHandler : MonoBehaviour, IInit<string, string>, IPrefix, IStop //IGet<int, bool>,
    {
        private bool lastFrame = false;
        private ARFace m_face;
        private ARFaceManager m_faceManager;
        private string filePath;

        private bool updatedVerticesThisFrame;
        private int frame;
        private bool recording = false;
        private static string jsonContents;
        private int curTick;

        private bool write;

        #region init
        private void Awake()
        {
            //list to store native vertices
            s_Vertices = new List<Vector3>();
        }

        private void Start()
        {
            write = false;
            //reference ar face component
            TrackingDataManager dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
            dataManager.SetRecorderReference(this.gameObject);
            m_faceManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARFaceManager>();
            m_faceManager.facesChanged += OnFaceUpdate;
        }

        //only works with a single face mesh
        public void Init(string path, string title)
        {
            //assign variables
            lastFrame = false;
            recording = true;
            jsonContents = "";

            //init json file on disk
            filePath = $"{path}{title}_{j_Prefix()}.json";
            JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "meshDataList", lastFrame: false);
            Debug.Log("Initialized face mesh");
        }
        #endregion

        #region Face Mesh Referencing
        private void OnDisable()
        {
            //unsub from the ar face changes event
            m_faceManager.facesChanged -= OnFaceUpdate;
        }

        private void OnFaceUpdate(ARFacesChangedEventArgs args)
        {
            //assign newly added ar face
            if (args.added.Count > 0)
            {
                Debug.Log("face added");
                var faceObj = args.added[0].gameObject;
                m_face = faceObj.GetComponent<ARFace>();
            }

            //unassign ar face when it's lost
            if (args.removed.Count > 0)
            {
                Debug.Log("face lost");
                m_face = null;
            }

            GetMeshDataAndWriteJson();
        }
        #endregion

        #region Getting and Writing Data
        public void StopTracking()
        {
            lastFrame = true;
        }

        private void GetMeshDataAndWriteJson()
        {
            if (!updatedVerticesThisFrame && recording)
            {
                //getting vertex data
                frame++;
                MeshData meshData = GetMeshData(frame);
                string json = JsonUtility.ToJson(meshData);
                (jsonContents, curTick, write) = DataHelper.JsonContentTicker(lastFrame: lastFrame, curTick: curTick, reqTick: 15, contents: jsonContents, json);


                if (write && !lastFrame)
                {
                    JsonFileWriter.WriteDataToFile(path: filePath, text: jsonContents, title: "", lastFrame: lastFrame);
                    jsonContents = "";
                }

                else if (write && lastFrame)
                {
                    JsonFileWriter.WriteDataToFile(path: filePath, text: jsonContents, title: "", lastFrame: lastFrame);
                    jsonContents = "";
                    recording = false;
                    filePath = null;
                }
            }
            updatedVerticesThisFrame = false;
        }

        //json file prefix
        public string j_Prefix()
        {
            return "face";
        }
        #endregion

        #region Accessing Subsystem Data
        private static List<Vector3> s_Vertices;
        private MeshData GetMeshData(int f)
        {
            //empty mesh data container
            var mvd = new MeshData()
            {
                pos = new List<Vector3>(),
                frame = f
            };

            //return an empty container if no face is found
            if (!m_face)
            {
                return mvd;
            }

            //copy vertex data from native buffer buffer
            if (TryCopyToList(m_face.vertices, s_Vertices))
            {
                mvd.pos = s_Vertices;
            }

            updatedVerticesThisFrame = true;
            return mvd;
        }

        //copying native vector array from buffer
        static bool TryCopyToList<T>(NativeArray<T> array, List<T> list) where T : struct
        {
            list.Clear();
            if (!array.IsCreated || array.Length == 0)
                return false;

            foreach (var item in array)
                list.Add(item);

            return true;
        }
        #endregion
    }
}