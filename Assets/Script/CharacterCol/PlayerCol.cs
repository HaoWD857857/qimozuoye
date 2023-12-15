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
    private GameObject attackTarget; //要攻击的target角色信息
    private CharacterStats characterStats; //player的角色信息
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
            //取消订阅
            MouseManager.Instance.OnMouseClicked -= MoveToTarget;
            MouseManager.Instance.OnEnemyClicked -= EventAttack;
        }

        SwitchAnimation();
        //NavAgentMove();
            if (agent.velocity.magnitude > 0)
            {
                ani.SetBool("IsMoving", true);
                IsAttacking = true;  //这里说正在攻击相当于占用了攻击状态
                IsMoving = true;
            }
            else
            {
                ani.SetBool("IsMoving", false);
                IsAttacking = false; //释放，防止走路攻击
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
        //获取当前的动画
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
            //获取鼠标指针射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //碰撞信息
            RaycastHit hit;
            //射线碰撞检测
            bool res = Physics.Raycast(ray, out hit);
            //如果射线碰撞到游戏物体
            if (res)
            {
                //获取碰撞点
                Vector3 point = hit.point;
                //移动到碰撞点，设置导航网格代理的目标位置
                agent.SetDestination(point);
                //Vector3 target = hit.point target为鼠标指针的位置
            }
        }
    }

    private void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();//move前停止协程(attackToTar)
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject target)
    {
        if(target != null)
        {
            attackTarget = target;
            //调用协程
            StartCoroutine(MoveToAttackTarget());
        }
    }
    IEnumerator MoveToAttackTarget()
    {
        transform.LookAt(attackTarget.transform);
        while (Vector3.Distance(attackTarget.transform.position, transform.position) >= characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            //下一帧调用的时候再次判断，一旦不满足大于attackRange，while就跳出循环实现攻击
            yield return null; 
        }
        agent.isStopped = true;
        //Attack
        characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
        Attack_S();
    }

    void Hit()
    {
        // 面向阈值，用于判断是否面向攻击目标
        float facingThreshold = 0.6f;
        if (attackTarget != null)
        {
            float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
            // 获取攻击者指向攻击目标的向量
            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            // 获取攻击者自身的前方向量
            Vector3 forward = transform.forward;
            // 使用 Dot Product 检查攻击者朝向是否与攻击目标相对位置一致
            float dotProduct = Vector3.Dot(direction, forward);
        
            if(distance <= characterStats.attackData.attackRange && dotProduct >= facingThreshold)
            {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
            }
        }
    }

}
