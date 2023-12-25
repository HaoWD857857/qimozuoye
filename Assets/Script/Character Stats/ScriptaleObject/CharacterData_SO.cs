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
        //�ȼ�Խ�ߣ�levelBuff����Խ�ߣ���������ֵ��Խ��
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
        //���������������ݵķ���
        currentLevel = Mathf.Clamp(currentLevel + 1,0,maxLevel);
        //��һ���ȼ�����Ҫ�ľ���ֵ
        //baseExp += (int)(baseExp * LevelMultiplier);
        //maxHealth = (int)(maxHealth * LevelMultiplier);
        baseExp = baseExp + 50;
        maxHealth = maxHealth + 30;
        GameManager.Instance.playerStats.attackData.minDamge += 1;
        GameManager.Instance.playerStats.attackData.maxDamge += 1;
        //����֮����Ѫ
        currentHealth = maxHealth;
        Debug.Log("LEVEL UP! ��ǰ�ȼ�:" + currentLevel);
    }
}
