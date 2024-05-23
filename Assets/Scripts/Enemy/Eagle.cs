using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum EnemyState
{
    Idle,Attack,Patrol,Die
}

public class Eagle : MonoBehaviour
{
    Animator anim;
    [SerializeField] Rigidbody2D rb;

    public Vector2 targetPos;
    public EnemyState curState;
    [SerializeField] float attackSpeed;
    [SerializeField] float flySpeed;
    float speed;

    [SerializeField] GameObject thisGo;
    [SerializeField] bool isPatrolRan;
    [SerializeField] float randRange;
    Vector2 prePos;
    public bool isOnAtk;
    bool isOnPatr;

    public bool isDie;
    public UnityAction levelAction;

    private void Start()
    {
        anim = GetComponent<Animator>();

        curState = EnemyState.Idle;
        prePos = transform.position;
        isOnAtk = false;
        isOnPatr = false;
        isDie = false;
    }

    private void Update()
    {
        if (transform.position.x -prePos.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 0);
        }
        else if(transform.position.x-prePos.x<0)
        {
            transform.localScale = new Vector3(1, 1, 0);
        }
        prePos = transform.position;

        switch (curState) {
            case EnemyState.Idle:
                OnIdle();
                break;
            case EnemyState.Attack:
                EnterAtk();
                break;
            case EnemyState.Die:
                OnDie();
                break;
        }
    }

    void OnIdle()
    {

        anim.SetTrigger("idle");
        //anim.Play("EnemyE_Idle");
        speed = flySpeed;
        if (!isOnPatr)
        {
            StartCoroutine(OnPatrol());
        }
    }

    IEnumerator OnPatrol()
    {
        isOnPatr = true;
        Vector2 pos;
        LayerMask layer = LayerMask.GetMask("Wall");

        while (true)
        {
            float x = Random.Range(-randRange, randRange+0.2f);
            float y = Random.Range(-randRange, randRange+0.2f);

            pos = new Vector2(thisGo.transform.position.x + x, thisGo.transform.position.y + y);
            RaycastHit2D hit = Physics2D.Raycast(thisGo.transform.position,
                (pos - new Vector2(thisGo.transform.position.x, thisGo.transform.position.y)).normalized,
                new Vector2(x,y).magnitude,
                layer
                );
            if (!hit)
            {
                break;
            }
            else
            {
                Debug.Log("检测到Wall");
            }
        }

        //Debug.Log(pos);

        targetPos = pos;
        yield return StartCoroutine(MoveTowardsTarget());
        isOnPatr = false;
    }

    void OnAttack()
    {
        speed = attackSpeed;
        if (!isOnAtk)
        {
            StartCoroutine(MoveTowardsTarget());
            curState = EnemyState.Idle;

        }
    }

    void EnterAtk()
    {
        anim.SetTrigger("attack");
    }

    void OnDie()
    {
        this.GetComponent<CircleCollider2D>().gameObject.SetActive(false);
        isDie = true;
        anim.SetTrigger("die");
        AudioManager.PlayEnemyDie();
        levelAction.Invoke();
    }

    public void SetThisGOUnactive()
    {
        thisGo.SetActive(false);

    }

    IEnumerator MoveTowardsTarget()
    {
        isOnAtk = true;
        // 持续移动直到足够靠近目标位置
        while (Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb.position, targetPos, speed * Time.deltaTime);
            rb.MovePosition(newPosition);
            yield return null;  // 等待下一帧
        }
        isOnAtk = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Enter.");
        //Debug.Log(collision.gameObject);
        if (collision.gameObject.CompareTag("Cube"))
        {
            curState = EnemyState.Die;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.position = prePos;
            curState = EnemyState.Idle;
        }
    }
}
