using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//�˽ű�ʵ�ֻ�����ͷ�
//�˽ű�������player����ϵ�fire01_object

public class FireSkill01 : MonoBehaviour, ISkill
{
    public GameObject fireBallPrefab;
    public Image cooldownImage;
    public Image skillIcon; // �� Inspector ��ͼ�н�����ͼ����ק�����������

    private bool onCooldown = false; // �Ƿ�����ȴ��,˽��
    private float cooldownTime = 2f;  //����CD
    private float cooldownTimer = 0f;//����CD��ʱ��

    private Color grayColor = new Color(0, 0, 0); // ��ɫ
    private Color originalColor = Color.white; // ����ԭɫ����������

    
    // ʵ�� ISkill �ӿڵķ���
    public void Activate()
    {
        if (!onCooldown)
        {
            if (fireBallPrefab != null)
            {
                
                // ʵ����������Ч����������輼��Ч����һ��Ԥ����
                Instantiate(fireBallPrefab, transform.position, transform.rotation);
            }
            StartCooldown();
            Invoke("ResetCooldown", cooldownTime); //  cooldownTime������� CD

        }
        else
        {
            Debug.Log("Q���ܻ�����ȴ�У�");
        }
    }

    public void ResetCooldown()
    {
        onCooldown = false;
        Debug.Log("Q������ȴ��ϣ�");
        cooldownImage.fillAmount = 1f; // ���� CD ͼƬ�����״̬
        cooldownImage.color = originalColor; //������ɫ
    }

    public bool IsOnCooldown()
    {
        return onCooldown;
    }
    private void StartCooldown() //��ʼ��ȴ��ÿ�ο�ʼ���ü�ʱ��������ȴʱ��
    {
        onCooldown = true;
        cooldownTimer = cooldownTime; 
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
