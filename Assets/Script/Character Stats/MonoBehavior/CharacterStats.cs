using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //event Action<>:���ж���������¼���ӽ�ȥ�ķ������ᱻִ��,��Ҫע��(��HealthBarUI)
    public event Action<int, int> UpdataHealthBarOnAttack;

    public CharacterData_SO templateData; //ģ�����ݣ�copy���ݣ���ֹ��������
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    [HideInInspector] 
    public bool isCritical;//��Inspector�����в��ῴ��

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
        //����������ȷ�����С����ô��С���1���˺�
        int damage = Mathf.Max( attacker.CurrentDamage() - defener.CurrentDefence,1);
        CurrentHealth = Mathf.Max(defener.CurrentHealth - damage, 0);

        if (attacker.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //Updata UI
        UpdataHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //����++
        //if (defener.CurrentHealth <= 0 && !defener.isDead)
        //    GameManager.Instance.playerStats.characterData.UpdataExp(characterData.killPoint);
        //��player�ľ���++�ŵ�enemy�����ж������ˣ�
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamge, attackData.maxDamge);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            Debug.Log("����!" + coreDamage);
        }
        return (int)coreDamage;
    }

    #endregion
}
