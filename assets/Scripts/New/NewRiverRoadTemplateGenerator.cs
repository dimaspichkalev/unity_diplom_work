using UnityEngine;
using System.Collections;
using PathCreation;

public class NewRiverRoadTemplateGenerator
{
    public static VertexPath[] GenerateCranePoints(float zLevel, float xMax, bool deadEndLeft, bool deadEndRight, Transform transform)
    {
        Vector3[] craneRailPoints1 = new Vector3[2]
            {
                    new Vector3(deadEndLeft ? 5 : 25, 0, zLevel - 5),
                    new Vector3(deadEndRight ? xMax - 5 : xMax - 25, 0, zLevel - 5),
            };
        BezierPath bezierPathCrane1 = new BezierPath(craneRailPoints1, false, PathSpace.xz);
        bezierPathCrane1.AutoControlLength = 0.01f;
        VertexPath craneRailRoadPath1 = new VertexPath(bezierPathCrane1, transform, 45f, 1);

        Vector3[] craneRailPoints2 = new Vector3[2]
            {
                    new Vector3(deadEndLeft ? 5 : 25, 0, zLevel - 15),
                    new Vector3(deadEndRight ? xMax - 5 : xMax - 25, 0, zLevel - 15),
            };
        BezierPath bezierPathCrane2 = new BezierPath(craneRailPoints2, false, PathSpace.xz);
        bezierPathCrane2.AutoControlLength = 0.01f;
        VertexPath craneRailRoadPath2 = new VertexPath(bezierPathCrane2, transform, 45f, 1);

        return new VertexPath[2] { craneRailRoadPath1, craneRailRoadPath2 };
    }

    public static VertexPath GenerateAutoPoints(float zLevel, float xMax, Transform transform)
    {
        Vector3[] autoRoadPoints = new Vector3[8]
        {
            new Vector3(10, 0, 0),
            new Vector3(15, 0, zLevel - 16.5f),
            new Vector3(18.5f, 0, zLevel - 10),
            new Vector3(25, 0, zLevel - 7.5f),
            new Vector3(xMax - 25, 0, zLevel - 7.5f),
            new Vector3(xMax - 18.5f, 0, zLevel - 10),
            new Vector3(xMax - 15, 0, zLevel - 16.5f),
            new Vector3(xMax - 10, 0, 0),
        };
        BezierPath bezierAuto = new BezierPath(autoRoadPoints, false, PathSpace.xz);
        bezierAuto.AutoControlLength = 0.15f;
        VertexPath autoRoadPath = new VertexPath(bezierAuto, transform, 2.5f, 1);
        return autoRoadPath;
    }

    public static VertexPath[] GenerateRailsPoints(float craneZOne, float craneZTwo, float xMax, int count, Transform transform)
    {
        if (count == 1)
        {
            float middle = (craneZOne + craneZTwo) / 2f;
            Vector3[] railPoints_1 = new Vector3[]
            {
                new Vector3(5, 0, 0),
                new Vector3(5, 0, 10),
                new Vector3(6, 0, (middle - 12.5f) > 12.5f ? (middle - 12.5f) : 12.5f),
                new Vector3(12.5f, 0, middle - 2.5f),
                new Vector3(19, 0, middle),
                new Vector3(21.5f, 0, middle),

                new Vector3(xMax - 21.5f, 0, middle),
                new Vector3(xMax - 19, 0, middle),
                new Vector3(xMax - 12.5f, 0, middle - 2.5f),
                new Vector3(xMax - 6, 0,(middle - 12.5f) > 12.5f ? (middle - 12.5f) : 12.5f),
                new Vector3(xMax - 5, 0, 10),
                new Vector3(xMax - 5, 0, 0),
            };
            BezierPath bezierPathRail_1 = new BezierPath(railPoints_1, false, PathSpace.xz);
            bezierPathRail_1.AutoControlLength = 0.3f;
            VertexPath railRoadPath_1 = new VertexPath(bezierPathRail_1, transform, 2.5f, 1);
            return new VertexPath[] { railRoadPath_1 };
        }
        else if (count == 2)
        {
            Vector3[] railPoints_1 = new Vector3[]
            {
                new Vector3(5, 0, 0),
                new Vector3(5, 0, 10),
                new Vector3(6, 0, (craneZOne - 15f) > 15f ? (craneZOne - 15f) : 15f),
                new Vector3(12.5f, 0, craneZOne - 5f),
                new Vector3(19, 0, craneZOne - 2.5f),
                new Vector3(21.5f, 0, craneZOne - 2.5f),

                new Vector3(xMax - 21.5f, 0, craneZOne - 2.5f),
                new Vector3(xMax - 19, 0, craneZOne - 2.5f),
                new Vector3(xMax - 12.5f, 0, craneZOne - 5f),
                new Vector3(xMax - 6, 0,(craneZOne - 15f) > 15f ? (craneZOne - 15f) : 15f),
                new Vector3(xMax - 5, 0, 10),
                new Vector3(xMax - 5, 0, 0),
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
            return new VertexPath[] { railRoadPath_1, railRoadPath_2 };
        }
        return null;
    }

    public static Vector3[] GetFreeZonePoints(float zLevel, float xMax, int startOffset, float width)
    {
        Vector3[] freeZone = new Vector3[4]
        {
                    new Vector3(25, 0, zLevel - startOffset),
                    new Vector3(xMax - 25, 0, zLevel - startOffset),
                    new Vector3(xMax - 25, 0, zLevel - startOffset - width),
                    new Vector3(25, 0, zLevel - startOffset - width),
        };
        return freeZone;
    }
}
