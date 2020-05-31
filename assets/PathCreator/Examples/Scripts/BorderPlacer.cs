using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class BorderPlacer : MonoBehaviour
{
    public GameObject prefab;
    public GameObject holder;
    public float spacing = 3;


    public Vector3[] borderPoints;

    public PlaneGenerator planeGenerator;

    const float minSpacing = .1f;

    void Generate()
    {
        if (planeGenerator.active && prefab != null && holder != null)
        {
            DestroyObjects();

            borderPoints = new Vector3[]
            {
                new Vector3(0.5f, 0, 0.5f),
                new Vector3(0.5f, 0, planeGenerator.zSize - 0.5f),
                new Vector3(planeGenerator.xSize - 0.5f, 0, planeGenerator.zSize - 0.5f),
                new Vector3(planeGenerator.xSize - 0.5f, 0, 0),
            };

            BezierPath border_path = new BezierPath(borderPoints, false, PathSpace.xz);
            border_path.AutoControlLength = 0.01f;
            VertexPath border_vp = new VertexPath(border_path, transform, 0.1f, 1);

            spacing = Mathf.Max(minSpacing, spacing);
            float dst = 0;

            while (dst < border_vp.length)
            {
                Vector3 point = border_vp.GetPointAtDistance(dst);
                Quaternion rot = Quaternion.identity * Quaternion.Euler(-90,0,0) * border_vp.GetRotationAtDistance(dst);  // Quaternion.AngleAxis(90, Vector3.left) * 
                Instantiate(prefab, point, rot, holder.transform);
                dst += spacing;
            }
        }
    }

    void DestroyObjects()
    {
        int numChildren = holder.transform.childCount;
        for (int i = numChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(holder.transform.GetChild(i).gameObject, false);
        }
    }

    private void Start()
    {
        Generate();
    }

    private void Update()
    {
        // Generate();
    }



    private void CalculateBorder()
    {
        //int size = 100;
        //int systemCount = (int)(100 / 27.5);

        // Идем по отрезкам
        //List<Segment> segments = new List<Segment> {
        //    new Segment(borderPoints[0], borderPoints[1]),
        //    new Segment(borderPoints[1], borderPoints[2]),
        //    new Segment(borderPoints[2], borderPoints[3]),
        //};



    }
}
