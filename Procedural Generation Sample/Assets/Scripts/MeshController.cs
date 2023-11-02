using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    [Range(1.5f, 5f)]
    public float radius = 2f;
    [Range(0.5f, 5f)]
    public float deformationStength = 2f;
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private Vector3[] verticies, modifiedVerts;


    void Start()
    {
        mesh = GetComponentInChildren<MeshFilter>().mesh;
        verticies = mesh.vertices;
        modifiedVerts = mesh.vertices;
    }

    void RecalculateMesh()
    {
        mesh.vertices = modifiedVerts;
        GetComponentInChildren<MeshCollider>().sharedMesh = mesh;
        mesh.RecalculateNormals();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
           // Debug.Log("left mouse");
           // modifiedVerts[v] = modifiedVerts[v] + (Vector3.up * force) / smoothingFactor;
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
           
            for (int v = 0; v < modifiedVerts.Length; v++)
            {
                Vector3 distance = modifiedVerts[v] - hit.point;
                float smoothingFactor = 2f;
                float force = deformationStength / (1f + hit.point.sqrMagnitude);
                if (distance.sqrMagnitude < radius)
                {
                    if (Input.GetMouseButton(0))
                    {
                        Debug.Log("left mouse");
                        modifiedVerts[v] = modifiedVerts[v] + (Vector3.up * force) / smoothingFactor;
                    }
                    else if (Input.GetMouseButton(1))
                    {
                        modifiedVerts[v] = modifiedVerts[v] + (Vector3.down * force) / smoothingFactor;
                    }
                }
            }
            RecalculateMesh();
        }
    }

}
