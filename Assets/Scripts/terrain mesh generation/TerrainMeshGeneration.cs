using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMeshGeneration : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    [SerializeField] int totalRectanglesOnX;
    [SerializeField] int totalRectanglesOnZ;
    [SerializeField] bool debugVertices;
    [SerializeField] Material meshMaterial;
    [SerializeField] Texture2D heightMap;
    [SerializeField] float heightScale;

    int totalVerticesOnX;
    int totalVerticesOnZ;
    int totalVertices;
    int indeciesPerRectangle = 6;
    int totalIndecies;

    Vector3[] verticesPositionArray;
    int[] indeciesArray;


    void Start()
    {
        InitializeReferences();
        InitializeValues();

        MakeVerticesAndSetScale();
        MakeIndeciesToMakeRectangle();

        SetVerticesToMeshFilter(verticesPositionArray);
        SetIndeciesToMeshFilterToDrawTriangles(indeciesArray);

    }

    private void InitializeReferences()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = meshMaterial;
    }

    private void InitializeValues()
    {
        totalVerticesOnX = totalRectanglesOnX + 1;
        totalVerticesOnZ = totalRectanglesOnZ + 1;
        totalVertices = totalVerticesOnX * totalVerticesOnZ;
        totalIndecies = indeciesPerRectangle * totalRectanglesOnX * totalRectanglesOnZ;

        indeciesArray = new int[totalIndecies];
        verticesPositionArray = new Vector3[totalVertices];
    }

    void MakeVerticesAndSetScale()
    {
        for (int z = 0; z < totalVerticesOnZ; z++)
        {
            for (int x = 0; x < totalVerticesOnX; x++)
            {
                int index = x + z * totalVerticesOnX;
                verticesPositionArray[index] = new Vector3(x, 0, z);
                ScaleVertexBasedOnTextureImagePixelColor(x, z, ref verticesPositionArray[index]);
            }
        }
    }

    void MakeIndeciesToMakeRectangle()
    {
        int currentIterationIndex =0; // this is the index of the current rectangle it has to make and loop iterationb
        int currentRowIndex =1; // this is the index of the current row of vertices it has to go to

        for (int indeciesSet = 0; indeciesSet < totalIndecies; indeciesSet+=indeciesPerRectangle)
        {
            // this goes and set  first 3 indexes of indecies to specefic vertices to make the first triangle 
            indeciesArray[indeciesSet + 0] = currentIterationIndex;
            indeciesArray[indeciesSet + 1 ]= currentIterationIndex + 1;
            indeciesArray[indeciesSet + 2 ]= currentIterationIndex + 1 +  totalVerticesOnX;
                          
            // this goes aixes of indecies to specefic vertices to make the first trainagle
            indeciesArray[indeciesSet + 3 ]= currentIterationIndex + 1 + totalVerticesOnX;
            indeciesArray[indeciesSet + 4 ]= currentIterationIndex + totalVerticesOnX;
            indeciesArray[indeciesSet + 5 ]= currentIterationIndex;
            // by line 70,  6 indecies have been set creating a rectangle;

            currentIterationIndex++;

            // this makes sure that we can go to the next row of vertices and set the indecies for the next row of rectangles 
            if(currentIterationIndex >= totalVerticesOnX *currentRowIndex -1)
            { 
                currentIterationIndex++;
                currentRowIndex++;
            }
        }
    }

    void ScaleVertexBasedOnTextureImagePixelColor(int x, int z,ref Vector3 vertex)
    {
        Color pixel = heightMap.GetPixel(x, z);
        float height = pixel.r * heightScale;
        vertex.y = height;
    }


    void SetVerticesToMeshFilter(Vector3[] setVertices)
    {
        meshFilter.mesh.vertices = setVertices;
    }

    void SetIndeciesToMeshFilterToDrawTriangles(int[] setIndicies)
    {
        meshFilter.mesh.triangles = setIndicies;
    }


   

    void DebugAndShowVertices()
    {
        if (!debugVertices) return;

        for (int z = 0; z < totalVerticesOnZ; z++)
        {
            for (int x = 0; x < totalVerticesOnX; x++)
            {
                int index = x + z * totalVerticesOnX;
                Gizmos.DrawSphere(new Vector3(x, 0, z), 0.1f);
            }
        }
    }


    private void OnDrawGizmos()
    {
        DebugAndShowVertices();
    }
}
