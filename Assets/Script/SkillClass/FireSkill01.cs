using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//此脚本实现火焰的释放
//此脚本挂载在player组件上的fire01_object

public class FireSkill01 : MonoBehaviour, ISkill
{
    public GameObject fireBallPrefab;
    public Image cooldownImage;
    public Image skillIcon; // 在 Inspector 视图中将技能图标拖拽到这个变量上

    private bool onCooldown = false; // 是否在冷却中,私有
    private float cooldownTime = 2f;  //技能CD
    private float cooldownTimer = 0f;//技能CD计时器

    private Color grayColor = new Color(0, 0, 0); // 灰色
    private Color originalColor = Color.white; // 保存原色，方便变回来

    
    // 实现 ISkill 接口的方法
    public void Activate()
    {
        if (!onCooldown)
        {
            if (fireBallPrefab != null)
            {
                
                // 实例化火球技能效果，这里假设技能效果是一个预制体
                Instantiate(fireBallPrefab, transform.position, transform.rotation);
            }
            StartCooldown();
            Invoke("ResetCooldown", cooldownTime); //  cooldownTime秒后重置 CD

        }
        else
        {
            Debug.Log("Q技能还在冷却中！");
        }
    }

    public void ResetCooldown()
    {
        onCooldown = false;
        Debug.Log("Q技能冷却完毕！");
        cooldownImage.fillAmount = 1f; // 重置 CD 图片的填充状态
        cooldownImage.color = originalColor; //重置颜色
    }

    public bool IsOnCooldown()
    {
        return onCooldown;
    }
    private void StartCooldown() //开始冷却，每次开始都让计时器等于冷却时间
    {
        onCooldown = true;
        cooldownTimer = cooldownTime; 
    }



    void Update()
    {

        // 更新 CD 图片的填充状态
        if (onCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            float fillAmount = cooldownTimer / cooldownTime; // 计算填充量

            // 更新 CD 图片的填充状态
            cooldownImage.fillAmount = fillAmount;
            cooldownImage.color = Color.Lerp(grayColor, originalColor, fillAmount);
        }
    }
    
}
