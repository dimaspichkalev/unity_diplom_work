using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class ProGen : MonoBehaviour
{
    [Header("Prefab options")]
    [SerializeField]
    private GameObject wallPrefab;

    [SerializeField]
    private GameObject[] roofPrefabs;

    int roofIndex;

    [SerializeField]
    private bool randomizeRoofSelection = false;

    [SerializeField]
    private GameObject[] windowPrefabs;

    [SerializeField]
    private bool randomizeWindowSelection = false;

    [SerializeField]
    private GameObject doorPrefab;

    [SerializeField]
    public bool includeRoof = false;

    [SerializeField]
    private bool keepInsideWalls = false;

    [SerializeField]
    [Range(0,1)]
    private float windowPercentChance = 0.3f;

    [SerializeField]
    [Range(0, 1)]
    private float doorPercentChance = 0.2f;

    [Header("Grid Options")]
    [SerializeField]
    [Range(1, 50)]
    public int rows = 3;

    [SerializeField]
    [Range(1, 50)]
    public int columns = 3;

    [SerializeField]
    public bool randomizeRows = false;

    [SerializeField]
    public bool randomizeColumns = false;

    [SerializeField]
    [Range(0, 20.0f)]
    public float cellUnitSize = 1;

    [SerializeField]
    [Range(0, 20)]
    public int numberOfFloors = 1;

    private Floor[] floors;

    private void Awake() => Generate();

    private List<GameObject> rooms = new List<GameObject>();

    private int prefabCounter = 0;

    ProBuilderMesh m_Mesh;

    public static Material material;

    public void Generate()
    {
        ColorStorage.RefreshWallColor();
        material = RandomColoredMaterial.GenerateMaterial(ColorStorage.wallColor);
        roofIndex = Random.Range(1, roofPrefabs.Length);
        prefabCounter = 0;

        Clear();
        BuildDataStructure();
        Render();

        if (!keepInsideWalls)
        {
            RemoveInsideWalls();
        }

    }

    void BuildDataStructure()
    {

        int floorCount = 0;

        int initialRows = rows;
        int initialColumns = columns;

        floors = new Floor[numberOfFloors];

        foreach (Floor floor in floors)
        {
            Room[,] rooms = new Room[initialRows, initialColumns];

            for (int row = 0; row < initialRows; row++)
            {
                for (int column = 0; column < initialColumns; column++)
                {
                    var roomPosition = new Vector3(row * cellUnitSize, floorCount, column * cellUnitSize);
                    rooms[row, column] = new Room(roomPosition, includeRoof ? ((floorCount == numberOfFloors - 1) && (column == 0 || column == initialColumns - 1)) : false);

                    rooms[row, column].Walls[0] = new Wall(roomPosition, Quaternion.Euler(0, 0, 0));
                    rooms[row, column].Walls[1] = new Wall(roomPosition, Quaternion.Euler(0, 90, 0));
                    rooms[row, column].Walls[2] = new Wall(roomPosition, Quaternion.Euler(0, 180, 0));
                    rooms[row, column].Walls[3] = new Wall(roomPosition, Quaternion.Euler(0, -90, 0));

                    if (randomizeRows || randomizeColumns)
                        rooms[row, column].HasRoof = true;
                }
            }
            floors[floorCount] = new Floor(floorCount++, rooms);

            if (randomizeRows)
                initialRows = Random.Range(1, rows);
            if (randomizeColumns)
                initialRows = Random.Range(1, columns);

            if (floorCount == numberOfFloors - 1)
            {
                Room room = rooms[initialRows - 1, 0];
                m_Mesh = ShapeGenerator.GeneratePrism(PivotLocation.Center, new Vector3(rows, Mathf.CeilToInt(columns / 5.0f), columns));
                m_Mesh.GetComponent<MeshRenderer>().sharedMaterial = material;// RandomColoredMaterial.GenerateMaterial(ColorStorage.wallColor);
                m_Mesh.transform.name = "House ROOF";
                m_Mesh.transform.position = transform.position + new Vector3(room.Walls[0].Position.x - 0.5f * (initialRows - 1), floorCount + 0.5f * Mathf.CeilToInt(columns / 5.0f), room.Walls[0].Position.z + 0.5f * (initialColumns - 1));
                m_Mesh.transform.parent = transform;
            }

        }
    }    
    

    void Render()
    {
        int doorwidth = (int)(0.4f * columns);
        int door_delta = doorwidth / 2;
        foreach (Floor floor in floors)
        {
            for (int row = 0; row < floor.rooms.GetLength(0); row++)
            {
                for (int column = 0; column < floor.rooms.GetLength(1); column++)
                {
                    Room room = floor.rooms[row, column];
                    room.FloorNumber = floor.FloorNumber;
                    GameObject roomGo = new GameObject($"Room_{row}_{column}");
                    rooms.Add(roomGo);
                    roomGo.transform.parent = transform;
                    if (floor.FloorNumber < 3 && numberOfFloors > 5 && column <= columns / 2 + door_delta && column >= columns / 2 - door_delta)
                    {
                        RoomPlacement(doorPrefab, room, roomGo);
                    }
                    else
                    {
                        if (Random.Range(0.0f, 1.0f) <= windowPercentChance && room.FloorNumber < numberOfFloors - 1 && room.FloorNumber > 0)
                        {
                            if (randomizeWindowSelection)
                            {
                                int windowIndex = Random.Range(0, windowPrefabs.Length);
                                RoomPlacement(windowPrefabs[windowIndex], room, roomGo);
                            }
                            else
                                RoomPlacement(windowPrefabs[0], room, roomGo);
                        }
                        else
                            RoomPlacement(wallPrefab, room, roomGo);
                    }
                }
            }

        }
    }

    private void RoomPlacement(GameObject prefab, Room room, GameObject roomGo)
    {
        SpawnPrefab(prefab, roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation);
        SpawnPrefab(prefab, roomGo.transform, room.Walls[1].Position, room.Walls[1].Rotation);
        SpawnPrefab(prefab, roomGo.transform, room.Walls[2].Position, room.Walls[2].Rotation);
        SpawnPrefab(prefab, roomGo.transform, room.Walls[3].Position, room.Walls[3].Rotation);

        //if (room.HasRoof)
        //{
        //    if (randomizeRoofSelection && room.FloorNumber >= numberOfFloors - 1)
        //    {
        //            SpawnPrefab(roofPrefabs[0], roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation);
        //    }
        //    else
        //        SpawnPrefab(roofPrefabs[0], roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation);
        //}
    }

    private void SpawnPrefab(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
    {
        var gameObject = Instantiate(prefab, transform.position + position, rotation);
        gameObject.transform.parent = parent;
        gameObject.AddComponent<WallComponent>();
        gameObject.name = $"{gameObject.name}_{prefabCounter}";
        prefabCounter++;
    }

    void RemoveInsideWalls()
    {
        var wallComponents = GameObject.FindObjectsOfType<WallComponent>();
        var childs = wallComponents.Select(c => c.transform.GetChild(0).position.ToString()).ToList();

        var dupPositions = childs.GroupBy(c => c)
            .Where(c => c.Count() > 1)
            .Select(grp => grp.Key)
            .ToList();

        foreach (WallComponent w in wallComponents)
        {
            var childTransform = w.transform.GetChild(0);
            if (dupPositions.Contains(childTransform.position.ToString()))
            {
                DestroyImmediate(childTransform.gameObject);
            }
        }

    }

    void Clear()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            DestroyImmediate(rooms[i]); 
        }
        rooms.Clear();

        if (m_Mesh != null)
        {
           DestroyImmediate(m_Mesh.transform.gameObject);
        }
    }
}
