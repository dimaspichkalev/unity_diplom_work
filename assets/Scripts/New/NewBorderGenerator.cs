using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class NewBorderGenerator : MonoBehaviour
{
    public GameObject prefab;
    public GameObject tire;
    public GameObject holder;
    public GameObject tireHolder;
    public float spacing = 3;

    Vector3[] borderPoints;

    public NewPlaneGenerator planeGenerator;

    const float minSpacing = .1f;

    public void GenerateBorder()
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
                Vector3 point = border_vp.GetPointAtDistance(dst) + new Vector3(0, planeGenerator.height, 0);
                Quaternion rot = /*Quaternion.identity * Quaternion.Euler(-90, 0, 0) */ border_vp.GetRotationAtDistance(dst);  // Quaternion.AngleAxis(90, Vector3.left) * 
                Instantiate(prefab, point, rot, holder.transform);
                dst += spacing;
            }
        }
    }

    public void GenerateTires()
    {
        if (planeGenerator.active && prefab != null && holder != null)
        {
            //DestroyObjects();

            Vector3[] tirePoints = new Vector3[]
            {
                new Vector3(0.25f, 0, 0.25f),
                new Vector3(0.25f, 0, planeGenerator.zSize - 0.25f),
                new Vector3(planeGenerator.xSize - 0.25f, 0, planeGenerator.zSize - 0.25f),
                new Vector3(planeGenerator.xSize - 0.25f, 0, 0),
            };

            BezierPath tire_path = new BezierPath(tirePoints, false, PathSpace.xz);
            tire_path.AutoControlLength = 0.01f;
            VertexPath tire_vp = new VertexPath(tire_path, transform, 0.1f, 1);

            spacing = Mathf.Max(minSpacing, spacing);
            float dst = 0;

            while (dst < tire_vp.length)
            {
                Vector3 point = tire_vp.GetPointAtDistance(dst);
                Quaternion rot = /*Quaternion.identity * Quaternion.Euler(-90, 0, 0) */ tire_vp.GetRotationAtDistance(dst);  // Quaternion.AngleAxis(90, Vector3.left) * 
                Instantiate(tire, point, rot, tireHolder.transform);
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
        numChildren = tireHolder.transform.childCount;
        for (int i = numChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(tireHolder.transform.GetChild(i).gameObject, false);
        }
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

