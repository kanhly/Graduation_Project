using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Wall : MonoBehaviour
{
    public GameObject wallPart;//小地图墙壁part
    public Door door_UD;
    public Door door_LR;

    [Header("WallPart生成坐标")]
    public Transform wUp;
    public Transform wDown;
    public Transform wLeft;
    public Transform wRight;

    [Header("Door生成坐标")]
    public Transform dUp;
    public Transform dDown;
    public Transform dLeft;
    public Transform dRight;

    public Tilemap wall;
    public Tile[] udTiles;
    public Tile[] lrTiles;

    public Vector3Int upBase;
    public Vector3Int downBase;
    public Vector3Int leftBase;
    public Vector3Int rightBase;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.Find("WallBase").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 需要注意tiles数组的存放顺序，_X是先竖着依次填充再沿着x轴重复xOffset次，_Y反之
    /// </summary>
    /// <param name="dir"></param>
    public void FillTiles_X(Tile[] tiles,Vector3Int startPos,int xOffset)
    {
        Vector3Int currentPos = startPos;
        for(int i = 0; i < xOffset; i++)
        {
            for(int j = 0; j < tiles.Length; j++)
            {
                wall.SetTile(currentPos, tiles[j]);
                currentPos.y++;
            }
            currentPos.y = startPos.y;
            currentPos.x++;
        }
    }

    public void FillTiles_Y(Tile[] tiles, Vector3Int startPos, int yOffset)
    {
        Vector3Int currentPos = startPos;
        for (int i = 0; i < yOffset; i++)
        {
            for (int j = 0; j < tiles.Length; j++)
            {
                wall.SetTile(currentPos, tiles[j]);
                currentPos.x++;
            }
            currentPos.x = startPos.x;
            currentPos.y++;
        }
    }

    public void SetWall(Direction dir)
    {
        GameObject go=null;
        switch (dir)
        {
            case Direction.Up:
                go=Instantiate(wallPart, wUp.position,Quaternion.identity);
                FillTiles_X(udTiles,upBase,4);
                break;
            case Direction.Down:
                go = Instantiate(wallPart, wDown.position,Quaternion.identity);
                FillTiles_X(udTiles, downBase, 4);
                break;
            case Direction.Left:
                go = Instantiate(wallPart, wLeft.position, Quaternion.Euler(0, 0, 90));
                FillTiles_Y(lrTiles, leftBase, 5);
                break;
            case Direction.Right:
                go = Instantiate(wallPart, wRight.position, Quaternion.Euler(0, 0, -90));
                FillTiles_Y(lrTiles, rightBase, 5);
                break;
        }
        go.SetActive(true);
        go.transform.SetParent(gameObject.transform.Find("WallBase"));
    }

    public Door SetDoor(Direction dir)
    {
        Door door = null;
        switch (dir)
        {
            case Direction.Up:
                door = Instantiate(door_UD, dUp.position, Quaternion.identity);
                break;
            case Direction.Down:
                door = Instantiate(door_UD, dDown.position, Quaternion.identity);
                break;
            case Direction.Left:
                door = Instantiate(door_LR, dLeft.position, Quaternion.identity);
                break;
            case Direction.Right:
                door = Instantiate(door_LR, dRight.position, Quaternion.identity);
                break;
        }
        door.gameObject.SetActive(true);
        return door;
        //go.transform.SetParent(gameObject.transform.Find("WallBase"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.transform.Find("WallBase").gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.transform.Find("WallBase").gameObject.SetActive(true);
        }
    }
}
