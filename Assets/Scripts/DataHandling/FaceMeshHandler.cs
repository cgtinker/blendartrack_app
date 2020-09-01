using System.Collections.Generic;
using UnityEngine;
using ArRetarget;

public class FaceMeshHandler : MonoBehaviour
{
    [HideInInspector]
    public List<MeshVertData> meshVertsList = new List<MeshVertData>();
    private MeshFilter meshFilter;
    private DataManager dataManager;

    private void Start()
    {
        dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<DataManager>();
        dataManager.SetDataType(DataManager.RecData.ArCore_FaceMesh);
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
        //tmp list for verts in mesh
        var tmpList = new List<Vector>();

        //getting mesh data
        for (int i = 0; i < meshFilter.mesh.vertexCount; i++)
        {
            Vector3 tmp = meshFilter.mesh.vertices[i];

            var meshVert = new Vector()
            {
                x = tmp.x,
                y = tmp.y,
                z = tmp.z
            };

            tmpList.Add(meshVert);
        }

        //assigning frame and mesh data
        var meshVertData = new MeshVertData()
        {
            meshVerts = tmpList,
            frame = f
        };

        //adding data to list
        meshVertsList.Add(meshVertData);
    }
}