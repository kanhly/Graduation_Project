using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor_Game : MonoBehaviour
{
    Entity go;

    private void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<Entity>(out go))
        {
            Debug.Log($"Hit {go}");
            MouseController.Instance.go = go;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<Entity>(out go))
        {          
            MouseController.Instance.go = null;
        }
    }
}
