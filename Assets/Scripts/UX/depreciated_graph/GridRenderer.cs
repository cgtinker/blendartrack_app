using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridRenderer : Graphic
{
    public float thickness = 10.0f;
    public Vector2Int GridSize = new Vector2Int(1, 1);

    float width;
    float height;

    float cellWidth;
    float cellHeight;

    //unitys ui gets rendered in meshes aswell
    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        //clearing vertex helper cache
        vertexHelper.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        //based on total width / height / gridsize
        cellWidth = width / (float)GridSize.x;
        cellHeight = height / (float)GridSize.y;

        int count = 0;
        for (int y = 0; y < GridSize.y; y++)
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                DrawCell(x, y, count, vertexHelper);
                count++;
            }
        }

    }

    private void DrawCell(int x, int y, int index, VertexHelper vertexHelper)
    {
        //localize vertex pos
        float xPos = cellWidth * x;
        float yPos = cellHeight * y;


        //generate a vertex for each corner
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        //generating a rectangle
        vertex.position = new Vector2(xPos, yPos);
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector2(xPos, yPos + cellHeight);
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector2(xPos + cellWidth, yPos + cellHeight);
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector2(xPos + cellWidth, yPos);
        vertexHelper.AddVert(vertex);

        //connect corrsponding vertex numbers
        // vertexHelper.AddTriangle(0, 1, 2);
        // vertexHelper.AddTriangle(2, 3, 0);


        //frame distance
        float widthSqr = thickness * thickness;
        float distanceSqr = widthSqr / 2;
        float distance = Mathf.Sqrt(distanceSqr);

        //generating an innern rectangle
        vertex.position = new Vector2(xPos + distance, yPos + distance);
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector2(xPos + distance, yPos + cellHeight - distance);
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector2(xPos + cellWidth - distance, yPos + cellHeight - distance);
        vertexHelper.AddVert(vertex);

        vertex.position = new Vector2(xPos + cellWidth - distance, yPos + distance);
        vertexHelper.AddVert(vertex);

        int offset = index * 8;

        //triangles to connect innern shape
        //left edge
        vertexHelper.AddTriangle(0 + offset, 1 + offset, 5 + offset);
        vertexHelper.AddTriangle(5 + offset, 4 + offset, 0 + offset);

        //top edge
        vertexHelper.AddTriangle(1 + offset, 2 + offset, 6 + offset);
        vertexHelper.AddTriangle(6 + offset, 5 + offset, 1 + offset);

        //right edge
        vertexHelper.AddTriangle(2 + offset, 3 + offset, 7 + offset);
        vertexHelper.AddTriangle(7 + offset, 6 + offset, 2 + offset);

        //bottom edge
        vertexHelper.AddTriangle(3 + offset, 0 + offset, 4 + offset);
        vertexHelper.AddTriangle(4 + offset, 7 + offset, 3 + offset);
    }
}
