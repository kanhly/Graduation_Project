using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecRange : MonoBehaviour
{
    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pc.targetGO != null && collision.gameObject.Equals(pc.targetGO))
        {
            pc.isOnRange = true;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (pc.targetGO != null && collision.gameObject.Equals(pc.targetGO))
        {
            pc.isOnRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(pc.targetGO) || pc.targetGO == null)
        {
            pc.isOnRange = false;
        }
    }
}
