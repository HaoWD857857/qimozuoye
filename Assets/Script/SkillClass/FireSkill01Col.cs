using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�˽ű�ʵ��Q���ܻ���Ŀ���
public class FireSkill01Col : MonoBehaviour
{
    //Ԥ�����ƶ��ٶ�
    private float speed = 15f;
    // Start is called before the first frame update
    void Start()
    {
        //5����Զ�����
        Destroy(gameObject, 5f);
        //�ƶ�
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
