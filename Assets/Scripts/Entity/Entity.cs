using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool isRotate;
    public bool isFire;
    public bool isScale;

    public virtual void Rotate() { }  

    public virtual void Fire() { }

    public virtual void Scale() { }

    public virtual void Register() { }
}
