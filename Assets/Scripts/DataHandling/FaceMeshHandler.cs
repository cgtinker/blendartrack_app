using System.Collections.Generic;
using UnityEngine;
using ArRetarget;

public class FaceMeshHandler : MonoBehaviour
{
    [HideInInspector]
    public List<MeshData> meshDataList = new List<MeshData>();
    private MeshFilter meshFilter;
    private DataManager dataManager;

    private void Start()
    {
        dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<DataManager>();
        DeviceManager.Instance.SetDataType(DeviceManager.Capabilities.ArCore_FaceMesh);
        dataManager.AssignDataType();
    }

    //only works with a single face mesh
    public void InitFaceMesh()
    {
        var mesh = GameObject.FindGameObjectWithTag("face");
        Debug.Log("Searching Face Mesh");

        if (mesh != null)
            meshFilter = mesh.GetComponent<MeshFilter>();

        else
            Debug.LogWarning("Couldn't find a Facemesh");
    }

    public void ProcessMeshVerts(int f)
    {
        var meshData = GetMeshData(meshFilter, f);
        meshDataList.Add(meshData);
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