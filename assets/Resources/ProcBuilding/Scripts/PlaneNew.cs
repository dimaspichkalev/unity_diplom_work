using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProBuilder.Examples
{
    [RequireComponent(typeof(MeshFilter))]
    public class PlaneNew : MonoBehaviour
    {
        Mesh mesh;

        [HideInInspector]
        public Vector3[] vertices;
        Vector2[] uv;
        int[] triangles;

        public int xSize;
        public int zSize;
        public enum PortType
        {
            Любой, Речной, Морской
        }
        public PortType portType;
        public bool active = false;

        public Material ground;
        public Material underground;


        // Start is called before the first frame update
        void Start()
        {
            //xSize = StaticValue.xSize;
            //zSize = StaticValue.zSize;
            //portType = StaticValue.portType;
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            CreateShape();
            UpdateMesh();
            active = true;
        }

        void CreateShape()
        {
            Random random = new Random();
            string[] list = new string[] { "Речной", "Морской" };
            int index;

            if (portType == PortType.Любой)
            {
                index = Random.Range(0, list.Length);
                portType = (PortType)Random.Range(1, 2); ;
            }

            if (portType == PortType.Речной)
            {
                Vector3[] points = new Vector3[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(100, 0, 0),
                    new Vector3(100, 0, 200),
                    new Vector3(0, 0, 200),
                };
                CreatePolyShape.CreatePortPlane(points, ground, underground);
                //CreateShapeBySize();
            }
            else if (portType == PortType.Морской)
            {
                Vector3[] points = new Vector3[]
                {
                    new Vector3(0, 0, 0),
                    new Vector3(0, 0, 75),
                    new Vector3(150, 0, 75),
                    new Vector3(150, 0, 175),
                    new Vector3(0, 0, 175),
                    new Vector3(0, 0, 250),
                    new Vector3(200, 0, 250),
                    new Vector3(200, 0, 0)
                };
                CreatePolyShape.CreatePortPlane(points, ground, underground);
                //Vector2[] vertices2D = new Vector2[] {
                //new Vector2(0,0),
                //new Vector2(0,75),
                //new Vector2(150,75),
                //new Vector2(150,175),
                //new Vector2(0,175),
                //new Vector2(0,250),
                //new Vector2(200,250),
                //new Vector2(200,0),
                //   };

                /*          
                0, 0, 0
                0, 0, 75
                150, 0, 75
                150, 0, 175
                0, 0, 175
                0, 0, 250
                200, 0, 250
                200, 0, 0
                 *          */

                //// Use the triangulator to get indices for creating triangles
                //Triangulator tr = new Triangulator(vertices2D);
                //triangles = tr.Triangulate();

                //// Create the Vector3 vertices
                //vertices = new Vector3[vertices2D.Length];
                //uv = new Vector2[vertices.Length];

                //for (int i = 0; i < vertices.Length; i++)
                //{
                //    vertices[i] = new Vector3(vertices2D[i].x, 0, vertices2D[i].y);
                //    uv[i] = new Vector2(vertices2D[i].x / 50.0f, vertices2D[i].y / 40.0f);

                //}
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
}