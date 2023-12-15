using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//�˽ű�������Wʵ�ּ��ܶ���
public class FireSkill02 : MonoBehaviour, ISkill
{
    public Image cooldownImage;
    public Image skillIcon; // �� Inspector ��ͼ�н�����ͼ����ק�����������

    private bool onCooldown = false; // �Ƿ�����ȴ��,˽��
    private float cooldownTime = 3f;  //����CD
    private float cooldownTimer = 0f;//����CD��ʱ��

    private Color grayColor = new Color(0, 0, 0); // ��ɫ
    private Color originalColor = Color.white; // ����ԭɫ����������

    private Animator ani;
    void Start()
    {
        // ��ȡAnimator���
        ani = GetComponent<Animator>();
    }
    public void Activate()
    {
        if (!onCooldown)
        {
            ani.SetTrigger("SkillTriggerW");
            StartCooldown();
            Invoke("ResetCooldown", cooldownTime); //  cooldownTime������� CD
        }
        else
        {
            Debug.Log("W���ܻ�����ȴ�У�");
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
        Debug.Log("W������ȴ��ϣ�");
        cooldownImage.fillAmount = 1f; // ���� CD ͼƬ�����״̬
        cooldownImage.color = originalColor; //������ɫ
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
