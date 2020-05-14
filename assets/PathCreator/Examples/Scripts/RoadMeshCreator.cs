using System.Collections.Generic;
using PathCreation.Utility;
using UnityEngine;

namespace PathCreation.Examples {
    public class RoadMeshCreator : PathSceneTool {
        [Header ("Road settings")]
        public float roadWidth = .4f;
        [Range (0, .5f)]
        public float thickness = .15f;
        public bool flattenSurface;

        [Header ("Material settings")]
        public Material roadMaterial;
        public Material undersideMaterial;
        public float textureTiling = 1;

        [SerializeField, HideInInspector]
        GameObject meshHolder;


        public int numroads = 2;
        public PlaneGenerator planeGenerator;
        VertexPath vpath;
        int currentroad = 0;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        Mesh mesh;

        protected override void PathUpdated () {
            if (pathCreator != null) {
                if (planeGenerator.active)
                {
                    currentroad = 0;
                    for (int i = 0; i < numroads; i++)
                    {
                        vpath = CreatePoints();
                        AssignMeshComponents();
                        AssignMaterials();
                        CreateRoadMesh();
                        currentroad += 1;

                    }
                }

            }
        }

        VertexPath CreatePoints()
        {
            BezierPath bpath = new BezierPath(Vector3.one);
            Vector3 minVector = Vector3.positiveInfinity;
            Vector3 maxVector = Vector3.zero;

            for (int i = 0; i < planeGenerator.vertices.Length; i++)
            {
                minVector = (planeGenerator.vertices[i].magnitude < minVector.magnitude) ? planeGenerator.vertices[i] : minVector;
                maxVector = (planeGenerator.vertices[i].magnitude > maxVector.magnitude) ? planeGenerator.vertices[i] : maxVector;
            }
            Debug.Log(minVector.x);
            Debug.Log(new Vector3(maxVector.x,0,0));
            Vector3[] points = new Vector3[]
                {
                    new Vector3(5, 0, maxVector.z - 10 - currentroad * 30),
                    new Vector3(maxVector.x * 0.7f, 0, maxVector.z - 10 - currentroad * 30),
                    new Vector3(maxVector.x * 0.95f, 0, maxVector.z - 15 - currentroad * 30),
                    new Vector3(maxVector.x, 0, 0),
                };
            for (int i = 0; i < points.Length; i++)
            {
                Debug.Log("Plane vector " + i.ToString() + points[i].ToString());
            }
            if (points.Length > 0)
            {
                // Create a new bezier path from the waypoints.
                bpath = new BezierPath(points, false, PathSpace.xz);
                bpath.ControlPointMode = BezierPath.ControlMode.Aligned;
            }
            return new VertexPath(bpath, pathCreator.transform, 1);
        }

        void CreateRoadMesh () {
            Vector3[] verts = new Vector3[vpath.NumPoints * 8];
            Vector2[] uvs = new Vector2[verts.Length];
            Vector3[] normals = new Vector3[verts.Length];

            int numTris = 2 * (vpath.NumPoints - 1) + ((vpath.isClosedLoop) ? 2 : 0);
            int[] roadTriangles = new int[numTris * 3];
            int[] underRoadTriangles = new int[numTris * 3];
            int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

            int vertIndex = 0;
            int triIndex = 0;

            // Vertices for the top of the road are layed out:
            // 0  1
            // 8  9
            // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
            int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
            int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

            bool usePathNormals = !(vpath.space == PathSpace.xyz && flattenSurface);

            for (int i = 0; i < vpath.NumPoints; i++) {
                Vector3 localUp = (usePathNormals) ? Vector3.Cross (vpath.GetTangent (i), vpath.GetNormal (i)) : vpath.up;
                Vector3 localRight = (usePathNormals) ? vpath.GetNormal (i) : Vector3.Cross (localUp, vpath.GetTangent (i));

                // Find position to left and right of current path vertex
                Vector3 vertSideA = vpath.GetPoint (i) - localRight * Mathf.Abs (roadWidth);
                Vector3 vertSideB = vpath.GetPoint (i) + localRight * Mathf.Abs (roadWidth);

                // Add top of road vertices
                verts[vertIndex + 0] = vertSideA;
                verts[vertIndex + 1] = vertSideB;
                // Add bottom of road vertices
                verts[vertIndex + 2] = vertSideA - localUp * thickness;
                verts[vertIndex + 3] = vertSideB - localUp * thickness;

                // Duplicate vertices to get flat shading for sides of road
                verts[vertIndex + 4] = verts[vertIndex + 0];
                verts[vertIndex + 5] = verts[vertIndex + 1];
                verts[vertIndex + 6] = verts[vertIndex + 2];
                verts[vertIndex + 7] = verts[vertIndex + 3];

                // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                uvs[vertIndex + 0] = new Vector2 (0, vpath.times[i]);
                uvs[vertIndex + 1] = new Vector2 (1, vpath.times[i]);

                // Top of road normals
                normals[vertIndex + 0] = localUp;
                normals[vertIndex + 1] = localUp;
                // Bottom of road normals
                normals[vertIndex + 2] = -localUp;
                normals[vertIndex + 3] = -localUp;
                // Sides of road normals
                normals[vertIndex + 4] = -localRight;
                normals[vertIndex + 5] = localRight;
                normals[vertIndex + 6] = -localRight;
                normals[vertIndex + 7] = localRight;

                // Set triangle indices
                if (i < vpath.NumPoints - 1 || vpath.isClosedLoop) {
                    for (int j = 0; j < triangleMap.Length; j++) {
                        roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                        // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                        underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                    }
                    for (int j = 0; j < sidesTriangleMap.Length; j++) {
                        sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                    }

                }

                vertIndex += 8;
                triIndex += 6;
            }

            mesh.Clear ();
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.normals = normals;
            mesh.subMeshCount = 3;
            mesh.SetTriangles (roadTriangles, 0);
            mesh.SetTriangles (underRoadTriangles, 1);
            mesh.SetTriangles (sideOfRoadTriangles, 2);
            mesh.RecalculateBounds ();
        }

        // Add MeshRenderer and MeshFilter components to this gameobject if not already attached
        void AssignMeshComponents () {

            meshHolder = GameObject.Find("Road Mesh Holder " + currentroad.ToString());
            if (meshHolder == null)
            {
                meshHolder = new GameObject("Road Mesh Holder " + currentroad.ToString());
            } 
            
            meshHolder.transform.rotation = Quaternion.identity;
            meshHolder.transform.position = new Vector3(0, 0.1f, 0);
            meshHolder.transform.localScale = Vector3.one;

            // Ensure mesh renderer and filter components are assigned
            if (!meshHolder.gameObject.GetComponent<MeshFilter> ()) {
                meshHolder.gameObject.AddComponent<MeshFilter> ();
            }
            if (!meshHolder.GetComponent<MeshRenderer> ()) {
                meshHolder.gameObject.AddComponent<MeshRenderer> ();
            }

            meshRenderer = meshHolder.GetComponent<MeshRenderer> ();
            meshFilter = meshHolder.GetComponent<MeshFilter> ();
            mesh = new Mesh ();
            meshFilter.sharedMesh = mesh;
        }

        void AssignMaterials () {
            if (roadMaterial != null && undersideMaterial != null) {
                meshRenderer.sharedMaterials = new Material[] { roadMaterial, undersideMaterial, undersideMaterial };
                meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3 (1, textureTiling);
            }
        }

    }
}