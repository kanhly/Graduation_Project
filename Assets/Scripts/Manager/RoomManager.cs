using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoSingleton<RoomManager>
{
    public Room curRoom;

    private void Update()
    {
        if (curRoom != null && !curRoom.isFinish)
        {
            curRoom.CloseDoors();
        }

        if (curRoom!=null&&curRoom.isFinish==true)
        {
            curRoom.isFinish = true;
            curRoom.OpenDoors();
        }
    }

}
