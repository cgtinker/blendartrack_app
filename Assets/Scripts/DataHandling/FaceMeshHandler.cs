﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ArRetarget
{

    public class FaceMeshHandler : MonoBehaviour, IInit, IGet<int>, IJson, IPrefix
    {
        [HideInInspector]
        private List<MeshData> meshDataList = new List<MeshData>();
        private MeshFilter meshFilter;

        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            TrackingDataManager dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
            dataManager.TrackingReference(this.gameObject);
        }

        //only works with a single face mesh
        public void Init()
        {
            var mesh = GameObject.FindGameObjectWithTag("face");
            Debug.Log("Searching Face Mesh");

            if (mesh != null)
                meshFilter = mesh.GetComponent<MeshFilter>();

            else
                Debug.LogWarning("Couldn't find a Facemesh");
        }

        //getting verts at a frame
        public void GetFrameData(int f)
        {
            var meshData = GetMeshData(meshFilter, f);
            meshDataList.Add(meshData);
        }

        //tracked data to json
        public string GetJsonString()
        {
            MeshDataContainer meshDataContainer = new MeshDataContainer()
            {
                meshDataList = meshDataList
            };

            var json = JsonUtility.ToJson(meshDataContainer);
            return json;
        }

        //json file prefix
        public string GetJsonPrefix()
        {
            return "FM";
        }


        public static MeshData GetMeshData(MeshFilter mf, int f)
        {
            //tmp list for verts in mesh
            var tmpList = new List<Vector>();

            //getting mesh data
            for (int i = 0; i < mf.mesh.vertexCount; i++)
            {
                Vector3 tmp = mf.mesh.vertices[i];

                var mVert = new Vector()
                {
                    x = tmp.x,
                    y = tmp.y,
                    z = tmp.z
                };

                tmpList.Add(mVert);
            }

            //assigning frame and mesh data
            var mvd = new MeshData()
            {
                pos = tmpList,
                frame = f
            };

            return mvd;
        }
    }
}