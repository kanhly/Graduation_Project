using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Entity
{
    public enum CubeState { Idle, Rotate, Fire,Scale}

    public Transform PlayerPos;
    public int flySpeed;
    public Animator anim;
    public AnimationClip rotateClip;

    private Rigidbody2D rb;

    private Vector3 prePos;
    CubeState cubeState;
    bool isOnScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        cubeState = CubeState.Idle;
        isOnScale = false;
    }

    private void Update()
    {
        switch (cubeState)
        {
            case CubeState.Idle:
                OnIdle();
                break;
            case CubeState.Rotate:
                OnRotate();
                break;
            case CubeState.Fire:
                OnFire();
                break;
            case CubeState.Scale:
                OnScale();
                break;
        }
    }

    public void EnterIdle()
    {
        cubeState = CubeState.Idle;
    }

    public void OnIdle()
    {
        rb.velocity = new Vector2(0, 0);
        anim.Play("Cube_Idle");
    }

    public void OnRotate()
    {
        anim.Play(rotateClip.name);
        Invoke("EnterIdle", rotateClip.length);
    }

    public void OnFire()
    {
        rb.velocity = flySpeed * MouseController.Instance.mousePosRTCenter.normalized * Time.deltaTime;
        //Debug.Log(MouseController.Instance.mousePosRTCenter.normalized);
    }

    public void OnScale()
    {
        if (isOnScale)
        {
            transform.localScale *= 0.5f;
            isOnScale = false;
            EnterIdle();
        }
        else
        {
            transform.localScale *= 2;
            isOnScale = true;
            EnterIdle();
        }
    }

    public override void Fire()
    {
        cubeState = CubeState.Fire;
        Debug.Log("Cube Fire.");
    }

    public override void Rotate()
    {
        cubeState = CubeState.Rotate;
        Debug.Log("Cube Rotate.");
    }

    public override void Scale()
    {
        cubeState = CubeState.Scale;
        Debug.Log("Cube Scale.");
    }

    public override void Register()
    {
        Debug.Log("register success");
        SpeechRecognition.Instance.keys["·¢Éä"] += Fire;
        SpeechRecognition.Instance.keys["Ðý×ª"] += Rotate;
        SpeechRecognition.Instance.keys["Ëõ·Å"] += Scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            cubeState = CubeState.Idle;
            prePos = transform.position;
            MouseController.Instance.MouseClick_1();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            cubeState = CubeState.Idle;
            transform.position = prePos;
        }
    }
}