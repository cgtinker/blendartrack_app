using System.Collections.Generic;
using UnityEngine;
using ArRetarget;

public class FaceMeshHandler : MonoBehaviour
{
    [HideInInspector]
    public MeshVertsList meshVertsList;
    private MeshFilter meshFilter;

    public DataHandler dataHandler;

    private void Start()
    {
        dataHandler.SetDataType(DataHandler.RecData.ArCore_FaceMesh);
    }

    public void InitFaceMesh()
    {
        var mesh = GameObject.FindGameObjectWithTag("FaceMesh");
        Debug.Log("Searching Face Mesh");

        if (mesh != null)
        {
            meshVertsList.meshVertsList.Clear();
            meshFilter = mesh.GetComponent<MeshFilter>();

            meshVertsList = new MeshVertsList();
        }

        else
            Debug.LogWarning("Couldnt find a Facemesh");
    }

    public void ProcessMeshVerts(int f)
    {
        var meshData = new List<vector>();

        //getting mesh data
        for (int i = 0; i < meshFilter.mesh.vertexCount; i++)
        {
            Vector3 tmp = meshFilter.mesh.vertices[i];

            var meshVert = new vector()
            {
                x = tmp.x,
                y = tmp.y,
                z = tmp.z
            };

            meshData.Add(meshVert);
        }

        //fetching frame
        var meshVerts = new MeshVerts()
        {
            meshVerts = meshData,
            frame = f
        };

        //adding data to list
        meshVertsList.meshVertsList.Add(meshVerts);
    }
}