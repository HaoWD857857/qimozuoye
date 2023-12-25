using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
//�˽ű�������Rʵ�ּ��ܶ���
public class Skill03 : MonoBehaviour,ISkill
{
    public Image cooldownImage;
    public Image skillIcon; // �� Inspector ��ͼ�н�����ͼ����ק�����������

    private bool onCooldown = false; // �Ƿ�����ȴ��,˽��
    private float cooldownTime = 2f;  //����CD
    private float cooldownTimer = 0f;//����CD��ʱ��

    private Color grayColor = new Color(0, 0, 0); // ��ɫ
    private Color originalColor = Color.white; // ����ԭɫ����������

    private Animator ani;

    public void Activate()
    {
        if (!onCooldown)
        {
            ani.GetComponent<NavMeshAgent>().isStopped = true;
            ani.SetTrigger("SkillTriggerR");
            StartCooldown();
            Invoke("ResetCooldown", cooldownTime); //  cooldownTime������� CD
        }
            //Debug.Log("W���ܻ�����ȴ�У�");
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
        //Debug.Log("R������ȴ��ϣ�");
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
