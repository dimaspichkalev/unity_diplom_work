using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PlaneGenerator : MonoBehaviour
{
    Mesh mesh;

    [HideInInspector]
    public Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    public int xSize;
    public int zSize;
    public string portType;
    public Material material;
    public bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        xSize = StaticValue.xSize;
        zSize = StaticValue.zSize;
        portType = StaticValue.portType;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
        active = true;
    }

    private void Update()
    {
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        Random random = new Random();
        string[] list = new string[] { "Речной", "Морской"};
        int index;

        if (portType == "Любой")
        {
            index = Random.Range(0, list.Length);
            portType = list[index];
        }

        if (portType == "Речной")
        {
            CreateShapeBySize();
        }
        else if (portType == "Морской")
        {
            Vector2[] vertices2D = new Vector2[] {
                new Vector2(0,0),
                new Vector2(0,75),
                new Vector2(150,75),
                new Vector2(150,175),
                new Vector2(0,175),
                new Vector2(0,250),
                new Vector2(200,250),
                new Vector2(200,0),
            };
            // Use the triangulator to get indices for creating triangles
            Triangulator tr = new Triangulator(vertices2D);
            triangles = tr.Triangulate();

            // Create the Vector3 vertices
            vertices = new Vector3[vertices2D.Length];
            uv = new Vector2[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, 0, vertices2D[i].y);
                uv[i] = new Vector2(vertices2D[i].x / (float)50, vertices2D[i].y / (float)40);

            }
        }

    }

    void CreateShapeBySize()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        uv = new Vector2[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                uv[i] = new Vector2(x / (float)xSize, z / (float)zSize);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }

}
