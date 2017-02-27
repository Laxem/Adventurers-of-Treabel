using UnityEngine;
using System.Collections;

public class ChunkHandler : MonoBehaviour
{

    private float positionX;
    private float positionZ;

    private float sizeChunk;
    private int nbSideSquare;
    

    public void InitChunk(float posX, float posZ, float size, int nbSquare)
    {
        sizeChunk = size;
        nbSideSquare = nbSquare;

        positionX = (Mathf.Round(posX / sizeChunk) * sizeChunk);
        positionZ = (Mathf.Round(posZ / sizeChunk) * sizeChunk);

        name = "Chunk" + positionX + "_" + positionZ;

        Vector3 pos = new Vector3(positionX-size/2, 0, positionZ - size / 2);
        transform.position = pos;

        Mesh m = CreatePlan(nbSideSquare + 1, sizeChunk / nbSideSquare);
        GetComponent<MeshFilter>().mesh = m;
        GetComponent<MeshCollider>().sharedMesh = m;
    }

    Mesh CreatePlan(int size, float sizeSquare)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[size * size];
        Vector3[] normals = new Vector3[size * size];
        int[] triangles = new int[(size - 1) * (size - 1) * 6];

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3((i % size) * sizeSquare, 0, (i / size) * sizeSquare);
        }

        for (int i = 0; i < triangles.Length / 6; i++)
        {

            triangles[i * 6] = i + i / (size - 1);
            triangles[i * 6 + 1] = i + i / (size - 1) + size;
            triangles[i * 6 + 2] = i + i / (size - 1) + 1;
            triangles[i * 6 + 3] = i + i / (size - 1) + 1;
            triangles[i * 6 + 4] = i + i / (size - 1) + size;
            triangles[i * 6 + 5] = i + i / (size - 1) + size + 1;
        }


        ShapingMap(size, sizeSquare, ref vertices, ref normals);

        mesh.vertices = vertices;
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x/(size*sizeSquare), vertices[i].z/(size*sizeSquare));
        }
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.normals = normals;

        return mesh;
    }


    void ShapingMap(int size, float sizeSquare, ref Vector3[] vertices, ref Vector3[] normals)
    {
        //Object[] h = FindObjectsOfType<MapHandler>();
        //Debug.Log(h.Length);
        float levelHeight = GameInfo.levelHeight;
        for (int i = 0; i < vertices.Length; i++)
        {
            int x = i % size;
            int z = i / (size);            
            float height = levelHeight * (Mathf.Sin(2 * Mathf.PI * x / (size-1)) + Mathf.Sin(2 * Mathf.PI * z / (size-1))) / 2;
            vertices[i] = new Vector3(vertices[i].x, height, vertices[i].z);
            normals[i] = new Vector3(-Mathf.Cos(2 * Mathf.PI * x / (size - 1)) , 1, -Mathf.Cos(2 * Mathf.PI * z / (size - 1)));
        }
    }

}