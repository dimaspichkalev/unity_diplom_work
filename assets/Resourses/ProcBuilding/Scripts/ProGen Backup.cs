using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ProGenBackup : MonoBehaviour
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
    [Range(1, 20)]
    public int rows = 3;

    [SerializeField]
    [Range(1, 20)]
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

    public void Generate()
    {
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
        floors = new Floor[numberOfFloors];

        int floorCount = 0;

        int initialRows = rows;
        int initialColumns = columns;

        foreach (Floor floor in floors)
        {
            Room[,] rooms = new Room[initialRows, initialColumns];

            for (int row = 0; row < initialRows; row++)
            {
                for (int column = 0; column < initialColumns; column++)
                {
                    var roomPosition = new Vector3(row * cellUnitSize, floorCount, column * cellUnitSize);
                    rooms[row, column] = new Room(roomPosition, includeRoof ? (floorCount == floors.Length - 1) : false);

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
        }
    }

    void Render()
    {
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
                    int roof_flag = 0;
                    if (column == 0)
                        roof_flag = 1;
                    else if (column == floor.rooms.GetLength(1) - 1)
                        roof_flag = 2; 
                    if (floor.FloorNumber == 0)
                    {
                        RoomPlacement(Random.Range(0.0f, 1.0f) <= doorPercentChance ? doorPrefab : wallPrefab, room, roomGo);
                    }
                    else
                    {
                        if (Random.Range(0.0f, 1.0f) <= windowPercentChance)
                        {
                            if (randomizeWindowSelection)
                            {
                                int windowIndex = Random.Range(0, windowPrefabs.Length);
                                RoomPlacement(windowPrefabs[windowIndex], room, roomGo, roof_flag);
                            }
                            else
                                RoomPlacement(windowPrefabs[0], room, roomGo, roof_flag);
                        }
                        else
                            RoomPlacement(wallPrefab, room, roomGo, roof_flag);
                    }
                }
            }
        }
    }

    private void RoomPlacement(GameObject prefab, Room room, GameObject roomGo, int roof_flag = 0)
    {
        SpawnPrefab(prefab, roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation);
        SpawnPrefab(prefab, roomGo.transform, room.Walls[1].Position, room.Walls[1].Rotation);
        SpawnPrefab(prefab, roomGo.transform, room.Walls[2].Position, room.Walls[2].Rotation);
        SpawnPrefab(prefab, roomGo.transform, room.Walls[3].Position, room.Walls[3].Rotation);

        if (room.HasRoof)
        {
            if (randomizeRoofSelection && room.FloorNumber == floors.Count() - 1)
            { 
                if (roof_flag == 1)
                    SpawnPrefab(roofPrefabs[roofIndex], roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation);
                else if (roof_flag == 2)
                    SpawnPrefab(roofPrefabs[roofIndex], roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation * Quaternion.Euler(0, 180, 0));
                else
                    SpawnPrefab(roofPrefabs[0], roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation);
            }
            else 
                SpawnPrefab(roofPrefabs[0], roomGo.transform, room.Walls[0].Position, room.Walls[0].Rotation);
        }
    }

    private void RoofPlacement()
    {
        Floor lastFloor = floors[floors.Length - 1];
        for (int row = 0; row < lastFloor.rooms.GetLength(0); row++)
        {
            for (int column = 0; column < lastFloor.rooms.GetLength(1); column++)
            {
                Room room = lastFloor.rooms[row, column];
                GameObject roomGo = new GameObject($"Room_{row}_{column}");
                rooms.Add(roomGo);
                roomGo.transform.parent = transform;
                int roof_flag = 0;
                if (column == 0)
                    roof_flag = 1;
                else if (column == lastFloor.rooms.GetLength(1) - 1)
                    roof_flag = 2;
            }
        }
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
    }
}
