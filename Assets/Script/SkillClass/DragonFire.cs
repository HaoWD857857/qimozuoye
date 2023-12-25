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
    //爆炸效果预设体
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
        //给他一个稍微向上的力,不至于直接飞过来
        direction = (target.transform.position - transform.position + Vector3.up + Vector3.up).normalized;
        //冲击力
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
