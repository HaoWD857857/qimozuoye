using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //event Action<>:所有订阅了这个事件添加进去的方法都会被执行,需要注册(在HealthBarUI)
    public event Action<int, int> UpdataHealthBarOnAttack;

    public CharacterData_SO templateData; //模板数据，copy数据，防止共享数据
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    [HideInInspector] 
    public bool isCritical;//在Inspector窗口中不会看见

    void Awake()
    {
        if(templateData != null)
        {
            characterData = Instantiate(templateData);
        }    
    }

    #region Read from Data_SO
    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value;}
    }
    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }
    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }
    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }
    #endregion

    #region Character Combat
    public void TakeDamage(CharacterStats attacker,CharacterStats defener)
    {
        //如果攻击力比防御力小，那么最小造成1点伤害
        int damage = Mathf.Max( attacker.CurrentDamage() - defener.CurrentDefence,1);
        CurrentHealth = Mathf.Max(defener.CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //Updata UI
        UpdataHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //经验++
        //if (defener.CurrentHealth <= 0 && !defener.isDead)
        //    GameManager.Instance.playerStats.characterData.UpdataExp(characterData.killPoint);
        //（player的经验++放到enemy死亡判定里面了）
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamge, attackData.maxDamge);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("暴击!" + coreDamage);
        }
        return (int)coreDamage;
    }

    #endregion
}
