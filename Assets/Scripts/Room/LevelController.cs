using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [SerializeField] bool isPuzzle;

    public List<Eagle> eagleEnemies;
    public Crystal_Target targetCrystal;
    public UnityAction register;

    public int count = 0;

    private void Start()
    {
        //Debug.Log($"count:{count},enemyc:{eagleEnemies.Count}");
        foreach(var i in eagleEnemies)
        {
            i.levelAction += AddCount;
        }
    }

    private void Update()
    {
        if (isPuzzle)
        {
            if (targetCrystal.isHit)
            {
                register.Invoke();
                foreach(var i in eagleEnemies)
                {
                    i.curState = EnemyState.Die;
                }
            }
        }
        else
        {
            if (CheckEagles())
            {
                register.Invoke();
            }
        }
    }

    public bool CheckEagles()
    {       
        if (count.Equals(eagleEnemies.Count))
            return true;
        else
            return false;
    }

    public void AddCount()
    {
        count++;
    }
}
