using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnvPlane : MonoBehaviour
{
    public MarkupTemplate markupTemplate;
    public Material sand;
    public Material grass;

    public void CreateEnvironmentPlane(float xStart, float yValue, float zStart, float zwidth, NewPlaneGenerator.PortType portType)
    {
        float length = GetLengthOfTemplate(markupTemplate);
        if (portType == NewPlaneGenerator.PortType.Морской)
        {
            Vector3[] points = new Vector3[]
            {
                    new Vector3(xStart, 0, zStart),
                    new Vector3(20 + xStart, yValue, zStart),
                    new Vector3(length + xStart, yValue, zStart),
                    new Vector3(length + xStart, 0, zStart)
            };
            ProBuilder.Examples.CreatePolyShape.CreateEnvPlane(points, zwidth, sand, grass, transform);
        }
        else if (portType == NewPlaneGenerator.PortType.Речной)
        {
            Vector3[] points = new Vector3[]
            {
                new Vector3(xStart, 0, zStart),
                new Vector3(xStart, yValue, zStart - 20),
                new Vector3(xStart, yValue, zStart  - length),
                new Vector3(xStart, 0, zStart - length)
            };
            ProBuilder.Examples.CreatePolyShape.CreateEnvPlane(points, zwidth, sand, grass, transform);
        }  
        
    }

    public void PlaceObjects(float xStart, float yValue, float zStart, float width, NewPlaneGenerator.PortType portType)
    {
        foreach (MarkupElement element in markupTemplate.roadsList)
        {
            if (element is ShorelineMarkupElement)
            {
                ShorelineMarkupElement elem = (ShorelineMarkupElement)element;
                if (portType == NewPlaneGenerator.PortType.Морской)
                {
                    PlaceObjectsInZone(elem.numItemsToSpawn, xStart, xStart + elem.length, yValue, zStart, zStart + width, elem.objectsToPlacement);
                    xStart = xStart + elem.length;
                }
                else
                {
                    PlaceObjectsInZone(elem.numItemsToSpawn, xStart, xStart + width, yValue, zStart, zStart - elem.length, elem.objectsToPlacement);
                    zStart = zStart - elem.length;
                }  
            }
        }
    }

    void PlaceObjectsInZone(int numItemsToSpawn, float xStart, float xEnd, float yValue, float zStart, float zEnd, List<GameObject> prefabs)
    {
        GameObject holder = new GameObject("Objects Holder");
        holder.transform.parent = transform;
        for (int i = 0; i < numItemsToSpawn; i++)
        {
            Vector3 randPosition = new Vector3(Random.Range(xStart, xEnd), yValue, Random.Range(zStart, zEnd));
            GameObject clone = Instantiate(prefabs[Random.Range(0, prefabs.Count-1)], randPosition, Quaternion.identity);
            clone.transform.parent = holder.transform;
        }
    }

    float GetLengthOfTemplate(MarkupTemplate markupTemplate)
    {
        float length = 20;
        foreach (MarkupElement element in markupTemplate.roadsList)
        {
            if (element is ShorelineMarkupElement)
            {
                ShorelineMarkupElement elem = (ShorelineMarkupElement)element;
                length += elem.length;
            }
        }
        return length;
    }
}
