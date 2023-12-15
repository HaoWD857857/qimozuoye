using UnityEngine;
using UnityEngine.AI;
//����ű����Ƶ���Carb��specter
public class Carb : EnemyCol
{
    [Header("SkillForce")]
    public float fickForce = 10;
    public void KickOff()
    {
        if (attackTarget != null)
        {
            transform.LookAt(attackTarget.transform);

            Vector3 direction = attackTarget.transform.position - transform.position;
            direction.Normalize();

            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * fickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
        }
    }
}
