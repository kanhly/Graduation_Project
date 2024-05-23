using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoSingleton<MouseController>
{
    public float checkTime = 1f;
    public PlayerController pc;

    public int layerMask;

    public RectTransform virtualCursor;
    public float radius=1f;
    public bool isConnect = false;
    bool isConStatic = false;
    public float sensitivity = 15f; // 移动灵敏度
    public GameObject cursor_game;

    public Vector2 mousePosRTCenter;

    Vector2 mousePos2D;
    RaycastHit2D hit;
    public Entity go;
    Entity currentGo;
    public bool isCoroRun;

    //限制物品移动
    Vector2 prePcPos;
    float mouseX;
    float mouseY;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Cube","Default");

        SetRealCursor(true);

        if(pc!=null)
            prePcPos = pc.transform.position;
        isCoroRun = false;

        //待修改
        virtualCursor = (RectTransform)UIController.Instance.gameObject.transform.Find("Cursor");
    }

    public void SetRealCursor(bool islock)
    {
        if (islock)
        {
            // 锁定光标到屏幕中心
            Cursor.lockState = CursorLockMode.Locked;
            // 隐藏系统光标
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        //修bug
        if (pc == null)
            pc = FindObjectOfType<PlayerController>();
        if (cursor_game == null)
            cursor_game = GameObject.Find("Cursor_Game");
        if(virtualCursor==null)
            virtualCursor = (RectTransform)GameObject.Find("Cursor").transform;


        mousePos2D = Camera.main.ScreenToWorldPoint(
            new Vector3(virtualCursor.position.x, virtualCursor.position.y, Camera.main.nearClipPlane));

        SetCursorPos();

        if (isConStatic)
        {
            ConStaticEntity();
        }
        else if (isConnect)
        {
            LimitCursorPos();
        }
        else
        {
            //Debug.Log(isCoroRun);
            if(!isCoroRun)
                StartCoroutine(CheckHit());
        }

        if (Input.GetMouseButtonDown(0))
        {
            MouseClick_0();
        }

        if (Input.GetMouseButtonDown(1))
        {
            MouseClick_1();
        }

    }

    public void LimitCursorPos()
    {
        SetCursorVisible(false);
        sensitivity = 10f;

        StopCoroutine(CheckHit());
        Vector2 constrainedPos = new Vector2(0, 0);

        if (!pc.movement.Equals(Vector2.zero))
        {

            Vector2 difValue =
                new Vector2(pc.transform.position.x - prePcPos.x, pc.transform.position.y - prePcPos.y);
            prePcPos = pc.transform.position;
            currentGo.transform.position += new Vector3(difValue.x, difValue.y, 0);

            virtualCursor.position = Camera.main.WorldToScreenPoint(currentGo.transform.position);

            Vector2 goPosRTCenter = new Vector2(
                currentGo.transform.position.x - pc.transform.position.x,
                currentGo.transform.position.y - pc.transform.position.y);

            if (goPosRTCenter.magnitude != radius)
            {
                currentGo.transform.position =
                    new Vector2(pc.transform.position.x, pc.transform.position.y)
                    + (goPosRTCenter.normalized * radius);
            }

        }
        else
        {
            mousePosRTCenter = new Vector2(
                mousePos2D.x - pc.transform.position.x, mousePos2D.y - pc.transform.position.y);
            constrainedPos =
                    new Vector2(pc.transform.position.x, pc.transform.position.y)
                    + (mousePosRTCenter.normalized * radius);

            currentGo.transform.position = constrainedPos;
        }

        if (mouseX == 0 && mouseY == 0)
        {
            virtualCursor.position = Camera.main.WorldToScreenPoint(currentGo.transform.position);

        }
    }

    public void SetCursorVisible(bool flag)
    {
        virtualCursor.GetComponent<Image>().enabled = flag;
        if(cursor_game!=null)
            cursor_game.GetComponent<SpriteRenderer>().enabled = flag;

    }

    public void SetCursorPos()
    {
        // 获取鼠标移动量
        mouseX = Input.GetAxis("Mouse X") * sensitivity; 
        mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // 更新虚拟光标位置
        virtualCursor.position += new Vector3(mouseX, mouseY, 0);

        //限制鼠标位置在屏幕边缘
        float newX = Mathf.Clamp(virtualCursor.position.x + mouseX, 0, Screen.width);
        float newY = Mathf.Clamp(virtualCursor.position.y + mouseY, 0, Screen.height);
        virtualCursor.position = new Vector2(newX, newY);

        Vector2 cursorPos= Camera.main.ScreenToWorldPoint(virtualCursor.position);

        if(GameManager.Instance.ReturnSceneName().Equals("GameScene"))
            cursor_game.transform.position = cursorPos;

        //cursor_game.GetComponent<Rigidbody2D>().MovePosition(Camera.main.ScreenToWorldPoint(virtualCursor.position));

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            isConnect = false;
        }
    }

    public void ConStaticEntity()
    {
        SetCursorVisible(false);
        if (!pc.isOnRange&&pc!=null)
        {
            MouseClick_1();
        }
    }

    public void MouseClick_0()
    {
        if (go != null && pc.isOnRange)
        {
            AudioManager.Instance.PlayConClip();
            if (go.CompareTag("Cube_Static"))
            {
                isConStatic = true;
            }
            else
            {
                isConnect = true;//限制鼠标位置
                prePcPos = pc.transform.position;//设置prePcPos位置
            }

            pc.SetLineColor(Color.blue);
            currentGo = go;
            SpeechRecognition.Instance.StartRecognize();
            go.Register();
        }
        else
            Debug.Log("未获得点击的脚本组件 或 与角色范围不够");
        
    }

    public void MouseClick_1()
    {
        SetCursorVisible(true);
        sensitivity = 20f;
        pc.SetLineColor(Color.red);

        if (currentGo != null)
        {
            isConnect = false;//限制鼠标位置
            isConStatic = false;    
        }

        StartCoroutine(CheckHit());

        SpeechRecognition.Instance.UnSubscribe();    
        Debug.Log("结束语音识别");
    }

    IEnumerator CheckHit()
    {
        isCoroRun = true;

        if (go != null&&pc != null)
        {
            //当检测到相应物品时，从角色位置发射一条射线到物品上，如果检测到物品不相同则说明途中有障碍物阻挡，将targetGO赋空
            LayerMask pcRayLayerMask = LayerMask.GetMask("Cube", "Wall");
            RaycastHit2D pcRay = Physics2D.Raycast(pc.transform.position,
                new Vector2(go.transform.position.x - pc.transform.position.x,
                go.transform.position.y - pc.transform.position.y),
                100, pcRayLayerMask);
            Debug.Log(pcRay.collider.gameObject);
            //Debug.DrawLine(pc.transform.position,new Vector2(go.transform.position.x - pc.transform.position.x,
            //    go.transform.position.y - pc.transform.position.y)*100);

            if (pc.targetGO == null)
            {
                pc.targetGO = go.gameObject;
            }
            else if (!pc.targetGO.Equals(go.gameObject))
            {
                pc.targetGO = go.gameObject;
            }

            if (!pcRay.collider.gameObject.Equals(pc.targetGO))
                pc.targetGO = null;
        }
        else
        {
            pc.targetGO = null;
        }

        yield return new WaitForSeconds(checkTime);
        isCoroRun = false;
    }
}
