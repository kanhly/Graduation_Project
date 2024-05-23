using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleRec : MonoBehaviour
{
    public Eagle eagle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            eagle.targetPos = collision.transform.position;
            eagle.curState = EnemyState.Attack;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&!eagle.isOnAtk)
        {

            eagle.targetPos = collision.transform.position;
            eagle.curState = EnemyState.Attack;
        }
    }
}
