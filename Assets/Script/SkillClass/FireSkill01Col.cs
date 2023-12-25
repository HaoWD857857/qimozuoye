using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�˽ű�ʵ��Q���ܻ���Ŀ���
//Q���ܵ�damage��Ԥ��������
public class FireSkill01Col : MonoBehaviour
{
    //��ըЧ��Ԥ����
    public GameObject ExplosionPre;
    //Ԥ�����ƶ��ٶ�
    private float speed = 15f;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        //5����Զ�����
        Destroy(gameObject, 5f);
        //�ƶ�
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
    void OnTriggerEnter(Collider other)
    {
        //�ж��ǲ��ǵ���
        if (other.gameObject.CompareTag("Enemy"))
        {
            //��ײ�����ˣ���ըЧ��
            Instantiate(ExplosionPre, transform.position, Quaternion.identity);
            var otherStats = other.gameObject.GetComponent<CharacterStats>();
            otherStats.TakeDamage(damage, otherStats);
            //��������
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Fire"))
        {
            //���ᷢ��ʲô
        }else
        {
            Instantiate(ExplosionPre, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    
}
