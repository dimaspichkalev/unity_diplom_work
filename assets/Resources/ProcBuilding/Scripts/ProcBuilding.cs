using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class ProcBuilding : MonoBehaviour
{

    public int rows = 3;
    public int columns = 3;
    public int floors = 3;
    public Material[] house_materials;
    public Material roof_material;
    ProBuilderMesh body;
    ProBuilderMesh roof;
    ProBuilderMesh door;

    // Start is called before the first frame update
    //void Start()
    //{
    //    Clear();
    //   Generate(rows, columns, floors, transform.gameObject);
    //}

    public void Generate(int length, int width, int height, GameObject holder)
    {
        ColorStorage.RefreshWallColor();
        //material = RandomColoredMaterial.GenerateMaterial(ColorStorage.wallColor);
        int index = Random.Range(0, house_materials.Length);
        Material material = house_materials[index];
        //material = Resources.Load("Materials/House/House Material 1", typeof(Material)) as Material;

        body = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(length, height, width));
        body.GetComponent<MeshRenderer>().sharedMaterial = material;
        body.transform.name = "PartHouse BODY";
        body.transform.parent = holder.transform;
        body.transform.position = holder.transform.position + new Vector3(0, height / 2f, 0);
        body.transform.rotation = holder.transform.rotation;

        roof = ShapeGenerator.GeneratePrism(PivotLocation.Center, new Vector3(length, Mathf.CeilToInt(width / 5.0f), width));
        roof.SetMaterial(new Face[] { roof.faces[3], roof.faces[1] }, roof_material);
        roof.SetMaterial(new Face[] { roof.faces[0], roof.faces[2], roof.faces[4] }, material);
        roof.ToMesh();
        roof.Refresh();
        //roof.GetComponent<MeshRenderer>().sharedMaterial = material;// RandomColoredMaterial.GenerateMaterial(ColorStorage.wallColor);
        roof.transform.position = body.transform.position + new Vector3(0, 0.5f * (height + Mathf.CeilToInt(width / 5.0f)) , 0);
        roof.transform.rotation = body.transform.rotation;
        roof.transform.name = "PartHouse ROOF";
        roof.transform.parent = holder.transform;

        if (length >= 5)
        {
            //int countSideDoors = Mathf.CeilToInt(width / 25f);
            int extraSideDoors = (width > 25) ? 1 : 0;
            // z = width / countSideDoors
            Vector3[] doors_offsets = new Vector3[4 + extraSideDoors * 2];
            doors_offsets[0] = new Vector3(0, height / 6f, width / 2f);
            doors_offsets[1] = new Vector3(0, height / 6f, -width / 2f);
            if (extraSideDoors == 0)
            {
                doors_offsets[2] = new Vector3(length / 2f, height / 6f, 0);
                doors_offsets[3] = new Vector3(-length / 2f, height / 6f, 0);
            }
            else
            {
                doors_offsets[2] = new Vector3(length / 2f, height / 6f, -width / 4f);
                doors_offsets[3] = new Vector3(-length / 2f, height / 6f, -width / 4f);
                doors_offsets[4] = new Vector3(length / 2f, height / 6f, width / 4f);
                doors_offsets[5] = new Vector3(-length / 2f, height / 6f, width / 4f);
            }

            //for (int i = 2; i < doors_offsets.Length; i+=2)
            //{
            //    for (int j = 0; j < 2; j++)
            //    {
            //        float x = ((i + j) % 2 == 0) ? length / 2f : -length / 2f;
            //        float z = (j % 2 == 1) ? width / countSideDoors / 2 : -width / countSideDoors / 2;
            //        doors_offsets[i + j] = new Vector3(x, height / 6f, z);
            //    }
            //};
            int doorwidth = (int)(Random.Range(0.2f, 0.4f) * length);
            int doorSideWidth = (int)(Random.Range(0.2f, 0.4f) * width);
            foreach (Vector3 offset in doors_offsets)
            {
                if (offset.x != 0)
                {
                    door = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(doorSideWidth, height / 3, 0.14f));
                    door.transform.rotation *= Quaternion.Euler(0, 90, 0);
                }
                else
                    door = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(doorwidth, height / 3, 0.14f));
                door.GetComponent<MeshRenderer>().sharedMaterial = Resources.Load("Materials/HouseDoor", typeof(Material)) as Material;
                door.transform.name = "PartHouse DOOR";
                door.transform.parent = holder.transform;
                door.transform.position = holder.transform.position + offset;

            }
        }


    }

    public void Clear()
    {
        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name.StartsWith("PartHouse"));
        if (objects != null)
        {
            foreach (GameObject obj in objects)
            {
                DestroyImmediate(obj);
            }
        }
    }

    public void GenerateFiveRandomHouses()
    {
       // var holder = new GameObject("PartHouse Holder" + length.ToString() + " " + width.ToString() + " " + height.ToString());
        var holder = new GameObject("PartHouse Holder");
        holder.transform.position = new Vector3(Random.Range(0, 500), 0, Random.Range(0, 500));
        for (int i = 0; i < 5; i++)
        {
            Generate(Random.Range(5, 50), Random.Range(5, 50), Random.Range(5, 15), holder);
        }
    }

    public void PlaceObjectsInZone(Vector3[] zonePoints)
    {
        Vector3 minVector = Vector3.positiveInfinity;
        Vector3 maxVector = Vector3.zero;

        for (int i = 0; i < zonePoints.Length; i++)
        {
            minVector = (zonePoints[i].magnitude < minVector.magnitude) ? zonePoints[i] : minVector;
            maxVector = (zonePoints[i].magnitude > maxVector.magnitude) ? zonePoints[i] : maxVector;
        }
        Debug.Log("Max Vector " + maxVector.ToString());
        Debug.Log("Min Vector " + minVector.ToString());
        Vector3 prevHousePosition = minVector;
        int prevLenght = 0;
        int j = 0;
        while (true)
        {
            int houseLenght = Random.Range(10, ((maxVector.x - prevHousePosition.x - prevLenght / 2f - 5) > 50) ? 50 : (int)(maxVector.x - prevHousePosition.x - 7 - prevLenght / 2f));
            int houseWidth = Random.Range(10, (int)(maxVector.z - minVector.z));
            int houseHeight = Random.Range(5, 15);
            GameObject holder = new GameObject("PartHouse Holder" + houseLenght.ToString() + " " + houseWidth.ToString() + " " + houseHeight.ToString());
            if (j == 0)
            {
                j++;
                holder.transform.position = new Vector3(prevHousePosition.x + houseLenght / 2f + 2, 0, minVector.z + houseWidth / 2f + 2);
            }
            else
                holder.transform.position = new Vector3(prevHousePosition.x + houseLenght / 2f + prevLenght / 2f + 5, 0, minVector.z + houseWidth / 2f + 2);
            if ((maxVector.x - holder.transform.position.x) < 2 + houseLenght / 2f)
            {
                DestroyImmediate(holder);
                break;
            }
            Generate(houseLenght, houseWidth, houseHeight, holder);
            prevHousePosition = holder.transform.position;
            prevLenght = houseLenght;
        }
    }
}
