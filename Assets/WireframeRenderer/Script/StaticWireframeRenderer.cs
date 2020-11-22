using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once UnusedMember.Global
[RequireComponent( typeof( MeshFilter ) )]
public class StaticWireframeRenderer: MonoBehaviour {
    
    private const float Distance = 1.0001f;
    private static readonly Color Color = Color.black;
    private List<Vector3> _renderingQueue;

    // ReSharper disable once MemberCanBePrivate.Global
    public Material WireMaterial;

    private void InitializeOnDemand() {
        if (_renderingQueue != null) {
            return;
        }
        var meshFilter = gameObject.GetComponent<MeshFilter>();
        if ( meshFilter == null ) {
            Debug.LogError( "No mesh detected at" + gameObject.name, gameObject );
            return;
        }
        var mesh = meshFilter.mesh;

        _renderingQueue = new List<Vector3>();
        foreach (var point in mesh.triangles) {
            _renderingQueue.Add( mesh.vertices[point] * Distance );
        }
    }

    // ReSharper disable once UnusedMember.Global
    public void OnPreRender() {
        GL.wireframe = true;
    }

    // ReSharper disable once UnusedMember.Global
    public void OnRenderObject() {
        InitializeOnDemand();

        if (WireMaterial != null) {
            WireMaterial.SetPass(0);
        } else {
            var color = Color;
            GL.Color( color );
        }

        GL.MultMatrix( transform.localToWorldMatrix );
        GL.Begin( GL.LINES );

        for ( var i = 0; i < _renderingQueue.Count; i+=3 ) {
            var vertex1 = _renderingQueue[i];
            var vertex2 = _renderingQueue[i+1];
            var vertex3 = _renderingQueue[i+2];
            GL.Vertex3( vertex1.x, vertex1.y, vertex1.z );
            GL.Vertex3( vertex2.x, vertex2.y, vertex2.z );
            GL.Vertex3( vertex2.x, vertex2.y, vertex2.z );
            GL.Vertex3( vertex3.x, vertex3.y, vertex3.z );
            GL.Vertex3( vertex3.x, vertex3.y, vertex3.z );
            GL.Vertex3( vertex1.x, vertex1.y, vertex1.z );
        }
        GL.End();
    }

    // ReSharper disable once UnusedMember.Global
    public void OnPostRender() {
        GL.wireframe = false;
    }

}
