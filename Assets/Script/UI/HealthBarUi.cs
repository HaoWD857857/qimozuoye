using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUi : MonoBehaviour
{
    public GameObject healthUIPrefab;
    public Transform barPoint;
    public bool alwaysVisible; //血条是否是长久可见
    public float visibleTime;  //可视化时间

    private float timeLeft; //剩余的可视化时间

    Image healthSlider;
    Transform UIbar;
    Transform cam;

    CharacterStats currentStats;

    void Start()
    {
        currentStats = GetComponent<CharacterStats>();
        currentStats.UpdataHealthBarOnAttack += UpdateHealthBar;
    }



    //开始的时候要生成UI
    [Obsolete]//(FindObjectsOfType()这个方法过时了)
    void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                //血条的坐标参数
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;
                //拿到第一个子物体（绿色血条）
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                //血条是否长久可见
                UIbar.gameObject.SetActive(alwaysVisible);
            }
        }
    }
    //实时更新血条
    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (UIbar == null) return;

        if (currentHealth <= 0)
            UIbar.gameObject.GetComponent<Image>().enabled = false;

        UIbar.gameObject.SetActive(true);
        timeLeft = visibleTime;

        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
    //血条跟随敌人
    void LateUpdate()
    {
        if(UIbar != null)
        {
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;

            if(timeLeft <=0 && !alwaysVisible)
                UIbar.gameObject.SetActive(false);
            else
                timeLeft -= Time.deltaTime;
        }
    }
}
