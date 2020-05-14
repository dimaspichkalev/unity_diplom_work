using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        int floorCount = 0;

        int initialRows = rows;
        int initialColumns = columns;

        int additionalFloors = columns / 2;
        floors = new Floor[numberOfFloors + additionalFloors];

        foreach (Floor floor in floors)
        {
            if (floorCount < numberOfFloors)
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
            }
            else
            {
                Room[,] rooms = new Room[initialRows, initialColumns - ((floorCount - numberOfFloors + 1) * 2)];
                for (int row = 0; row < rooms.GetLength(0); row++)
                {
                    for (int column = 1 + (floorCount - numberOfFloors); column < rooms.GetLength(1) + (floorCount - numberOfFloors + 1); column++)
                    {
                        var roomPosition = new Vector3(row * cellUnitSize, floorCount, column * cellUnitSize);
                        int col_index = column - (floorCount - numberOfFloors) - 1;
                        rooms[row, col_index] = new Room(roomPosition, includeRoof ? (initialColumns % 2 == 0 || floorCount != floors.Length - 1 || floors.Length == 3) : false);

                        rooms[row, col_index].Walls[0] = new Wall(roomPosition, Quaternion.Euler(0, 0, 0));
                        rooms[row, col_index].Walls[1] = new Wall(roomPosition, Quaternion.Euler(0, 90, 0));
                        rooms[row, col_index].Walls[2] = new Wall(roomPosition, Quaternion.Euler(0, 180, 0));
                        rooms[row, col_index].Walls[3] = new Wall(roomPosition, Quaternion.Euler(0, -90, 0));
                        
                    }
                }
                floors[floorCount] = new Floor(floorCount++, rooms);
            }
        }
    }

    void Render()
    {
        foreach (Floor floor in floors)
        {
            if (floor.FloorNumber < numberOfFloors)
            {
                for (int row = 0; row < floor.rooms.GetLength(0); row++)
                {
                    for (int column = 0; column < floor.rooms.GetLength(1); column++)
                    {
                        Room room = floor.rooms[row, column];
                        room.FloorNumber = floor.FloorNumber;
                        int roof_flag = 0;
                        if (column == 0)
                            roof_flag = 1;
                        else if (column == floor.rooms.GetLength(1) - 1)
                            roof_flag = 2;
                        GameObject roomGo = new GameObject($"Room_{row}_{column}");
                        rooms.Add(roomGo);
                        roomGo.transform.parent = transform;
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
                                    RoomPlacement(windowPrefabs[windowIndex], room, roomGo);
                                }
                                else
                                    RoomPlacement(windowPrefabs[0], room, roomGo);
                            }
                            else
                                RoomPlacement(wallPrefab, room, roomGo, roof_flag);
                        }
                    }
                }
            }
            else
            {
                for (int row = 0; row < floor.rooms.GetLength(0); row++)
                {
                    for (int column = 1 + (floor.FloorNumber - numberOfFloors); column < floor.rooms.GetLength(1) + (floor.FloorNumber - numberOfFloors + 1); column++)
                    {
                        int col_index = column - (floor.FloorNumber - numberOfFloors) - 1;
                        int roof_flag = 0;
                        if (column == 1 + (floor.FloorNumber - numberOfFloors))
                            roof_flag = 1;
                        else if (column == floor.rooms.GetLength(1) + (floor.FloorNumber - numberOfFloors))
                            roof_flag = 2;
                        Room room = floor.rooms[row, col_index];
                        room.FloorNumber = floor.FloorNumber;
                        GameObject roomGo = new GameObject($"Room_{row}_{col_index}");
                        rooms.Add(roomGo);
                        roomGo.transform.parent = transform;
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
            if (randomizeRoofSelection && room.FloorNumber >= numberOfFloors - 1)
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
        for (int i = floors.Length - numberOfFloors; i < floors.Length - 1; i++)
        {

        }
        Floor lastFloor = floors[floors.Length - 1];
        int half = lastFloor.rooms.GetLength(1) / 2;

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

                if (row < half - 1)
                {
                    for (int i = 0; i < half - 1; i++)
                    {
                        RoomPlacement(wallPrefab, room, roomGo, roof_flag);
                    }
                   
                }
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
