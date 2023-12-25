using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//此脚本实现Q技能火焰的控制
//Q技能的damage在预制体上面
public class FireSkill01Col : MonoBehaviour
{
    //爆炸效果预设体
    public GameObject ExplosionPre;
    //预制体移动速度
    private float speed = 15f;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        //5秒后自动销毁
        Destroy(gameObject, 5f);
        //移动
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
    void OnTriggerEnter(Collider other)
    {
        //判断是不是敌人
        if (other.gameObject.CompareTag("Enemy"))
        {
            //碰撞到敌人，爆炸效果
            Instantiate(ExplosionPre, transform.position, Quaternion.identity);
            var otherStats = other.gameObject.GetComponent<CharacterStats>();
            otherStats.TakeDamage(damage, otherStats);
            //销毁自身
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Fire"))
        {
            //不会发生什么
        }else
        {
            Instantiate(ExplosionPre, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    
}
