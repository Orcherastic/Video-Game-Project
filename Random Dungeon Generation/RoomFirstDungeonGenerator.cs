using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomFirstDungeonGenerator : SimpleRandomDungeonWalkGenerator
{
    private static RoomFirstDungeonGenerator instance;

    public static RoomFirstDungeonGenerator MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RoomFirstDungeonGenerator>();
            }
            return instance;
        }
    }

    [Serializable]
    private class propLootInfo
    {
        public GameObject propPrefab;
        public int dropChance;
    }

    [Serializable]
    private class propInfo
    {
        [Min(1)]
        public int MinQuantity = 1;
        [Min(1)]
        public int MaxQuantity = 1;
        public BreakableObject propPrefab;
        public List<propLootInfo> lootTable = new List<propLootInfo>();
    }
    [Serializable]
    private class chestLootInfo
    {
        public Item propPrefab;
        public int dropChance;
    }

    [Serializable]
    private class chestInfo
    {
        public int MinQuantity = 0;
        public int MaxQuantity = 0;
        public SmallChest propPrefab;
        public List<chestLootInfo> lootTable = new List<chestLootInfo>();
    }

    [Serializable]
    private class enemyInfo
    {
        [Min(1)]
        public int MinQuantity = 1;
        [Min(1)]
        public int MaxQuantity = 1;
        public Enemy propPrefab;
    }

    private Animator anim;

    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;


    // PCG Data
    //private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
    private List<Vector2Int> floorPositions = new List<Vector2Int>();
    private List<Vector2Int> usedPositions = new List<Vector2Int>();
    private List<Enemy> enemies = new List<Enemy>();

    public string goToScene;

    [SerializeField] private WallTorch torchPrefab;
    [SerializeField] private GameObject dungeonGateKeyPrefab;
    [SerializeField] private List<enemyInfo> enemiesToInstantiate = new List<enemyInfo>();
    [SerializeField] private List<propInfo> propsToInstantiate = new List<propInfo>();
    [SerializeField] private List<chestInfo> chestsToInstantiate = new List<chestInfo>();
    [Header("Enterace Gate Prefabs")]
    [SerializeField] private GameObject gateDownClosed;
    [SerializeField] private GameObject gateLeftClosed;
    [SerializeField] private GameObject gateRightClosed;
    [SerializeField] private GameObject gateUpClosed;
    [SerializeField] private GameObject gateDownOpened;
    [SerializeField] private GameObject gateLeftOpened;
    [SerializeField] private GameObject gateRightOpened;
    [SerializeField] private GameObject gateUpOpened;
    [Header("Exit Gate Prefabs")]
    [SerializeField] private GameObject exitGateDownClosed;
    [SerializeField] private GameObject exitGateLeftClosed;
    [SerializeField] private GameObject exitGateRightClosed;
    [SerializeField] private GameObject exitGateUpClosed;
    [SerializeField] private GameObject exitGateDownOpened;
    [SerializeField] private GameObject exitGateLeftOpened;
    [SerializeField] private GameObject exitGateRightOpened;
    [SerializeField] private GameObject exitGateUpOpened;

    public GameObject entranceOpened;
    public GameObject entranceClosed;
    public GameObject exitOpened;
    public GameObject exitClosed;

    private int startGateIndexSaver = -1;
    private string gateSide;
    private Vector2Int entrancePosition = new Vector2Int();

    bool introCutscene = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        GenerateDungeon();
    }

    private void Update()
    {
        if (introCutscene)
        {
            introCutscene = false;
            StartCoroutine(InrtoCutsceneCo(gateSide));
        }
    }

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition,
            new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if(randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomList);
        }
        else
        {
            floor = CreateSimpleRooms(roomList);
        }

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        List<Vector2Int> roomCenters2 = new List<Vector2Int>();
        foreach (var room in roomList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
            roomCenters2.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        GenerateStartAndEndGates(roomCenters2, true);
        roomCenters2.RemoveAt(startGateIndexSaver);
        GenerateStartAndEndGates(roomCenters2, false);

        InstantiateTorches(tilemapVisualizer.wallPositions);
        InstantiateProps(floorPositions);
        InstantiateChests(floorPositions);
        InstantiateEnemies(floorPositions);

        GiveKeyToEnemy();
    }

    private void InstantiateTorches(List<Vector2Int> wallPositions)
    {
        for (int i = 0; i < wallPositions.Count; i++)
        {
            Vector2Int position = wallPositions[i];

            int spawnChance = UnityEngine.Random.Range(0, 101);
            if (spawnChance <= 30)
            {
                position.x += 1;
                position.y += 4;

                Instantiate(torchPrefab, (Vector3Int)position, Quaternion.identity);
                i += 10;
            }
        }
    }

    private void InstantiateChests(List<Vector2Int> floorPositions)
    {
        foreach (chestInfo prop in chestsToInstantiate)
        {
            int numOf = UnityEngine.Random.Range(prop.MinQuantity, prop.MaxQuantity + 1);
            for (int i = 0; i < numOf; i++)
            {
                Vector2Int position = floorPositions[UnityEngine.Random.Range(0, floorPositions.Count)];

                int randomNumber = UnityEngine.Random.Range(0, 101);
                List<Item> possibleItems = new List<Item>();
                foreach(chestLootInfo chestItem in prop.lootTable)
                {
                    if (chestItem.dropChance >= randomNumber)
                        possibleItems.Add(chestItem.propPrefab);
                }
                if(possibleItems.Count > 0)
                {
                    prop.propPrefab.pickUpPrefab = possibleItems[UnityEngine.Random.Range(0, possibleItems.Count)];
                }
                else
                    prop.propPrefab.pickUpPrefab = prop.lootTable[0].propPrefab;

                if (!usedPositions.Contains(position))
                {
                    if(!usedPositions.Contains(position + new Vector2Int(1,0)) && !usedPositions.Contains(position + new Vector2Int(1, -1))
                         && !usedPositions.Contains(position + new Vector2Int(0, -1)) && !usedPositions.Contains(position + new Vector2Int(-1, -1))
                          && !usedPositions.Contains(position + new Vector2Int(-1, 0)) && !usedPositions.Contains(position + new Vector2Int(-1, 1))
                           && !usedPositions.Contains(position + new Vector2Int(0, 1)) && !usedPositions.Contains(position + new Vector2Int(1, 1)))
                    {
                        Instantiate(prop.propPrefab, (Vector3Int)position, Quaternion.identity);
                        usedPositions.Add(position);
                    }
                }
                else
                    i--;
            }
        }
    }

    private void InstantiateEnemies(List<Vector2Int> floorPositions)
    {
        foreach (enemyInfo prop in enemiesToInstantiate)
        {
            int numOf = UnityEngine.Random.Range(prop.MinQuantity, prop.MaxQuantity + 1);
            for (int i = 0; i < numOf; i++)
            {
                Vector2Int position = floorPositions[UnityEngine.Random.Range(0, floorPositions.Count)];
                if (!usedPositions.Contains(position))
                {
                    Enemy enemy = Instantiate(prop.propPrefab, (Vector3Int)position, Quaternion.identity);
                    usedPositions.Add(position);
                    enemies.Add(enemy);

/*                    if (!keyAdded)
                    {
                        int randomNum = UnityEngine.Random.Range(0, 101);
                        if (randomNum <= keyChance)
                        {
                            enemy.pickUpPrefab.Add(dungeonGateKeyPrefab);
                            keyAdded = true;
                        }
                        else
                        {
                            keyChance += 2;
                        }
                    }*/
                }
                else
                    i--;
            }
        }
    }

    private void GiveKeyToEnemy()
    {
        int randIndex = UnityEngine.Random.Range(0, enemies.Count);
        enemies[randIndex].dungeonKeyPrefab = dungeonGateKeyPrefab;
    }

    private void InstantiateProps(List<Vector2Int> floorPositions)
    {
        foreach(propInfo prop in propsToInstantiate)
        {
            int numOf = UnityEngine.Random.Range(prop.MinQuantity, prop.MaxQuantity + 1);
            for(int i=0; i<numOf; i++)
            {
                Vector2Int position = floorPositions[UnityEngine.Random.Range(0, floorPositions.Count)];

                int randomNumber = UnityEngine.Random.Range(0, 101);
                List<GameObject> possibleItems = new List<GameObject>();
                foreach (propLootInfo propItem in prop.lootTable)
                {
                    if (propItem.dropChance >= randomNumber)
                        possibleItems.Add(propItem.propPrefab);
                }
                if (possibleItems.Count > 0)
                {
                    prop.propPrefab.pickUpPrefab = possibleItems[UnityEngine.Random.Range(0, possibleItems.Count)];
                }
                else
                    prop.propPrefab.pickUpPrefab = null;

                if (!usedPositions.Contains(position))
                {
                    Instantiate(prop.propPrefab, (Vector3Int)position, Quaternion.identity);
                    usedPositions.Add(position);
                }
                else
                    i--;
            }
        }
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for(int i=0; i<roomList.Count; i++)
        {
            var roomBounds = roomList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(roomCenter);
            foreach(var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) &&
                    position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }

        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while(roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }

        return corridors;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float length = float.MaxValue;
        foreach(var position in roomCenters)
        {
            float currentLength = Vector2.Distance(position, currentRoomCenter);
            if (currentLength < length)
            {
                length = currentLength;
                closest = position;
            }
        }

        return closest;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int closest)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        while(position.y != closest.y)
        {
            if(closest.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if(closest.y < position.y)
            {
                position += Vector2Int.down;
            }
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    corridor.Add(position + new Vector2Int(x, y));
                    floorPositions.Add(position + new Vector2Int(x+1, y+1));
                }
            }
        }
        while (position.x != closest.x)
        {
            if (closest.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (closest.x < position.x)
            {
                position += Vector2Int.left;
            }
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    corridor.Add(position + new Vector2Int(x, y));
                    floorPositions.Add(position + new Vector2Int(x+1, y+1));
                }
            }
        }
        return corridor;
    }
    
    /*
    public void GenerateGate(Vector2Int direction, Vector2Int roomCenter, int corridorLength)
    {
        if(direction.x != roomCenter.x)
        {
            Vector2Int position = new Vector2Int();
            position.x = direction.x;
            position.y = roomCenter.y + 1;
            if (direction.x < roomCenter.x)
            {
                Instantiate(gateLeftClosed, (Vector3Int)position, Quaternion.identity);
                Instantiate(gateLeftOpened, (Vector3Int)position, Quaternion.identity);
                gateLeftOpened.SetActive(false);
                gateLeftClosed.SetActive(true);
            }
            else
            {
                Instantiate(gateRightClosed, (Vector3Int)position, Quaternion.identity);
                Instantiate(gateRightOpened, (Vector3Int)position, Quaternion.identity);
                gateRightOpened.SetActive(false);
                gateRightClosed.SetActive(true);
            }
        }
        else if(direction.y != roomCenter.y)
        {
            Vector2Int position = new Vector2Int();
            position.x = roomCenter.x+1;
            position.y = direction.y;
            if (direction.y < roomCenter.y)
            {
                Instantiate(gateDownClosed, (Vector3Int)position, Quaternion.identity);
                Instantiate(gateDownOpened, (Vector3Int)position, Quaternion.identity);
                gateDownOpened.SetActive(false);
                gateDownClosed.SetActive(true);
            }
            else
            {
                Instantiate(gateUpClosed, (Vector3Int)position, Quaternion.identity);
                Instantiate(gateUpOpened, (Vector3Int)position, Quaternion.identity);
                gateUpOpened.SetActive(false);
                gateUpClosed.SetActive(true);
            }
        }
    }


    private Vector2Int StartCoridorDirection(Vector2Int roomCenter)
    {
        Vector2Int direction = new Vector2Int();
        int rand = UnityEngine.Random.Range(0, 101);
        int corridorLength = 15;
        if(rand <= 25)
        {
            direction.x = -roomCenter.x - corridorLength;
            direction.y = roomCenter.y;
        }
        else if (rand > 25 && rand <= 50)
        {
            direction.y = -roomCenter.y - corridorLength;
            direction.x = roomCenter.x;
        }
        else if (rand > 50 && rand <= 75)
        {
            direction.x = roomCenter.x + corridorLength;
            direction.y = roomCenter.y;
        }
        else if (rand > 75 && rand <= 100)
        {
            direction.y = roomCenter.y + corridorLength;
            direction.x = roomCenter.x;
        }

        GenerateGate(direction, roomCenter, corridorLength);

        return direction;
    }
    */
    
    private void GenerateStartAndEndGates(List<Vector2Int> roomCenters, bool entrance)
    {
        int startRoomIndex = UnityEngine.Random.Range(0, roomCenters.Count);
        startGateIndexSaver = startRoomIndex;

        Vector2Int gatePosition = new Vector2Int();
        gatePosition = CalculateShortestPathToWall(roomCenters[startRoomIndex]);

        if(entrance)
        {
            entrancePosition = gatePosition;
            if (gatePosition.x > roomCenters[startRoomIndex].x)
            {
                entranceClosed = Instantiate(gateRightClosed, (Vector3Int)gatePosition, Quaternion.identity);
                entranceOpened = Instantiate(gateRightOpened, (Vector3Int)(gatePosition - new Vector2Int(-1, 2)), Quaternion.identity);
                gateSide = "right";
                for (int i = -4; i < 4; i++)
                    tilemapVisualizer.RemoveSingleBasicWall(gatePosition + new Vector2Int(-1, i), "side");
            }
            else if (gatePosition.x < roomCenters[startRoomIndex].x)
            {
                entranceClosed = Instantiate(gateLeftClosed, (Vector3Int)(gatePosition - new Vector2Int(1, 0)), Quaternion.identity);
                entranceOpened = Instantiate(gateLeftOpened, (Vector3Int)(gatePosition - new Vector2Int(2, 2)), Quaternion.identity);
                gateSide = "left";
                for (int i = -4; i < 4; i++)
                    tilemapVisualizer.RemoveSingleBasicWall(gatePosition + new Vector2Int(-1, i), "side");
            }
            else if (gatePosition.y > roomCenters[startRoomIndex].y)
            {
                entranceClosed = Instantiate(gateUpClosed, (Vector3Int)(gatePosition + new Vector2Int(0, 2)), Quaternion.identity);
                entranceOpened = Instantiate(gateUpOpened, (Vector3Int)(gatePosition + new Vector2Int(-2, 2)), Quaternion.identity);
                gateSide = "up";
                for (int i = -2; i < 2; i++)
                {
                    tilemapVisualizer.RemoveSingleBasicWall(gatePosition + new Vector2Int(i, -1), "front");
                    tilemapVisualizer.wallPositions.Remove(gatePosition + new Vector2Int(i, -1));
                }

            }
            else if (gatePosition.y < roomCenters[startRoomIndex].y)
            {
                entranceClosed = Instantiate(gateDownClosed, (Vector3Int)(gatePosition - new Vector2Int(0, 2)), Quaternion.identity);
                entranceOpened = Instantiate(gateDownOpened, (Vector3Int)(gatePosition - new Vector2Int(1, 2)), Quaternion.identity);
                gateSide = "down";
                for (int i = -2; i < 2; i++)
                    tilemapVisualizer.RemoveSingleBasicWall(gatePosition + new Vector2Int(i, -1), "back");
            }

            RemoveFloorPositionsNearGate(gatePosition, gateSide);
        }
        else
        {
            if (gatePosition.x > roomCenters[startRoomIndex].x)
            {
                exitClosed = Instantiate(exitGateRightClosed, (Vector3Int)gatePosition, Quaternion.identity);
                exitOpened = Instantiate(exitGateRightOpened, (Vector3Int)(gatePosition - new Vector2Int(-1, 2)), Quaternion.identity);
                exitOpened.SetActive(false);
                exitClosed.SetActive(true);
                for (int i = -4; i < 4; i++)
                    tilemapVisualizer.RemoveSingleBasicWall(gatePosition + new Vector2Int(-1, i), "side");
            }
            else if (gatePosition.x < roomCenters[startRoomIndex].x)
            {
                exitClosed = Instantiate(exitGateLeftClosed, (Vector3Int)(gatePosition - new Vector2Int(1, 0)), Quaternion.identity);
                exitOpened = Instantiate(exitGateLeftOpened, (Vector3Int)(gatePosition - new Vector2Int(2, 2)), Quaternion.identity);
                exitOpened.SetActive(false);
                exitClosed.SetActive(true);
                for (int i = -4; i < 4; i++)
                    tilemapVisualizer.RemoveSingleBasicWall(gatePosition + new Vector2Int(-1, i), "side");
            }
            else if (gatePosition.y > roomCenters[startRoomIndex].y)
            {
                exitClosed = Instantiate(exitGateUpClosed, (Vector3Int)(gatePosition + new Vector2Int(0, 2)), Quaternion.identity);
                exitOpened = Instantiate(exitGateUpOpened, (Vector3Int)(gatePosition + new Vector2Int(-2, 2)), Quaternion.identity);
                exitOpened.SetActive(false);
                exitClosed.SetActive(true);
                for (int i = -2; i < 2; i++)
                    tilemapVisualizer.RemoveSingleBasicWall(gatePosition + new Vector2Int(i, -1), "front");
            }
            else if (gatePosition.y < roomCenters[startRoomIndex].y)
            {
                exitClosed = Instantiate(exitGateDownClosed, (Vector3Int)(gatePosition - new Vector2Int(0, 2)), Quaternion.identity);
                exitOpened = Instantiate(exitGateDownOpened, (Vector3Int)(gatePosition - new Vector2Int(1, 2)), Quaternion.identity);
                exitOpened.SetActive(false);
                exitClosed.SetActive(true);
                for (int i = -2; i < 2; i++)
                    tilemapVisualizer.RemoveSingleBasicWall(gatePosition + new Vector2Int(i, -1), "back");
            }
        }
    }

    private void RemoveFloorPositionsNearGate(Vector2Int gatePosition, string side)
    {
        Vector2Int positionToRemoveFloor = gatePosition;
        switch (side)
        {
            case "down":
                for(int y=0; y<6; y++)
                {
                    if (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(0, y)))
                    {
                        int i = -1;
                        while(floorPositions.Contains(positionToRemoveFloor + new Vector2Int(i, y)))
                        {
                            floorPositions.Remove(positionToRemoveFloor + new Vector2Int(i, y));
                            i--;
                        }
                        i = 0;
                        while (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(i, y)))
                        {
                            floorPositions.Remove(positionToRemoveFloor + new Vector2Int(i, y));
                            i++;
                        }
                    }
                }
                break;

            case "left":
                for (int i = 0; i < 6; i++)
                {
                    if (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(i, 0)))
                    {
                        int y = 1;
                        while (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(i, y)))
                        {
                            floorPositions.Remove(positionToRemoveFloor + new Vector2Int(i, y));
                            y++;
                        }
                        y = 0;
                        while (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(i, y)))
                        {
                            floorPositions.Remove(positionToRemoveFloor + new Vector2Int(i, y));
                            y--;
                        }
                    }
                }
                break;

            case "right":
                for (int i = 0; i > -6; i--)
                {
                    if (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(i, 0)))
                    {
                        int y = 1;
                        while (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(i, y)))
                        {
                            floorPositions.Remove(positionToRemoveFloor + new Vector2Int(i, y));
                            y++;
                        }
                        y = 0;
                        while (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(i, y)))
                        {
                            floorPositions.Remove(positionToRemoveFloor + new Vector2Int(i, y));
                            y--;
                        }
                    }
                }
                break;

            case "up":
                for (int y = 0; y > -6; y--)
                {
                    if (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(0, y)))
                    {
                        int i = -1;
                        while (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(i, y)))
                        {
                            floorPositions.Remove(positionToRemoveFloor + new Vector2Int(i, y));
                            i--;
                        }
                        i = 0;
                        while (floorPositions.Contains(positionToRemoveFloor + new Vector2Int(i, y)))
                        {
                            floorPositions.Remove(positionToRemoveFloor + new Vector2Int(i, y));
                            i++;
                        }
                    }
                }
                break;

        }
    }

    private Vector2Int CalculateShortestPathToWall(Vector2Int roomCenter)
    {
        int down = 0, left = 0, right = 0, up = 0;

        while (floorPositions.Contains(roomCenter + new Vector2Int(down, 0)))
            down++;
        while (floorPositions.Contains(roomCenter - new Vector2Int(up, 0)))
            up++;
        while (floorPositions.Contains(roomCenter + new Vector2Int(0, right)))
            right++;
        while (floorPositions.Contains(roomCenter - new Vector2Int(0, left)))
            left++;

        if (down <= left)
            if (down <= right)
                if (down <= up)
                    return roomCenter + new Vector2Int(down, 0);

        if (up <= down)
            if (up <= right)
                if (up <= left)
                    return roomCenter - new Vector2Int(up, 0);

        if (left <= down)
            if (left <= right)
                if (left <= up)
                    return roomCenter - new Vector2Int(0, left);

        return roomCenter + new Vector2Int(0, right);
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach(var room in roomList)
        {

            for(int col=offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                    position.x += 1;
                    position.y += 1;
                    floorPositions.Add(position);
                }
            }
        }
        return floor;
    }

    private IEnumerator InrtoCutsceneCo(string gateSide)
    {
        Player.MyInstance.currentState = PlayerState.cutscene;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.wait;
        }
        yield return null;
        entranceClosed.SetActive(false);
        entranceOpened.SetActive(true);
        MainCamera.MyInstance.target = entranceClosed.transform;
        yield return null;
        Vector2 playerStartPos = new Vector2();
        switch (gateSide)
        {
            case "down":
                playerStartPos = entranceOpened.transform.position - new Vector3(-1, 3, 0);
                break;
            case "left":
                playerStartPos = entranceOpened.transform.position - new Vector3(3, -2, 0);
                break;
            case "right":
                playerStartPos = entranceOpened.transform.position + new Vector3(3, 2, 0);
                break;
            case "up":
                playerStartPos = entranceOpened.transform.position + new Vector3(2, 3, 0);
                break;
        }
        Player.MyInstance.transform.position = playerStartPos;
        Player.MyInstance.animator.SetBool("isRunning", true);
        switch (gateSide)
        {
            case "down":
                Player.MyInstance.animator.SetFloat("Horizontal", 0f);
                Player.MyInstance.animator.SetFloat("Vertical", 1f);
                Player.MyInstance.toGoPosition = entranceOpened.transform.position + new Vector3(1, 5, 0);
                break;
            case "left":
                Player.MyInstance.animator.SetFloat("Horizontal", 1f);
                Player.MyInstance.animator.SetFloat("Vertical", 0f);
                Player.MyInstance.toGoPosition = entranceOpened.transform.position + new Vector3(5, 2, 0);
                break;
            case "right":
                Player.MyInstance.animator.SetFloat("Horizontal", -1f);
                Player.MyInstance.animator.SetFloat("Vertical", 0f);
                Player.MyInstance.toGoPosition = entranceOpened.transform.position - new Vector3(5, -2, 0);
                break;
            case "up":
                Player.MyInstance.animator.SetFloat("Horizontal", 0f);
                Player.MyInstance.animator.SetFloat("Vertical", -1f);
                Player.MyInstance.toGoPosition = entranceOpened.transform.position - new Vector3(-2, 5, 0);
                break;
        }
        foreach (PlayerEquipment pe in Player.MyInstance.equipments)
        {
            pe.SetXAndY(Player.MyInstance.animator.GetFloat("Horizontal"), Player.MyInstance.animator.GetFloat("Vertical"));
            pe.SetRunWalkBools("isRunning", Player.MyInstance.animator.GetBool("isRunning"),
                               "isWalking", Player.MyInstance.animator.GetBool("isWalking"));
        }
        yield return new WaitForSeconds(3f);
        Player.MyInstance.toGoPosition = Vector2.zero;
        Player.MyInstance.animator.SetBool("isRunning", false);
        foreach (PlayerEquipment pe in Player.MyInstance.equipments)
        {
            pe.SetRunWalkBools("isRunning", Player.MyInstance.animator.GetBool("isRunning"),
                               "isWalking", Player.MyInstance.animator.GetBool("isWalking"));
        }
        entranceOpened.SetActive(false);
        entranceClosed.SetActive(true);
        MainCamera.MyInstance.BigShake();
        yield return new WaitForSeconds(2f);
        MainCamera.MyInstance.cutscene = true;
        MainCamera.MyInstance.MoveToPosition(Player.MyInstance.transform);
        yield return new WaitForSeconds(1f);
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.idle;
        }
        MainCamera.MyInstance.cutscene = false;
        Player.MyInstance.currentState = PlayerState.run;
    }

    public IEnumerator OutroCutsceneCo(string gateSide)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.wait;
        }
        Player.MyInstance.currentState = PlayerState.cutscene;
        MainCamera.MyInstance.target = null;
        yield return null;
        Player.MyInstance.animator.SetBool("isRunning", false);
        Player.MyInstance.animator.SetBool("isWalking", true);
        switch (gateSide)
        {
            case "down":
                Player.MyInstance.animator.SetFloat("Horizontal", 0f);
                Player.MyInstance.animator.SetFloat("Vertical", -1f);
                Player.MyInstance.toGoPosition = Player.MyInstance.transform.position + new Vector3(0, -5, 0);
                break;
            case "left":
                Player.MyInstance.animator.SetFloat("Horizontal", -1f);
                Player.MyInstance.animator.SetFloat("Vertical", 0f);
                Player.MyInstance.toGoPosition = Player.MyInstance.transform.position + new Vector3(-5, 0, 0);
                break;
            case "right":
                Player.MyInstance.animator.SetFloat("Horizontal", 1f);
                Player.MyInstance.animator.SetFloat("Vertical", 0f);
                Player.MyInstance.toGoPosition = Player.MyInstance.transform.position - new Vector3(-5, 0, 0);
                break;
            case "up":
                Player.MyInstance.animator.SetFloat("Horizontal", 0f);
                Player.MyInstance.animator.SetFloat("Vertical", 1f);
                Player.MyInstance.toGoPosition = Player.MyInstance.transform.position - new Vector3(0, -5, 0);
                break;
        }
        foreach (PlayerEquipment pe in Player.MyInstance.equipments)
        {
            pe.SetXAndY(Player.MyInstance.animator.GetFloat("Horizontal"), Player.MyInstance.animator.GetFloat("Vertical"));
            pe.SetRunWalkBools("isRunning", Player.MyInstance.animator.GetBool("isRunning"),
                               "isWalking", Player.MyInstance.animator.GetBool("isWalking"));
        }
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Transition");
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(goToScene);
    }

    public IEnumerator OpenCo()
    {
        Player.MyInstance.currentState = PlayerState.cutscene;
        Player.MyInstance.animator.SetBool("isRunning", false);
        Player.MyInstance.animator.SetBool("isWalking", false);
        foreach (PlayerEquipment pe in Player.MyInstance.equipments)
        {
            pe.SetXAndY(Player.MyInstance.animator.GetFloat("Horizontal"), Player.MyInstance.animator.GetFloat("Vertical"));
            pe.SetRunWalkBools("isRunning", Player.MyInstance.animator.GetBool("isRunning"),
                               "isWalking", Player.MyInstance.animator.GetBool("isWalking"));
        }
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.wait;
        }
        yield return null;
        exitOpened.SetActive(true);
        exitOpened.SetActive(false);
        yield return null;
        MainCamera.MyInstance.BigShake();
        yield return new WaitForSeconds(2f);
        foreach (Enemy enemy in enemies)
        {
            enemy.currentState = EnemyStates.idle;
        }
        Player.MyInstance.currentState = PlayerState.run;
    }
}
