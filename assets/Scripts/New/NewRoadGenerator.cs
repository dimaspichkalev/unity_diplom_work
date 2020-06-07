using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;

public class NewRoadGenerator : MonoBehaviour
{
    [Header("Road settings")]
    public bool flattenSurface;

    [SerializeField, HideInInspector]
    GameObject meshHolder;

    public NewPlaneGenerator planeGenerator;
    int numroads;
    int currentroad = 0;
    int widthSystem = 45;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Mesh mesh;
    public List<Vector3[]> freeZonesList = new List<Vector3[]>();


    public void GenerateRoads()
    {
        if (planeGenerator.active)
        {
            Clear();
            numroads = planeGenerator.zSize / widthSystem;
            currentroad = 0;
            for (int i = 0; i < numroads; i++)
            {
                VertexPath[] pathList = CreateAllRoadPoints();
                CreateRails(pathList[0], pathList[1]);
                CreateCraneRails(pathList[2], pathList[3]);
                CreateAutoRoads(pathList[4]);
                currentroad += 1;
            }
            CreateFreeZonesMesh();
        }
    }

    void CreateRails(VertexPath railPath1, VertexPath railPath2)
    {
        Material roadMaterial = Resources.Load("Materials/Rails", typeof(Material)) as Material;
        Material undersideMaterial = Resources.Load("Materials/Road Underside", typeof(Material)) as Material;
        AssignMeshComponents("Rail First");
        AssignMaterials(roadMaterial, undersideMaterial, 75);
        CreateRoadMesh(railPath1, 2f, 0.15f);
        AssignMeshComponents("Rail Second");
        AssignMaterials(roadMaterial, undersideMaterial, 75);
        CreateRoadMesh(railPath2, 2f, 0.15f);
    }

    void CreateCraneRails(VertexPath railPath1, VertexPath railPath2)
    {
        Material roadMaterial = Resources.Load("Materials/CraneRails", typeof(Material)) as Material;
        Material undersideMaterial = Resources.Load("Materials/Road Underside", typeof(Material)) as Material;
        AssignMeshComponents("Crane Rail First");
        AssignMaterials(roadMaterial, undersideMaterial, 1);
        CreateRoadMesh(railPath1, 0.25f, 0.15f);
        AssignMeshComponents("Crane Rail Second");
        AssignMaterials(roadMaterial, undersideMaterial, 1);
        CreateRoadMesh(railPath2, 0.25f, 0.15f);
    }

    void CreateAutoRoads(VertexPath autoPath)
    {
        Material roadMaterial = Resources.Load("Materials/Tire9_Material", typeof(Material)) as Material;
        Material undersideMaterial = Resources.Load("Materials/Road Underside", typeof(Material)) as Material;
        AssignMeshComponents("Auto Road");
        AssignMaterials(roadMaterial, undersideMaterial, 1);
        CreateRoadMesh(autoPath, 2.1f, 0);
    }

    VertexPath[] CreateAllRoadPoints()
    {
        Vector3 maxVector = new Vector3(planeGenerator.xSize, 0, planeGenerator.zSize);

        Vector3[] railPoints_1 = new Vector3[]
            {
                    //new Vector3(5, 0, 0),
                    //new Vector3(10, 0, maxVector.z - 10 - currentroad * widthSystem),
                    //new Vector3(25, 0, maxVector.z - 5 - currentroad * widthSystem),
                    //new Vector3(maxVector.x * 0.7f, 0, maxVector.z - 5 - currentroad * widthSystem),
                    //new Vector3(maxVector.x * 0.9f, 0, maxVector.z - 10 - currentroad * widthSystem),
                    //new Vector3(maxVector.x - 5, 0, 0),
                    new Vector3(5, 0, 0),
                    new Vector3(5, 0, maxVector.z - 45 - currentroad * widthSystem),
                    new Vector3(6, 0, maxVector.z - 22.5f - currentroad * widthSystem),
                    new Vector3(12.5f, 0, maxVector.z - 12.5f - currentroad * widthSystem),
                    new Vector3(19, 0, maxVector.z - 10 - currentroad * widthSystem),
                    new Vector3(21.5f, 0, maxVector.z - 10 - currentroad * widthSystem),

                    new Vector3(maxVector.x - 21.5f, 0, maxVector.z - 10 - currentroad * widthSystem),
                    new Vector3(maxVector.x - 19, 0, maxVector.z - 10 - currentroad * widthSystem),
                    new Vector3(maxVector.x - 12.5f, 0, maxVector.z - 12.5f - currentroad * widthSystem),
                    new Vector3(maxVector.x - 6, 0, maxVector.z - 22.5f - currentroad * widthSystem),
                    new Vector3(maxVector.x - 5, 0, maxVector.z - 45 - currentroad * widthSystem),
                    new Vector3(maxVector.x - 5, 0, 0),
            };
        BezierPath bezierPathRail_1 = new BezierPath(railPoints_1, false, PathSpace.xz);
        bezierPathRail_1.AutoControlLength = 0.3f;
        VertexPath railRoadPath_1 = new VertexPath(bezierPathRail_1, transform, 2.5f, 1);

        Vector3[] railPoints_2 = new Vector3[]
         {
                railPoints_1[0] + new Vector3(5f, 0, 0),
                railPoints_1[1] + new Vector3(5f, 0, 0),
                railPoints_1[2] + new Vector3(5f, 0, -5f),
                railPoints_1[3] + new Vector3(5f, 0, -5f),
                railPoints_1[4] + new Vector3(5f, 0, -5f),
                railPoints_1[5] + new Vector3(5f, 0, -5f),
                railPoints_1[6] + new Vector3(-5f, 0, -5f),
                railPoints_1[7] + new Vector3(-5f, 0, -5f),
                railPoints_1[8] + new Vector3(-5f, 0, -5f),
                railPoints_1[9] + new Vector3(-5f, 0, -5f),
                railPoints_1[10] + new Vector3(-5f, 0, 0),
                railPoints_1[11] + new Vector3(-5f, 0, 0),
         };
        BezierPath bezierPathRail_2 = new BezierPath(railPoints_2, false, PathSpace.xz);
        bezierPathRail_2.AutoControlLength = 0.3f;
        VertexPath railRoadPath_2 = new VertexPath(bezierPathRail_2, transform, 2.5f, 1);

        Vector3[] craneRailPoints1 = new Vector3[]
            {
                    new Vector3(railPoints_1[5].x + 5f, 0, railPoints_1[5].z + 2.5f),
                    new Vector3(railPoints_1[6].x - 5f, 0, railPoints_1[5].z + 2.5f),
            };
        BezierPath bezierPathCrane1 = new BezierPath(craneRailPoints1, false, PathSpace.xz);
        bezierPathCrane1.AutoControlLength = 0.01f;
        VertexPath craneRailRoadPath1 = new VertexPath(bezierPathCrane1, transform, 45f, 1);

        Vector3[] craneRailPoints2 = new Vector3[]
            {
                    new Vector3(railPoints_2[5].x, 0, railPoints_2[5].z - 2.5f),
                    new Vector3(railPoints_2[6].x, 0, railPoints_2[5].z - 2.5f),
            };
        BezierPath bezierPathCrane2 = new BezierPath(craneRailPoints2, false, PathSpace.xz);
        bezierPathCrane2.AutoControlLength = 0.01f;
        VertexPath craneRailRoadPath2 = new VertexPath(bezierPathCrane2, transform, 45f, 1);

        Vector3[] autoRoadPoints = new Vector3[]
            {
                    new Vector3(10, 0, 0),
                    new Vector3(15, 0, maxVector.z - 27.5f - currentroad * widthSystem),
                    new Vector3(18.5f, 0, maxVector.z - 21 - currentroad * widthSystem),
                    new Vector3(25, 0, maxVector.z - 17.5f - currentroad * widthSystem),
                    new Vector3(maxVector.x * 0.75f, 0, maxVector.z - 17.5f - currentroad * widthSystem),
                    new Vector3(maxVector.x * 0.815f, 0, maxVector.z - 21 - currentroad * widthSystem),
                    new Vector3(maxVector.x * 0.85f, 0, maxVector.z - 27.5f - currentroad * widthSystem),
                    new Vector3(maxVector.x * 0.9f, 0, 0),
            };
        BezierPath bezierAuto = new BezierPath(autoRoadPoints, false, PathSpace.xz);
        bezierAuto.AutoControlLength = 0.15f;
        VertexPath autoRoadPath = new VertexPath(bezierAuto, transform, 2.5f, 1);

        Vector3[] freeSpacePoints = new Vector3[]
        {
            new Vector3(autoRoadPoints[3].x, 0, autoRoadPoints[3].z - 5),
            new Vector3(autoRoadPoints[4].x, 0, autoRoadPoints[4].z - 5),
            new Vector3(autoRoadPoints[4].x, 0, autoRoadPoints[4].z - 27.5f),
            new Vector3(autoRoadPoints[3].x, 0, autoRoadPoints[3].z - 27.5f)
        };
        freeZonesList.Add(freeSpacePoints);

        return new VertexPath[5] { railRoadPath_1, railRoadPath_2, craneRailRoadPath1, craneRailRoadPath2, autoRoadPath };
    }


    void CreateFreeZonesMesh()
    {
        int i = 0;
        foreach (Vector3[] zone in freeZonesList)
        {
            BezierPath b_path = new BezierPath(zone, false, PathSpace.xz);
            b_path.AutoControlLength = 0.01f;
            VertexPath v_path = new VertexPath(b_path, transform, 45, 1);
            Material roadMaterial = Resources.Load("Materials/Tire9_Material", typeof(Material)) as Material;
            Material undersideMaterial = Resources.Load("Materials/Road Underside", typeof(Material)) as Material;
            AssignMeshComponents("Free Zone Road" + i++);
            AssignMaterials(roadMaterial, undersideMaterial, 1);
            CreateRoadMesh(v_path, 1, 0);
        }
    }

    void CreateRoadMesh(VertexPath vpath, float roadWidth, float thickness)
    {
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

        for (int i = 0; i < vpath.NumPoints; i++)
        {
            Vector3 localUp = (usePathNormals) ? Vector3.Cross(vpath.GetTangent(i), vpath.GetNormal(i)) : vpath.up;
            Vector3 localRight = (usePathNormals) ? vpath.GetNormal(i) : Vector3.Cross(localUp, vpath.GetTangent(i));

            // Find position to left and right of current path vertex
            Vector3 vertSideA = vpath.GetPoint(i) - localRight * Mathf.Abs(roadWidth);
            Vector3 vertSideB = vpath.GetPoint(i) + localRight * Mathf.Abs(roadWidth);

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
            uvs[vertIndex + 0] = new Vector2(0, vpath.times[i]);
            uvs[vertIndex + 1] = new Vector2(1, vpath.times[i]);

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
            if (i < vpath.NumPoints - 1 || vpath.isClosedLoop)
            {
                for (int j = 0; j < triangleMap.Length; j++)
                {
                    roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                    // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                    underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                }
                for (int j = 0; j < sidesTriangleMap.Length; j++)
                {
                    sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                }

            }

            vertIndex += 8;
            triIndex += 6;
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.subMeshCount = 3;
        mesh.SetTriangles(roadTriangles, 0);
        mesh.SetTriangles(underRoadTriangles, 1);
        mesh.SetTriangles(sideOfRoadTriangles, 2);
        mesh.RecalculateBounds();
    }

    // Add MeshRenderer and MeshFilter components to this gameobject if not already attached
    void AssignMeshComponents(string meshName)
    {

        meshHolder = GameObject.Find("Road Mesh Holder " + meshName + " " + currentroad.ToString());
        if (meshHolder == null)
        {
            meshHolder = new GameObject("Road Mesh Holder " + meshName + " " + currentroad.ToString());
        }

        meshHolder.transform.rotation = Quaternion.identity;
        meshHolder.transform.position = (meshName == "Rail") ? new Vector3(0, 0.2f, 0) : new Vector3(0, 0.1f, 0) + new Vector3(0, planeGenerator.height, 0);
        meshHolder.transform.localScale = Vector3.one;

        // Ensure mesh renderer and filter components are assigned
        if (!meshHolder.gameObject.GetComponent<MeshFilter>())
        {
            meshHolder.gameObject.AddComponent<MeshFilter>();
        }
        if (!meshHolder.GetComponent<MeshRenderer>())
        {
            meshHolder.gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer = meshHolder.GetComponent<MeshRenderer>();
        meshFilter = meshHolder.GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.sharedMesh = mesh;
    }

    void AssignMaterials(Material roadMaterial, Material undersideMaterial, int textureTilling)
    {
        if (roadMaterial != null && undersideMaterial != null)
        {
            meshRenderer.sharedMaterials = new Material[] { roadMaterial, undersideMaterial, undersideMaterial };
            meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1, textureTilling);
        }
    }

    void Clear()
    {
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.StartsWith("Road Mesh"));
        foreach (GameObject obj in objects)
        {
            DestroyImmediate(obj);
        }
    }

}
