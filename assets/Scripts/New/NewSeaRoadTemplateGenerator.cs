using UnityEngine;
using System.Collections;
using PathCreation;

public class NewSeaRoadTemplateGenerator 
{
    public static VertexPath[] GenerateCranePoints(float zLevel, float xMax, bool deadEndLeft, bool deadEndRight, Transform transform)
    {
        Vector3[] craneRailPoints1 = new Vector3[2]
            {
                    new Vector3(deadEndLeft ? 5 : 25, 0, zLevel + 5),
                    new Vector3(deadEndRight ? xMax - 5 : xMax - 25, 0, zLevel + 5),
            };
        BezierPath bezierPathCrane1 = new BezierPath(craneRailPoints1, false, PathSpace.xz);
        bezierPathCrane1.AutoControlLength = 0.01f;
        VertexPath craneRailRoadPath1 = new VertexPath(bezierPathCrane1, transform, 45f, 1);

        Vector3[] craneRailPoints2 = new Vector3[2]
            {
                    new Vector3(deadEndLeft ? 5 : 25, 0, zLevel + 15),
                    new Vector3(deadEndRight ? xMax - 5 : xMax - 25, 0, zLevel + 15),
            };
        BezierPath bezierPathCrane2 = new BezierPath(craneRailPoints2, false, PathSpace.xz);
        bezierPathCrane2.AutoControlLength = 0.01f;
        VertexPath craneRailRoadPath2 = new VertexPath(bezierPathCrane2, transform, 45f, 1);

        return new VertexPath[2] { craneRailRoadPath1, craneRailRoadPath2 };
    }


    public static VertexPath[] GenerateRailsPoints(float craneZOne, float craneZTwo, float xMax, float zMiddleOfPart, int count, Transform transform)
    {
        Debug.Log("ZmiddleOfPart = " + zMiddleOfPart.ToString());
        if (count == 1)
        {
            float middle = (craneZOne + craneZTwo) / 2f;
            Vector3[] railPoints_1 = new Vector3[]
            {
                new Vector3(5, 0, middle),
                new Vector3(xMax, 0, middle),
                new Vector3(xMax + NewPlaneGenerator.seaRectangleXsize, 0, middle),
                new Vector3(xMax + NewPlaneGenerator.seaRectangleXsize + 100, 0, zMiddleOfPart),
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
                new Vector3(5, 0, craneZOne + 2.5f),
                new Vector3(xMax, 0, craneZOne + 2.5f),
                new Vector3(xMax + NewPlaneGenerator.seaRectangleXsize, 0, craneZOne + 2.5f),
                new Vector3(xMax + NewPlaneGenerator.seaRectangleXsize + 100, 0, zMiddleOfPart),
            };
            BezierPath bezierPathRail_1 = new BezierPath(railPoints_1, false, PathSpace.xz);
            bezierPathRail_1.AutoControlLength = 0.3f;
            VertexPath railRoadPath_1 = new VertexPath(bezierPathRail_1, transform, 2.5f, 1);

            Vector3[] railPoints_2 = new Vector3[]
            {
                railPoints_1[0] + new Vector3(0, 0, 5f),
                railPoints_1[1] + new Vector3(0, 0, 5f),
                railPoints_1[2] + new Vector3(0, 0, 5f),
                railPoints_1[3] + new Vector3(0, 0, 5f),
            };
            BezierPath bezierPathRail_2 = new BezierPath(railPoints_2, false, PathSpace.xz);
            bezierPathRail_2.AutoControlLength = 0.3f;
            VertexPath railRoadPath_2 = new VertexPath(bezierPathRail_2, transform, 2.5f, 1);
            return new VertexPath[] { railRoadPath_1, railRoadPath_2 };
        }
        return null;
    }

    public static VertexPath GenerateAutoPoints(float zLevel, float xMax, float zMax, Transform transform)
    {
        Vector3[] autoRoadPoints = new Vector3[5]
        {
            new Vector3(5, 0, zLevel + 5),
            new Vector3(xMax, 0, zLevel + 5),
            new Vector3(xMax + NewPlaneGenerator.seaRectangleXsize / 2f, 0, zLevel + 5),
            new Vector3(xMax + NewPlaneGenerator.seaRectangleXsize / 2f, 0, zMax / 2f),
            new Vector3(xMax + NewPlaneGenerator.seaRectangleXsize, 0, zMax / 2f),
        };
        BezierPath bezierAuto = new BezierPath(autoRoadPoints, false, PathSpace.xz);
        bezierAuto.AutoControlLength = 0.15f;
        VertexPath autoRoadPath = new VertexPath(bezierAuto, transform, 2.5f, 1);
        return autoRoadPath;
    }

    public static Vector3[] GetFreeZonePoints(float zLevel, float xMax, int startOffset, float width)
    {
        Vector3[] freeZone = new Vector3[4]
        {
                    new Vector3(5, 0, zLevel + startOffset),
                    new Vector3(xMax - 5, 0, zLevel + startOffset),
                    new Vector3(xMax - 5, 0, zLevel + startOffset + width),
                    new Vector3(5, 0, zLevel + startOffset + width),
        };
        return freeZone;
    }

}
