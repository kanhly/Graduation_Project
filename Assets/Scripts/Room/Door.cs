using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isUD_Door;

    public AnimationClip openClip;
    public AnimationClip closeClip;

    bool isOpen = false;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor()//开门方法
    {
        if (!isOpen)
        {
            anim.Play(openClip.name);
            isOpen = true;
        }
    }

    public void CloseDoor()//关门方法
    {
        if (isOpen)
        {
            anim.Play(closeClip.name);
            isOpen = false;
        }
    }

    public void ChangeDoorState()
    {
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
}
