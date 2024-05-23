using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Target : MonoBehaviour
{
    public Animator anim;

    public bool isHit;

    private void Start()
    {
        anim = GetComponent<Animator>();
        isHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lasing")&&!isHit)
        {
        Debug.Log("lasing.");
            AudioManager.Instance.PlayCrystalOn();
            isHit = true;
            anim.Play("Target_ChangeColor");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Lasing") && !isHit)
        {
            Debug.Log("lasing.");
            AudioManager.Instance.PlayCrystalOn();

            isHit = true;
            anim.Play("Target_ChangeColor");
        }
    }
}
