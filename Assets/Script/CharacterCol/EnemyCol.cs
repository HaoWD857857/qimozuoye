using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD , PATROL , CHASE ,DEAD  }
//ö�� ���˵�״̬ ��������Ѳ�ߣ�׷��������

[RequireComponent(typeof(NavMeshAgent))]//�Զ����AI����
[RequireComponent(typeof(CharacterStats))]
public class EnemyCol : MonoBehaviour,IEndGameObserver
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator ani;
    protected GameObject attackTarget;
    protected CharacterStats characterStats;
    private Collider coll;

    private float speed; //��¼ԭ�����ٶȣ�ʵ��׷�������ٶȣ���ȥվ׮��ʱ���ٶ���
    private Vector3 wayPoint; //enemyѲ�߷�Χ�������
    private Vector3 guardPos; // enemy��ʼ�㣬վ׮�㣬������
    private float remainLookAtTime = 2f; //��ս��enemy��ԭ��ͣ���ٻص�֮ǰ��״̬
    private float lastAttackTime; //enemy������ȴ
    private float skillCoolDownTime; //������ȴ
    private Quaternion guardRotation; //��¼��ת�Ƕȣ���֤վ׮���˻�ȥ�ٴο���Ĭ�Ϸ���

    [Header("Basic Settings")]
    public float sightRadius; //���ӷ�Χ
    [Header("Patrol State")]
    public float patrolRange;//Ѳ�߷�Χ
    public bool isGuard; //��վ׮���� ����Ѳ�ߵ���


    //��϶����ĵ�bool
    protected bool isWalk, isChase, isFollowPlayer,isDead;
    //�㲥��player������enemy����ִ���κβ�����û��ͬ����
    bool playerDead = false;

    //����,����۲����б�(�����л���ĵ�) ����start�е���
    //void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}
    //����,�Ƴ��۲����б�
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
        //����,����۲����б�(�����л���ĵ�)
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

    //�л�����
    private void SwitchAnimation()
    {
        //��Animator��bool�ͽű������õ�bool������һ��
        ani.SetBool("Walk", isWalk);
        ani.SetBool("Chase", isChase);
        ani.SetBool("FollowPlayer", isFollowPlayer);
        ani.SetBool("Critical", characterStats.isCritical);
        ani.SetBool("Death", isDead);
    }

    //״̬�л�
    void SwitchStates()
    {
        if (isDead)
            enemyStates = EnemyStates.DEAD;
        //�ҵ�Player�л���CHASE״̬
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
        }
        switch (enemyStates)
        {
            case EnemyStates.GUARD: //����
                Enemy_GUARD();
                break;
            case EnemyStates.PATROL: //Ѳ��
                Enemy_PATROL();
                break;
            case EnemyStates.CHASE: //׷��
                Enemy_CHASE();
                break;
            case EnemyStates.DEAD: //����
                Enemy_DEAD();
                break;
        }
    }

    void Enemy_DEAD()
    {
        coll.enabled = false;
        //ֱ�ӹر�agent�ᵼ��StopAgent�ű�������Ϊһֱ�ڸ���agent���������agent��Χ���ó�0Ҳ��ʵ�ֲ��߶�
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
            //SqrMagnitude ͬ�� Distance ���ܿ�����һ�������ﲻ֪��ΪʲôDis�ᱨ��
            if (Vector3.SqrMagnitude(guardPos - transform.position) <= 0.5)
            {
                isWalk = false;
                //��λ��ת��
                transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.01f);
            }
        }
    }

    void Enemy_PATROL()
    {
        isChase = false;
        agent.speed = speed * 0.6f;
        //�ж��Ƿ��ߵ����µص㣬�ٴλ�ȡ�µص�
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
        //��϶���
        isWalk = false;
        isChase = true;


        //player��ս
        if(!FoundPlayer())
        {
            //������һ��״̬������ԭ�ط���������
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
        //playerδ��ս��׷��
        else
        {
            isFollowPlayer = true;
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
        }
        //����
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
                //�жϱ���,�������һ��0��1֮��������ж��Ƿ�С��criticalChance
                characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData.criticalChance;
                //ִ�й���
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

    //�Ƿ��ڹ�������֮��
    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position,transform.position) <= characterStats.attackData.attackRange;
        else 
            return false;
    }

    //DragonCol��Ҫ��д�˷��������Ըĳ�public virtual
    //�Ƿ��ڼ��ܹ�������֮��
    public virtual bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange ;
        else
            return false;
    }

    //�������ĵ㣬��enemy��Ѳ�߷�Χ��Ѳ��
    void GetNewWayPoint()
    {
        float randomX = UnityEngine.Random.Range(-patrolRange,patrolRange);
        float randomZ = UnityEngine.Random.Range(-patrolRange,patrolRange);
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);
        //wayPoint = randomPoint;
        //BUG:�򵥵ĵ��ڻᵼ�����get�����ɵ����ĵ㣬��һֱ������ȥ�ĵ�
        //SamplePosition( )��ָ����Χ���ҵ���������������ĵ�,����һ��Boolֵ
        //true����ֵ��Ч��hit.positon
        //false��վ��ԭ������get�µĵ�
        NavMeshHit hit; 
        wayPoint = NavMesh.SamplePosition(randomPoint,out hit,patrolRange,1)? hit.position:transform.position;
    }

    //��ʾenemyѲ��(Patrol)�ķ�Χ
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
        //��ʤ
        //ֹͣ�����ƶ�
        //ֹͣAgent
        ani.SetBool("Win", true);
        playerDead = true;
        isWalk = false;
        isChase = false;
        attackTarget = null;
        ani.GetComponent<NavMeshAgent>().isStopped = true;
    }
}
