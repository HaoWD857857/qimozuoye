using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUi : MonoBehaviour
{
    public GameObject healthUIPrefab;
    public Transform barPoint;
    public bool alwaysVisible; //Ѫ���Ƿ��ǳ��ÿɼ�
    public float visibleTime;  //���ӻ�ʱ��

    private float timeLeft; //ʣ��Ŀ��ӻ�ʱ��

    Image healthSlider;
    Transform UIbar;
    Transform cam;

    CharacterStats currentStats;

    void Start()
    {
        currentStats = GetComponent<CharacterStats>();
        currentStats.UpdataHealthBarOnAttack += UpdateHealthBar;
    }



    //��ʼ��ʱ��Ҫ����UI
    [Obsolete]//(FindObjectsOfType()���������ʱ��)
    void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                //Ѫ�����������
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;
                //�õ���һ�������壨��ɫѪ����
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                //Ѫ���Ƿ񳤾ÿɼ�
                UIbar.gameObject.SetActive(alwaysVisible);
            }
        }
    }
    //ʵʱ����Ѫ��
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
    //Ѫ���������
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
