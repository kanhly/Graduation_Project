using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Run,
    Die
}

public class PlayerController : MonoBehaviour
{
    [Header("属性相关")]
    public int hp;
    public float speed;

    [Header("组件相关")]
    public Animator anim;
    public LineRenderer lr;
    public Rigidbody2D rb;
    public GameObject sprite;

    [Header("其他")]
    public bool isDead;
    public State currentState;
    public Material[] materials;

    public Transform startPos;
    public GameObject targetGO;
    public bool isOnRange = false;

    public Vector2 movement;
    float moveX;
    float moveY;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Idle;
        isDead = false;
        SetLineColor(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        ConnectLine();

        if (isDead)
            return;

        CheckInput();

        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Run:
                Run();
                break;
            case State.Die:
                Die();
                break;
        }

        //测试用
        if (Input.GetKeyDown(KeyCode.Q))
            UIController.Instance.OnTakeDamage(1);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    public void CheckInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;

        if (movement.Equals(Vector2.zero))
        {
            currentState = State.Idle;
        }
        else
        {
            currentState = State.Run;
            if (movement.x > 0)
                sprite.transform.localScale = new Vector3(1, 1, 1);
            else if(movement.x<0)
                sprite.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (PlayerModel.Instance.CurrentHp <= 0)
        {
            currentState = State.Die;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UIController.Instance.SetMiniMap(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            UIController.Instance.SetMiniMap(false);

        }
    }

    public void Idle()
    {
        anim.SetFloat("Speed", 0);
    }

    public void Run()
    {
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    public void Die()
    {
        anim.SetTrigger("Die");
        isDead = true;
    }

    public void ConnectLine()
    {
        if (targetGO != null)
        {
            if (isOnRange)
            {
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startWidth = 0.1f;
            }
            else
            {
                lr.material = materials[0];
                lr.startWidth = 0.5f;

            }

            lr.enabled = true;

            lr.SetPosition(0, startPos.position);
            lr.SetPosition(1, targetGO.transform.position);

            //SetLineColor(Color.red);           
        }
        else
        {
            lr.enabled = false;
        }
    }

    public void SetLineColor(Color color)
    {
        lr.startColor = color;
        lr.endColor = color;
    }
}
