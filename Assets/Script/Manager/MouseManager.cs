using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class MouseManager : Singleton<MouseManager> 
{
    //event Action<>:���ж���������¼���ӽ�ȥ�ķ������ᱻִ��,��Ҫע�� 
    //eg��������PlayerCol�м���һ��MoveTo������ OnmouseClicked += MoveTo; �Ϳ��������������
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;

    //public static MouseManager Instance; //��MouseManager����Ϊ����ģʽ //��Ƴɷ��͵����ˣ�����Ҫ���д���
    RaycastHit hitinfo;  //��ȡ���ߵ�λ��
    public Texture2D point, doorway, attack, target, arrow;//���״̬

    protected override void Awake()
    {
        //����ԭ�и�������ĺ����������������е�
        base.Awake();
        //DontDestroyOnLoad(this);
    }

    //void Start()
    //{
    //    //��Ƴɷ��͵����ˣ�����Ҫ���д���
    //    //if (Instance != null)
    //    //    Destroy(gameObject);
    //    //else
    //    //   Instance = this;
    //}

    void Update()
    {
        SetCursorTexture();
        OnMouseControl();
    }

    //�������
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitinfo))
        {
            //�л������ͼ
            switch (hitinfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto); //Vector������ƫ��
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto); //Vector������ƫ��
                    break;
            }
        }
    }

    void OnMouseControl()
    {
        if (Input.GetMouseButtonDown(1) && hitinfo.collider != null)
        {
            //���Tag��������ʵ���ƶ�
            if (hitinfo.collider.gameObject.CompareTag("Ground"))
            {
                //һ��������˼�ǣ����hitinfo��Ϊ�ա���position����OnMouseClicked����¼�
                //���ж���������¼���ӽ�ȥ�ķ������ᱻִ��
                OnMouseClicked?.Invoke(hitinfo.point);
            }
            if (hitinfo.collider.gameObject.CompareTag("Enemy"))
            {
                //��PlayerCol��ȥ����������
                OnEnemyClicked?.Invoke(hitinfo.collider.gameObject);
            }
        }
    }
}
