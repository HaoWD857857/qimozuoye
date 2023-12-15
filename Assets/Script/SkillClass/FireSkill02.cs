using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//此脚本管理按键W实现技能动画
public class FireSkill02 : MonoBehaviour, ISkill
{
    public Image cooldownImage;
    public Image skillIcon; // 在 Inspector 视图中将技能图标拖拽到这个变量上

    private bool onCooldown = false; // 是否在冷却中,私有
    private float cooldownTime = 3f;  //技能CD
    private float cooldownTimer = 0f;//技能CD计时器

    private Color grayColor = new Color(0, 0, 0); // 灰色
    private Color originalColor = Color.white; // 保存原色，方便变回来

    private Animator ani;
    void Start()
    {
        // 获取Animator组件
        ani = GetComponent<Animator>();
    }
    public void Activate()
    {
        if (!onCooldown)
        {
            ani.SetTrigger("SkillTriggerW");
            StartCooldown();
            Invoke("ResetCooldown", cooldownTime); //  cooldownTime秒后重置 CD
        }
        else
        {
            Debug.Log("W技能还在冷却中！");
        }
    }
    private void StartCooldown() //开始冷却，每次开始都让计时器等于冷却时间
    {
        onCooldown = true;
        cooldownTimer = cooldownTime;
    }
    public bool IsOnCooldown()
    {
        return false;
    }

    public void ResetCooldown()
    {
        onCooldown = false;
        Debug.Log("W技能冷却完毕！");
        cooldownImage.fillAmount = 1f; // 重置 CD 图片的填充状态
        cooldownImage.color = originalColor; //重置颜色
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
