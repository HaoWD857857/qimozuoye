using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data",menuName ="Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int maxHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    [Header("Kill")]
    public int killPoint;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;

    public float LevelMultiplier
    {
        //等级越高，levelBuff增量越高，升级的数值就越高
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }

    public void UpdataExp(int point)
    {
        currentExp += point;
        if (currentExp >= baseExp)
            LebeUp();
    }

    private void LebeUp()
    {
        //所有你想提升数据的方法
        currentLevel = Mathf.Clamp(currentLevel + 1,0,maxLevel);
        //下一个等级所需要的经验值
        //baseExp += (int)(baseExp * LevelMultiplier);
        //maxHealth = (int)(maxHealth * LevelMultiplier);
        baseExp = baseExp + 50;
        maxHealth = maxHealth + 30;
        GameManager.Instance.playerStats.attackData.minDamge += 1;
        GameManager.Instance.playerStats.attackData.maxDamge += 1;
        //升级之后满血
        currentHealth = maxHealth;
        Debug.Log("LEVEL UP! 当前等级:" + currentLevel);
    }
}
