using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Attack" ,menuName = "Character Stats/AttackData")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public float skillCoolDown;
    public int minDamge;
    public int maxDamge;
    public float criticalMultiplier; //±©»÷±¶ÂÊ
    public float criticalChance;  //±©»÷ÂÊ
}
