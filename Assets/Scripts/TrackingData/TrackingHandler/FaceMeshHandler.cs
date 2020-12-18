using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
    public class FaceMeshHandler : MonoBehaviour, IInit, IGet<int>, IJson, IPrefix
    {
        public List<MeshData> m_meshDataList = new List<MeshData>();
        private MeshFilter meshFilter;
        private ARFaceManager m_faceManager;

        private void Start()
        {
            TrackingDataManager dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
            dataManager.SetRecorderReference(this.gameObject);
            m_faceManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARFaceManager>();
            m_faceManager.facesChanged += OnFaceUpdate;
        }

        private void OnDisable()
        {
            m_meshDataList.Clear();
            m_faceManager.facesChanged -= OnFaceUpdate;
        }

        private void OnFaceUpdate(ARFacesChangedEventArgs args)
        {
            if (args.added.Count > 0)
            {
                var faceObj = args.added[0].gameObject;
                meshFilter = faceObj.GetComponent<MeshFilter>();
            }

            if (args.removed.Count > 0)
            {
                meshFilter = null;
            }
        }

        //only works with a single face mesh
        public void Init()
        {
            m_meshDataList.Clear();
        }

        //getting verts at a frame
        public void GetFrameData(int f)
        {
            MeshData meshData = GetMeshData(meshFilter, f);
            m_meshDataList.Add(meshData);
        }

        //MeshDataContainer meshDataContainer = new MeshDataContainer();
        //tracked data to json
        public string GetJsonString()
        {
            MeshDataContainer meshDataContainer = new MeshDataContainer()
            {
                meshDataList = m_meshDataList
            };

            var json = JsonUtility.ToJson(meshDataContainer);
            return json;
        }

        //json file prefix
        public string GetJsonPrefix()
        {
            return "face";
        }

        public List<Vector> tmpList = new List<Vector>();
        public Vector3 tmp = new Vector3();
        public Vector mVert = new Vector();
        public MeshData GetMeshData(MeshFilter mf, int f)
        {
            //tmp list for verts in mesh
            var tmpList = new List<Vector>();

            //empty mesh data container
            var mvd = new MeshData()
            {
                frame = f
            };


            //if the face is lost
            if (!mf)
            {
                return mvd;
            }

            //getting mesh data from mesh filter
            for (int i = 0; i < mf.mesh.vertexCount; i++)
            {
                tmp = mf.mesh.vertices[i];
                mVert = DataHelper.GetVector(tmp);
                tmpList.Add(mVert);
            }

            //mesh data
            mvd.pos = tmpList;

            return mvd;
        }
    }
}