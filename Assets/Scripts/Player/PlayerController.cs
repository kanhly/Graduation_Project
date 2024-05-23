using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Run,
    Die,
    Hurt
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

    float footStepInterv = 0.2f;
    float footStepTimer = 0;

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
        {
            movement = Vector2.zero;
            return;
        }

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
            //case State.Hurt:
            //    Hurt();
            //    break;
        }

        //测试用
        if (Input.GetKeyDown(KeyCode.Q))
            UIController.Instance.OnTakeDamage(1);
    }

    public void Hurt()
    {
        UIController.Instance.OnTakeDamage(1);
        AudioManager.Instance.PlayPlayerHurt();
        anim.SetTrigger("Hurt");
        Invoke("EnterIdle", 0.25f);
    }

    public void EnterIdle()
    {
        currentState = State.Idle;
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

        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    UIController.Instance.SetMiniMap(true);
        //}
        //if (Input.GetKeyUp(KeyCode.Tab))
        //{
        //    UIController.Instance.SetMiniMap(false);

        //}
    }

    public void Idle()
    {
        anim.SetFloat("Speed", 0);
    }

    public void Run()
    {
        footStepTimer -= Time.deltaTime;
        if (footStepTimer <=0)
        {
            PlayMoveAudio();
            footStepTimer = footStepInterv;
        }

        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    public void PlayMoveAudio()
    {
        AudioManager.PlayPlayerMove();
    }

    public void Die()
    {
        AudioManager.Instance.PlayFailClip();
        isDead = true;
        anim.SetTrigger("Die");
        Invoke("EnterEndScene", 3f);
    }

    public void EnterEndScene()
    {
        GameManager.Instance.EndScene();
        UIController.Instance.SetPlayerInfo(false);
        UIController.Instance.SetMiniMap(false);
        UIController.Instance.SetEndTitle(false);
        UIController.Instance.SetEndPanel(true);
        MouseController.Instance.SetRealCursor(false);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            UIController.Instance.OnTakeDamage(1);
            AudioManager.Instance.PlayPlayerHurt();

            //currentState = State.Hurt;
            //Hurt();
        }
    }


}
