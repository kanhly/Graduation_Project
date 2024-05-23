using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public bool roomLeft, roomRight, roomUp, roomDown;

    public bool isFinish=false;

    public List<Door> doors = new List<Door>();

    public LevelController lc;

    public Wall wall;

    private void Start()
    {
        lc.register += SetFinish;
    }

    private void Update()
    {
        ////≤‚ ‘”√
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    foreach(var door in doors)
        //    {
        //        door.ChangeDoorState();
        //    }
        //}
    }

    public void SetFinish()
    {
        isFinish = true;
    }

    public void OpenDoors()
    {
        foreach (var door in doors)
        {
            door.OpenDoor();
        }
    }

    public void CloseDoors()
    {
        foreach (var door in doors)
        {
            door.CloseDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger == false)
        {
            lc.gameObject.SetActive(true);
            CameraController.Instance.ChangeTarget(transform);
            RoomManager.Instance.curRoom = this;
            wall.transform.Find("WallBase").gameObject.SetActive(true);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&collision.isTrigger==false)
        {
            CameraController.Instance.ChangeTarget(transform);

            RoomManager.Instance.curRoom = this;
            wall.transform.Find("WallBase").gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger == false)
        {
            lc.gameObject.SetActive(false);      
        }
    }
}
