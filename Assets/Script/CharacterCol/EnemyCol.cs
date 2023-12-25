using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD , PATROL , CHASE ,DEAD  }
//枚举 敌人的状态 ，守卫，巡逻，追击，死亡

[RequireComponent(typeof(NavMeshAgent))]//自动添加AI代理
[RequireComponent(typeof(CharacterStats))]
public class EnemyCol : MonoBehaviour,IEndGameObserver
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator ani;
    protected GameObject attackTarget;
    protected CharacterStats characterStats;
    private Collider coll;

    private float speed; //记录原来的速度，实现追击正常速度，回去站桩的时候速度慢
    private Vector3 wayPoint; //enemy巡逻范围的随机点
    private Vector3 guardPos; // enemy初始点，站桩点，守卫点
    private float remainLookAtTime = 2f; //脱战后enemy在原地停留再回到之前的状态
    private float lastAttackTime; //enemy攻击冷却
    private float skillCoolDownTime; //技能冷却
    private Quaternion guardRotation; //记录旋转角度，保证站桩敌人回去再次看向默认方向

    [Header("Basic Settings")]
    public float sightRadius; //可视范围
    [Header("Patrol State")]
    public float patrolRange;//巡逻范围
    public bool isGuard; //是站桩敌人 还是巡逻敌人


    //配合动画的的bool
    protected bool isWalk, isChase, isFollowPlayer,isDead;
    //广播后player死亡，enemy不再执行任何操作（没有同步）
    bool playerDead = false;

    //启用,加入观察者列表(场景切换后改掉) 已在start中调用
    //void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}
    //禁用,移除观察者列表
    void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveObserver(this);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        speed = agent.speed;
        guardPos = transform.position;
        guardRotation = transform.rotation;
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();

        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        //启用,加入观察者列表(场景切换后改掉)
        GameManager.Instance.AddObserver(this);
    }
    void Update()
    {
        if(characterStats.CurrentHealth <= 0 && !isDead)
        {
            isDead = true;
            GameManager.Instance.playerStats.characterData.UpdataExp(characterStats.characterData.killPoint);
        }

        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
            skillCoolDownTime -= Time.deltaTime;
        }
    }

    //切换动画
    private void SwitchAnimation()
    {
        //将Animator的bool和脚本中设置的bool关联到一起
        ani.SetBool("Walk", isWalk);
        ani.SetBool("Chase", isChase);
        ani.SetBool("FollowPlayer", isFollowPlayer);
        ani.SetBool("Critical", characterStats.isCritical);
        ani.SetBool("Death", isDead);
    }

    //状态切换
    void SwitchStates()
    {
        if (isDead)
            enemyStates = EnemyStates.DEAD;
        //找到Player切换成CHASE状态
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD: //防守
                Enemy_GUARD();
                break;
            case EnemyStates.PATROL: //巡逻
                Enemy_PATROL();
                break;
            case EnemyStates.CHASE: //追击
                Enemy_CHASE();
                break;
            case EnemyStates.DEAD: //死亡
                Enemy_DEAD();
                break;
        }
    }

    void Enemy_DEAD()
    {
        coll.enabled = false;
        //直接关闭agent会导致StopAgent脚本报错，因为一直在更新agent所以这里把agent范围设置成0也能实现不走动
        //agent.enabled = false;
        agent.radius = 0;
        Destroy(gameObject,2f);
    }

    void Enemy_GUARD()
    {
        isChase = false;
        if(transform.position != guardPos)
        {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = guardPos;
            //SqrMagnitude 同理 Distance 性能开销不一样，这里不知道为什么Dis会报错
            if (Vector3.SqrMagnitude(guardPos - transform.position) <= 0.5)
            {
                isWalk = false;
                //归位后转向
                transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.01f);
            }
        }
    }

    void Enemy_PATROL()
    {
        isChase = false;
        agent.speed = speed * 0.6f;
        //判断是否走到了新地点，再次获取新地点
        if (Vector3.Distance(wayPoint,transform.position) <= 0.5)
        {
            isWalk = false;
            GetNewWayPoint();
        }
        else
        {
            isWalk=true;
            agent.destination = wayPoint;
        }
    }

    void Enemy_CHASE()
    {
        agent.speed = speed;
        //配合动画
        isWalk = false;
        isChase = true;


        //player脱战
        if(!FoundPlayer())
        {
            //返回上一个状态，先在原地发呆两秒钟
            isFollowPlayer = false;
            if(remainLookAtTime > 0)
            {
                agent.destination = transform.position;
                remainLookAtTime -= Time.deltaTime;
            }else if (isGuard)
            {
                enemyStates = EnemyStates.GUARD;
                remainLookAtTime = 2f;
            }
            else
            {
                enemyStates=EnemyStates.PATROL;
                remainLookAtTime = 2f;
            }
        }
        //player未脱战，追击
        else
        {
            isFollowPlayer = true;
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
        }
        //攻击
        if (TargetInSkillRange())
        {
            isFollowPlayer = false;
            agent.isStopped = true;
            if (skillCoolDownTime < 0)
            {
                skillCoolDownTime = characterStats.attackData.skillCoolDown;
                Skill_Attack();
            }
        }
        if (TargetInAttackRange())
        {
            isFollowPlayer = false;
            agent.isStopped = true;
            if(lastAttackTime < 0)
            {
                lastAttackTime = characterStats.attackData.coolDown;
                //判断暴击,随机生成一个0到1之间的数，判断是否小于criticalChance
                characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
                //执行攻击
                Attack();
            }
        }

    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform.position);
        if (TargetInAttackRange())
        {
            ani.SetTrigger("Attack");
        }
    }
    void Skill_Attack()
    {
        transform.LookAt(attackTarget.transform.position);
        //if(TargetInSkillRange())
        //{
            ani.SetTrigger("Skill");
        //}
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    //是否在攻击距离之内
    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position,transform.position) <= characterStats.attackData.attackRange;
        else 
            return false;
    }

    //DragonCol中要重写此方法，所以改成public virtual
    //是否在技能攻击距离之内
    public virtual bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange ;
        else
            return false;
    }

    //获得随机的点，让enemy在巡逻范围内巡逻
    void GetNewWayPoint()
    {
        float randomX = UnityEngine.Random.Range(-patrolRange,patrolRange);
        float randomZ = UnityEngine.Random.Range(-patrolRange,patrolRange);
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);
        //wayPoint = randomPoint;
        //BUG:简单的等于会导致随机get到不可导航的点，会一直挤进不去的点
        //SamplePosition( )在指定范围内找到导航网格上最近的点,返回一个Bool值
        //true：赋值有效点hit.positon
        //false：站在原地重新get新的点
        NavMeshHit hit; 
        wayPoint = NavMesh.SamplePosition(randomPoint,out hit,patrolRange,1)? hit.position:transform.position;
    }

    //显示enemy巡逻(Patrol)的范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    void Hit()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform) && !isDead)
        {
            if (Vector3.Distance(transform.position,attackTarget.transform.position) <= characterStats.attackData.attackRange)
            {
                var targetStats = attackTarget.GetComponent<CharacterStats>();
                targetStats.TakeDamage(characterStats, targetStats);
            }
        }
    }

    public void EndNotify()
    {
        //获胜
        //停止所有移动
        //停止Agent
        ani.SetBool("Win", true);
        playerDead = true;
        isWalk = false;
        isChase = false;
        attackTarget = null;
        ani.GetComponent<NavMeshAgent>().isStopped = true;
    }
}
