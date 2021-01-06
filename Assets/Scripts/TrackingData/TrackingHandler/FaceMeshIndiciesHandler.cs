using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;

namespace ArRetarget
{
    public class FaceMeshIndiciesHandler : MonoBehaviour, IInit<string, string>, IPrefix
    {
        private ARFace m_face;
        private ARFaceManager m_faceManager;
        private string filepath;

        private void Start()
        {
            m_faceManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARFaceManager>();
            m_faceManager.facesChanged += OnFaceUpdate;
            recording = false;
        }

        private void OnDisable()
        {
            //unsub from the ar face changes event
            m_faceManager.facesChanged -= OnFaceUpdate;
            recording = false;
        }

        private bool recording = false;
        private void OnFaceUpdate(ARFacesChangedEventArgs args)
        {
            //assign newly added ar face
            if (args.added.Count > 0 && !write)
            {
                var faceObj = args.added[0].gameObject;
                m_face = faceObj.GetComponent<ARFace>();
            }


            //unassign ar face when it's lost
            if (args.removed.Count > 0)
                m_face = null;


            if (recording)
                GetMeshIndices();
        }

        bool write = false;
        private void GetMeshIndices()
        {
            MeshGeometry meshGeometry = GetMeshGeometry();

            if (write)
            {
                string json = JsonUtility.ToJson(meshGeometry);

                JsonFileWriter.WriteDataToFile(path: filepath, text: json, title: "", lastFrame: true);
                recording = false;
                write = false;
            }
        }

        //only works with a single face mesh
        public void Init(string path, string title)
        {
            write = false;
            //init json file on disk
            filepath = $"{path}{title}_{j_Prefix()}.json";
            JsonFileWriter.WriteDataToFile(path: filepath, text: "", title: "meshGeometry", lastFrame: false);
            recording = true;
            Debug.Log("init face mesh indicies handler");
        }

        //json file prefix
        public string j_Prefix()
        {
            return "face_mesh";
        }

        static List<Vector3> s_Vertices = new List<Vector3>();
        static List<int> s_Indices = new List<int>();
        private MeshGeometry GetMeshGeometry()
        {
            MeshGeometry meshGeometry = new MeshGeometry();

            if (!m_face)
            {
                Debug.Log("no face??");
                return meshGeometry;
            }

            if (TryCopyToList(m_face.vertices, s_Vertices) && TryCopyToList(m_face.indices, s_Indices) && !write)
            {
                meshGeometry.pos = s_Vertices;
                meshGeometry.indices = s_Indices;
                write = true;
                Debug.Log("success?");
            }

            return meshGeometry;
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
    }
}