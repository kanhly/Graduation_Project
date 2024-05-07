using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewPlayerInfo : MonoBehaviour
{
    public Slider hp;
    public Slider shield;

    public Text hpInfo;
    public Text shieldInfo;

    public void UpdateHp(int currentHp)
    {
        hp.value = currentHp;
        UpdateHpInfo();
    }

    public void SetMaxHp(int maxHp)
    {
        hp.maxValue = maxHp;
        UpdateHpInfo();
    }

    private void UpdateHpInfo()
    {
        int curHp = PlayerModel.Instance.CurrentHp;
        int maxHp = PlayerModel.Instance.MaxHp;
        hpInfo.text = curHp.ToString() + "/" + maxHp.ToString();
    }
    
}
