using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoSingleton<UIController>
{
    public ViewPlayerInfo viewPlayerInfo;
    public MiniMap miniMap;
    public GameObject startPanel;

    void Start()
    {
        PlayerModel.Instance.OnMaxHpChange += viewPlayerInfo.SetMaxHp;
        PlayerModel.Instance.OnCurHpChange += viewPlayerInfo.UpdateHp;

        PlayerModel.Instance.Init();
    }

    public void OnTakeDamage(int amount)
    {
        PlayerModel.Instance.TakeDamage(amount);
    }

    public void SetMiniMap(bool flag)
    {
        miniMap.gameObject.SetActive(flag);
    }

    public void SetPlayerInfo(bool flag)
    {
        viewPlayerInfo.gameObject.SetActive(flag);
    }

    public void SetStartPanel(bool flag)
    {
        startPanel.SetActive(flag);
        SetPlayerInfo(true);
        GameManager.Instance.StartGame();
    }
}
