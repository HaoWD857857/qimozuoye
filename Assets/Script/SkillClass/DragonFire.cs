using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class DragonFire : MonoBehaviour
{
    public enum FireStates { HitPlayer , HitEnemy ,HitNoting}
    private Rigidbody rb;
    private FireStates fireStates;

    [Header("Basic Settings")]
    public float force;
    public int damage;
    public GameObject target;
    private Vector3 direction;
    //��ըЧ��Ԥ����
    public GameObject ExplosionPre;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fireStates = FireStates.HitPlayer;
        FlyToTarget();
    }

    private void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerCol>().gameObject;
        //����һ����΢���ϵ���,������ֱ�ӷɹ���
        direction = (target.transform.position - transform.position + Vector3.up + Vector3.up).normalized;
        //�����
        rb.AddForce(direction * force,ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        switch(fireStates)
        {
            case FireStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");

                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, other.gameObject.GetComponent<CharacterStats>());
                    fireStates = FireStates.HitNoting;
                    Destroy(gameObject);
                    Instantiate(ExplosionPre, transform.position, Quaternion.identity);
                }
                else
                    Destroy(gameObject,2.5f);
                break;
        }
    }
}
