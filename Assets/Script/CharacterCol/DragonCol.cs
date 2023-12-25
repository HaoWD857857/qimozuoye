using UnityEngine;
using UnityEngine.AI;
//这个脚本控制敌人Carb和Dragon
public class Dragon : EnemyCol
{
    [Header("SkillForce")]
    public float fickForce = 20;

    public GameObject DragonFirePre;
    public Transform Point;

    public void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform) && !isDead)
        {
                var targetStats = attackTarget.GetComponent<CharacterStats>();

                Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
                //direction.Normalize();

                targetStats.GetComponent<NavMeshAgent>().isStopped = true;
                targetStats.GetComponent<NavMeshAgent>().velocity = direction * fickForce;
                targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    public void ThrowFire()
    {
        if(attackTarget != null)
        {
            var fire = Instantiate(DragonFirePre, Point.position, Quaternion.identity);
            fire.GetComponent<DragonFire>().target = attackTarget;
        }
    }
}
