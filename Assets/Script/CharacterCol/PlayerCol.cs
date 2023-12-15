using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCol : MonoBehaviour
{
    public static bool IsAttacking = false;
    public static bool IsMoving = false;

    private Animator ani;
    private NavMeshAgent agent;
    private GameObject attackTarget; //Ҫ������target��ɫ��Ϣ
    private CharacterStats characterStats; //player�Ľ�ɫ��Ϣ
    private Collider coll;
    private bool isDead = false;    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();    
        ani = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();

        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        GameManager.Instance.RigisterPlayer(characterStats);
    }

   

    void Update()
    {
        isDead = characterStats.CurrentHealth <= 0;
        if (isDead)
        {
            coll.enabled = false;
            GameManager.Instance.NotifyObserver();
            //ȡ������
            MouseManager.Instance.OnMouseClicked -= MoveToTarget;
            MouseManager.Instance.OnEnemyClicked -= EventAttack;
        }

        SwitchAnimation();
        //NavAgentMove();
            if (agent.velocity.magnitude > 0)
            {
                ani.SetBool("IsMoving", true);
                IsAttacking = true;  //����˵���ڹ����൱��ռ���˹���״̬
                IsMoving = true;
            }
            else
            {
                ani.SetBool("IsMoving", false);
                IsAttacking = false; //�ͷţ���ֹ��·����
                IsMoving = false;
            }

            if (!IsMoving)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (!IsAttacking)
                    {
                        Attack_S();
                    }
                }
            }
    }

    private void SwitchAnimation()
    {
        ani.SetBool("Death", isDead);
    }

    void Attack_S()
    {
        //��ȡ��ǰ�Ķ���
        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
        if (stateInfo.IsName("Idle"))
        {
            ani.SetTrigger("Attack1");
        }
        else if(stateInfo.IsName("Attack01") )
        {
            ani.SetTrigger("Attack2");
        }else if (stateInfo.IsName("Attack02") )
        {
            ani.SetTrigger("Attack3");
        }
    }
    private void NavAgentMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //��ȡ���ָ������
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //��ײ��Ϣ
            RaycastHit hit;
            //������ײ���
            bool res = Physics.Raycast(ray, out hit);
            //���������ײ����Ϸ����
            if (res)
            {
                //��ȡ��ײ��
                Vector3 point = hit.point;
                //�ƶ�����ײ�㣬���õ�����������Ŀ��λ��
                agent.SetDestination(point);
                //Vector3 target = hit.point targetΪ���ָ���λ��
            }
        }
    }

    private void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();//moveǰֹͣЭ��(attackToTar)
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject target)
    {
        if(target != null)
        {
            attackTarget = target;
            //����Э��
            StartCoroutine(MoveToAttackTarget());
        }
    }
    IEnumerator MoveToAttackTarget()
    {
        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(attackTarget.transform.position, transform.position) >= characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            //��һ֡���õ�ʱ���ٴ��жϣ�һ�����������attackRange��while������ѭ��ʵ�ֹ���
            yield return null; 
        }
        agent.isStopped = true;
        //Attack
        characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
        Attack_S();
    }

    void Hit()
    {
        // ������ֵ�������ж��Ƿ����򹥻�Ŀ��
        float facingThreshold = 0.6f;
        if (attackTarget != null)
        {
            float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
            // ��ȡ������ָ�򹥻�Ŀ�������
            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            // ��ȡ�����������ǰ������
            Vector3 forward = transform.forward;
            // ʹ�� Dot Product ��鹥���߳����Ƿ��빥��Ŀ�����λ��һ��
            float dotProduct = Vector3.Dot(direction, forward);
        
            if(distance <= characterStats.attackData.attackRange && dotProduct >= facingThreshold)
            {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
            }
        }
    }

}
