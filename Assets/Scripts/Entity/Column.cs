using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : Entity
{
    Animator anim;
    public Crystal crystal;
    bool isHitByCrystal = false;

    private void Start()
    {
        crystal.isFire = false;
        anim = GetComponent<Animator>();
    }

    public override void Rotate()
    {
        AudioManager.Instance.PlayColumnRotate();
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
            if (collision.transform.parent.name.Equals("Crystal"))
                isHitByCrystal = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Lasing"))
        {
            crystal.isFire = true;
            if (collision.transform.parent.name.Equals("Crystal"))
                isHitByCrystal = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lasing")&&collision.transform.parent.name.Equals("Crystal"))
            isHitByCrystal = false;

        if (collision.CompareTag("Lasing") && !isHitByCrystal)
        {
            crystal.isFire = false;
            Debug.Log("exit");
        }
    }
}
