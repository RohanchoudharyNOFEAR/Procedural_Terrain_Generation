using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTerrainGenerator : MonoBehaviour
{
    public int Worldx;
    public int Worldz;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    Color[] colors;

    public Gradient gradient;
  [SerializeField]  float minTerrainHeight;
   [SerializeField] float maxTerrainHeight;

    private MeshCollider GetMeshCollider
    {
        get
        {
            return GetComponent<MeshCollider>();
        }
    }

    private MeshFilter GetMeshFilter
    {
        get
        {
            return GetComponent<MeshFilter>();
        }
    }

    private void Awake()
    {
        generateMesh();
    }

    void Start()
    {
        // generateMesh();
    }

    // Method that does our mesh stuff :)
    private void generateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "ProceduralMesh";

        mesh.Clear();

        triangles = new int[Worldx * Worldz * 6];
        vertices = new Vector3[(Worldx + 1) * (Worldz + 1)];

        for (int i = 0, z = 0; z <= Worldz; z++)
        {
            for (int x = 0; x <= Worldx; x++)
            {
                float y = (Mathf.PerlinNoise(x * .3f, z * .3f) * 2);
                vertices[i] = new Vector3(x, y /*Mathf.PerlinNoise(x * 2, z * .3f) / .3f*/, z);

                if (y > maxTerrainHeight)
                {
                    maxTerrainHeight = y;
                }
                if (y < minTerrainHeight)
                { minTerrainHeight = y; }

                i++;
            }
        }

        int tris = 0;
        int verts = 0;

        for (int z = 0; z < Worldz; z++)
        {
            for (int x = 0; x < Worldx; x++)
            {
                triangles[tris + 0] = verts + 0;
                triangles[tris + 1] = verts + Worldz + 1;
                triangles[tris + 2] = verts + 1;

                triangles[tris + 3] = verts + 1;
                triangles[tris + 4] = verts + Worldz + 1;
                triangles[tris + 5] = verts + Worldz + 2;

                verts++;
                tris += 6;
            }
            verts++;
        }

        uvs = new Vector2[vertices.Length];
        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= Worldz; z++)
        {
            for (int x = 0; x <= Worldx; x++)
            {
                uvs[i] = new Vector2((float)x / Worldx, (float)z / Worldz);

                float height = Mathf.InverseLerp(minTerrainHeight,maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }


        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;
        mesh.RecalculateNormals();

        GetMeshFilter.mesh = mesh;
        GetMeshCollider.sharedMesh = mesh;

    }
}


