using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnValueChange(int value);

public class PlayerModel
{
    //单例
    private static PlayerModel instance;
    public static PlayerModel Instance
    {
        get
        {
            if (instance == null)
                instance = new PlayerModel();
            return instance;
        }
           
    }
    private PlayerModel() { }

    private int currentHp;//当前生命值
    private int maxHp;//最大生命值

    public OnValueChange OnCurHpChange;
    public OnValueChange OnMaxHpChange;

    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;
            OnCurHpChange?.Invoke(currentHp);
        }
    }

    public int MaxHp
    {
        get { return maxHp; }
        set
        {
            maxHp = value;
            OnMaxHpChange?.Invoke(maxHp);
        }
    }

    public void Init()
    {
        MaxHp = 5;//先后顺序有影响
        CurrentHp = 5;
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        Mathf.Clamp(currentHp, 0, MaxHp);
        OnCurHpChange(currentHp);
    }
}
