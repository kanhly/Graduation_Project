using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public LineRenderer line;
    public bool isFire;
    public Transform firePos;
    public Transform startPos, upPos, downPos, leftPos, rightPos;
    public Direction direction;
    public float checkTime = 0.2f;
    public GameObject bullet;
    public Rigidbody2D rb;

    public LayerMask layer;
    Vector2 hitDir;
    Vector2 endPos;
    bool isCoroRun=false;
    Vector2 PrePos;

    private void Start()
    {
        isFire = true;
    }

    private void Update()
    {
        if(!isFire)
        {
            Sleep();
        }else if (!isCoroRun)
        {
            StartCoroutine(Lasing());

        }
    }

    public void Sleep()
    {
        line.gameObject.SetActive(false);
        StopCoroutine(Lasing());
        bullet.SetActive(false);
    }

    IEnumerator Lasing()
    {
        isCoroRun = true;

        //测试暂时放在这，后面挪到start里
        switch (direction)
        {
            case Direction.Up:
                hitDir = Vector2.up;
                firePos = upPos;
                break;
            case Direction.Down:
                hitDir = Vector2.down;
                firePos = downPos;
                break;
            case Direction.Left:
                hitDir = Vector2.left;
                firePos = leftPos;
                break;
            case Direction.Right:
                hitDir = Vector2.right;
                firePos = rightPos;
                break;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(firePos.position, hitDir,Mathf.Infinity,layer);

        Debug.Log(hitInfo.transform.gameObject.name);

        endPos= new Vector2(100, 0);

        if (hitInfo)
        {
            endPos = hitInfo.point;
        }

        line.gameObject.SetActive(true);

        line.SetPosition(0, transform.InverseTransformPoint(startPos.position));
        line.SetPosition(1, transform.InverseTransformPoint(endPos));

        if (!endPos.Equals(PrePos))
        {
            //float time = 0;
            //while (time < checkTime)
            //{
            //    //非物理的移动方式会影响物理检测
            //    //bullet.transform.localPosition = 
            //    //    Vector3.Lerp(firePos.localPosition, transform.InverseTransformPoint(endPos), time / checkTime);

            //    //物理的移动方式
            //    Vector3 newPos = Vector3.Lerp(firePos.position, endPos, time / checkTime);
            //    rb.MovePosition(newPos);
            //    time += Time.deltaTime;
            //    yield return null;
            //}
            //bullet.transform.localPosition = transform.InverseTransformPoint(endPos);
            //rb.MovePosition(endPos);
            //rb.velocity = Vector3.zero;//修bug

            bullet.SetActive(true);
            bullet.transform.position = endPos;
        }
        PrePos = endPos;

        yield return new WaitForSeconds(checkTime);
        isCoroRun = false;
    }
}
