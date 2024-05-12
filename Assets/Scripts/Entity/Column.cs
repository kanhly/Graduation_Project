using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : Entity
{
    Animator anim;
    public Crystal crystal;

    private void Start()
    {
        crystal.isFire = false;
        anim = GetComponent<Animator>();
    }

    public override void Rotate()
    {
        anim.SetTrigger("Rotate");
    }

    public override void Register()
    {
        SpeechRecognition.Instance.keys["Ðý×ª"] += Rotate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lasing"))
        {
            crystal.isFire = true;
            Debug.Log("enter");

        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Lasing"))
    //    {
    //        crystal.isFire = false;
    //        Debug.Log("exit");
    //    }
    //}
}
