using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
//����ű�ʵ��E�����ͷŵ�CD��ʵ�ּ���д��playercol����

public class Skill_E : MonoBehaviour, ISkill
{
    public Image cooldownImage;
    public Image skillIcon; // �� Inspector ��ͼ�н�����ͼ����ק�����������

    private bool onCooldown = false; // �Ƿ�����ȴ��,˽��
    private float cooldownTime = 10f;  //����CD
    private float cooldownTimer = 0f;//����CD��ʱ��

    private Color grayColor = new Color(0, 0, 0); // ��ɫ
    private Color originalColor = Color.white; // ����ԭɫ����������

    private Animator ani;

    public void Activate()
    {
        if (!onCooldown)
        {
            ani.GetComponent<NavMeshAgent>().isStopped = true;
            ani.SetTrigger("SkillTriggerE");
            StartCooldown();
            Invoke("ResetCooldown", cooldownTime); //  cooldownTime������� CD
        }
    }

    private void StartCooldown() //��ʼ��ȴ��ÿ�ο�ʼ���ü�ʱ��������ȴʱ��
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
        cooldownImage.fillAmount = 1f; // ���� CD ͼƬ�����״̬
        cooldownImage.color = originalColor; //������ɫ
    }

    void Start()
    {
        ani = GetComponent<Animator>();

    }

    void Update()
    {
        // ���� CD ͼƬ�����״̬
        if (onCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            float fillAmount = cooldownTimer / cooldownTime; // ���������

            // ���� CD ͼƬ�����״̬
            cooldownImage.fillAmount = fillAmount;
            cooldownImage.color = Color.Lerp(grayColor, originalColor, fillAmount);
        }
    }
}
