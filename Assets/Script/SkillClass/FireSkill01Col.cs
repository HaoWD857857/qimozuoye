using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//此脚本实现Q技能火焰的控制
public class FireSkill01Col : MonoBehaviour
{
    //预制体移动速度
    private float speed = 15f;
    // Start is called before the first frame update
    void Start()
    {
        //5秒后自动销毁
        Destroy(gameObject, 5f);
        //移动
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
