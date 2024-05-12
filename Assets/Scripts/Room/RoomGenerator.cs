using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public enum Direction { Up, Down, Left, Right };

[System.Serializable]
public class Level
{
    public GameObject level;
    public float weight;
}

public class RoomGenerator : MonoBehaviour
{
    public Direction direction;

    private int generateIdx = 0;

    [Header("房间信息")]
    public GameObject roomPrefab;
    public int roomNumber;
    private Room endRoom;

    public GameObject wall;

    [Header("位置控制")]
    public Transform generatorPoint;
    public float xOffset;
    public float yOffset;

    public List<Room> rooms = new List<Room>();
    public HashSet<Vector3> rPoints = new HashSet<Vector3>();
    public HashSet<Vector3> dPoints = new HashSet<Vector3>();

    private Dictionary<Vector3, List<Vector3>> adjList = new Dictionary<Vector3, List<Vector3>>();
    private List<Vector3> longestPath = new List<Vector3>();

    [Header("关卡")]
    public List<Level> levels = new List<Level>();
    List<int> weightPoul = new List<int>();
    public int baseNum = 100;

    // Start is called before the first frame update
    void Start()
    {
        SetWeightPoul();

        Vector3 startPoint = generatorPoint.position;
        rPoints.Add(generatorPoint.position);
        rooms.Add(Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>());
        for (int i = 0; i < roomNumber; i++)
        {
            ChangePointPos();
            rooms.Add(Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>());
        }

        //标记初始房间颜色
        //rooms[0].GetComponentInChildren<Text>().text = "Start";
        endRoom = rooms[0];

        //设置墙壁
        foreach (var room in rooms)
        {
            SetupRoom(room, room.transform.position);
        }


        //FindLongPath();

        Vector3 endPos = BFS(rooms[0].transform.position);
        foreach (var room in rooms)
        {
            //if (room.transform.position.Equals(longestPath[longestPath.Count - 1]))
            //    endRoom = room;

            if (room.transform.position.Equals(endPos))
                endRoom = room;
        }

        //Debug.Log(longestPath.Count);
        //Debug.Log(longestPath[longestPath.Count - 1]);

        //SetupEndRoom(endRoom, endRoom.transform.position);


        //endRoom.GetComponentInChildren<Text>().text = "End";
    }

    // Update is called once per frame
    void Update()
    {
        //测试用
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
    }

    public void AddEdge(Vector3 src,Vector3 dest)
    {
        if (!adjList.ContainsKey(src))
            adjList[src] = new List<Vector3>();
        adjList[src].Add(dest);
        if (!adjList.ContainsKey(dest))
            adjList[dest] = new List<Vector3>();
        adjList[dest].Add(src);
    }


    public Vector3 BFS(Vector3 start)
    {
        Queue<(Vector3, int)> queue = new Queue<(Vector3, int)>();
        HashSet<Vector3> visited = new HashSet<Vector3>();
        queue.Enqueue((start, 0));
        visited.Add(start);

        Vector3 farthestNode = start;
        int maxDist = 0;

        while (queue.Count > 0)
        {
            (Vector3 current, int dist) = queue.Dequeue();

            foreach(Vector3 neighbor in adjList[current])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue((neighbor, dist + 1));
                    if (dist + 1 > maxDist)
                    {
                        maxDist = dist + 1;
                        farthestNode = neighbor;
                    }
                }
            }

        }

        return farthestNode;
    }

    public void ChangePointPos()
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);

            switch (direction)
            {
                case Direction.Up:
                    generatorPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.Down:
                    generatorPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.Right:
                    generatorPoint.position += new Vector3(xOffset, 0, 0);
                    break;
                case Direction.Left:
                    generatorPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
            }
            //if (generateIdx < 2) //限制开始房间只有最多两个房间与之相连
            //{
            //    points.Add(generatorPoint.position * -1);
            //    generateIdx++;
            //}
        } while (rPoints.Contains(generatorPoint.position));

        rPoints.Add(generatorPoint.position);
    }

    public void SetupRoom(Room newRoom,Vector3 roomPosition)
    {
        newRoom.roomUp = rPoints.Contains(roomPosition + new Vector3(0, yOffset, 0));
        newRoom.roomDown = rPoints.Contains(roomPosition + new Vector3(0, -yOffset, 0));
        newRoom.roomRight = rPoints.Contains(roomPosition + new Vector3(xOffset, 0, 0));
        newRoom.roomLeft = rPoints.Contains(roomPosition + new Vector3(-xOffset, 0, 0));

        Wall wallBase=Instantiate(wall, roomPosition, Quaternion.identity).GetComponent<Wall>();
        wallBase.transform.SetParent(newRoom.transform);
        //绘制墙壁
        if (newRoom.roomUp != true)
            wallBase.SetWall(Direction.Up);    
        if (newRoom.roomDown != true)
            wallBase.SetWall(Direction.Down);
        if (newRoom.roomLeft != true)
            wallBase.SetWall(Direction.Left);
        if (newRoom.roomRight != true)
            wallBase.SetWall(Direction.Right);

        //添加图节点 并 放置门（每个房间只需要放置上门和右门即可全部覆盖）
        if(newRoom.roomUp == true)
        {
            AddEdge(roomPosition, roomPosition + new Vector3(0, yOffset, 0));
            Room upRoom = FindRoom(roomPosition + new Vector3(0, yOffset, 0));
            Door door = wallBase.SetDoor(Direction.Up);
            newRoom.doors.Add(door);
            if (upRoom) upRoom.doors.Add(door);
        }
        if (newRoom.roomDown == true)
        {
            AddEdge(roomPosition, roomPosition + new Vector3(0, -yOffset, 0));
        }
        if (newRoom.roomLeft == true)
        {
            AddEdge(roomPosition, roomPosition + new Vector3(-xOffset, 0, 0));
        }
        if (newRoom.roomRight == true)
        {
            AddEdge(roomPosition, roomPosition + new Vector3(xOffset, 0, 0));
            Room rightRoom = FindRoom(roomPosition + new Vector3(xOffset, 0, 0));
            Door door = wallBase.SetDoor(Direction.Right);
            newRoom.doors.Add(door);
            if (rightRoom) rightRoom.doors.Add(door);
        }

        newRoom.lc= GenerateLevel(newRoom).GetComponent<LevelController>();
    }

    public Room FindRoom(Vector3 pos)
    {
        foreach(var room in rooms)
        {
            if (room.transform.position.Equals(pos))
                return room;
        }
        return null;
    }

    public void SetWeightPoul()
    {
        float totalWeight = 0;
        foreach (var level in levels)
        {
            totalWeight += level.weight;
        }

        for(int i = 0; i < levels.Count; i++)
        {
            int count = (int)Mathf.Round(levels[i].weight / totalWeight * baseNum);
            for(int j = 0; j < count; j++)
            {
                weightPoul.Add(i);
            }
        }
    }

    public GameObject GenerateLevel(Room room)
    {
        int ran = Random.Range(0, weightPoul.Count);

        int index = weightPoul[ran];

        if (levels[index].level!=null)
        {
            GameObject go = Instantiate(levels[index].level,room.transform);
            return go;
        }
        return null;
    }

    //public void SetupEndRoom(Room endRoom, Vector3 roomPosition)
    //{
    //    endRoom.roomUp = longestPath[longestPath.Count - 2].Equals(roomPosition + new Vector3(0, yOffset, 0));
    //    endRoom.roomDown = longestPath[longestPath.Count - 2].Equals(roomPosition + new Vector3(0, -yOffset, 0));
    //    endRoom.roomRight = longestPath[longestPath.Count - 2].Equals(roomPosition + new Vector3(xOffset, 0, 0));
    //    endRoom.roomLeft = longestPath[longestPath.Count - 2].Equals(roomPosition + new Vector3(-xOffset, 0, 0));

    //    Wall wallBase = endRoom.GetComponentInChildren<Wall>();

    //    if (endRoom.roomUp != true)
    //        wallBase.SetWall(Direction.Up);
    //    if (endRoom.roomDown != true)
    //        wallBase.SetWall(Direction.Down);
    //    if (endRoom.roomLeft != true)
    //        wallBase.SetWall(Direction.Left);
    //    if (endRoom.roomRight != true)
    //        wallBase.SetWall(Direction.Right);

    //    wallBase.GetComponentInChildren<TilemapRenderer>().sortingOrder = roomNumber;
    //}
}
