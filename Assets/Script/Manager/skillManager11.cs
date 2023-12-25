using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class skillManager1 : MonoBehaviour
{
    public List<ISkill> skills = new List<ISkill>(); // ������м���
    // �� Unity Inspector ��ͼ���ֶ���������ӵ� skills �б���
    public FireSkill01 fireSkill; // �� Inspector ��ͼ�н� FireSkill ʵ����ק�����������
    public FireSkill02 skill_W;  //��������
    public Skill03 skill_R;  //��������
    public Skill_E skill_E; //��������
    //public ... E���ܻ�Ѫ��������Ѫ������������Ч�����ĸ�ʤ��

    private Animator ani;//��ϼ��ܶ���
    private float QTimeless = 0;
    void Start()
    {
        ani = GetComponent<Animator>();

        // ���ֶ���ӵļ��ܼ��뵽�����б���
        if (fireSkill != null)
        {
            skills.Add(fireSkill);
        }
        if (skill_W != null)
        {
            skills.Add(skill_W);
        }
        if (skill_R != null)
        {
            skills.Add(skill_R);
        }
        if (skill_E != null)
        {
            skills.Add(skill_E);
        }
    }

    void Update()
    {
        QTimeless -= Time.deltaTime;
        //��ȡ�������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //��ײ��Ϣ
        RaycastHit hit;
        //�����ײ
        if (Physics.Raycast(ray, out hit))
        {
            //��ɫ������ײ����
            //transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));

            //��·��ʱ��ż���player�ͻ�Ʈ��
            if(PlayerCol.IsMoving == false)
            {
                
                if (Input.GetKeyDown(KeyCode.W))
                {
                    ActivateSkill(1);
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    ActivateSkill(2);
                }
            }
            if (Input.GetKeyDown(KeyCode.Q) && QTimeless<=0)
            {
                ani.GetComponent<NavMeshAgent>().isStopped = true;
                //�ȷŶ�������ʵ������
                ani.SetTrigger("SkillTriggerQ");
                //��Ϊ�ȷŶ�������ʵ��������������CD��ͬ��
                QTimeless = 3.5f;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                ActivateSkill(3);
            }

        }
    }
    //�ŵ������¼�����ȥ��
    void ActiveteSkill_Q()
    {
        ActivateSkill(0);
    }
    public void ActivateSkill(int index)
    {
        if (index >= 0 && index < skills.Count)
        {
            skills[index].Activate(); // �����Ӧ�����ļ���
        }
    }
}
