using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class MouseManager : Singleton<MouseManager> 
{
    //event Action<>:所有订阅了这个事件添加进去的方法都会被执行,需要注册 
    //eg：后面在PlayerCol中加入一个MoveTo方法： OnmouseClicked += MoveTo; 就可以在这里调用了
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;

    //public static MouseManager Instance; //将MouseManager设置为单例模式 //设计成泛型单例了，不需要这行代码
    RaycastHit hitinfo;  //获取射线的位置
    public Texture2D point, doorway, attack, target, arrow;//光标状态

    protected override void Awake()
    {
        //基于原有父类里面的函数方法，额外运行的
        base.Awake();
        //DontDestroyOnLoad(this);
    }

    //void Start()
    //{
    //    //设计成泛型单例了，不需要这行代码
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

    //光标设置
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitinfo))
        {
            //切换鼠标贴图
            switch (hitinfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto); //Vector是设置偏移
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto); //Vector是设置偏移
                    break;
            }
        }
    }

    void OnMouseControl()
    {
        if (Input.GetMouseButtonDown(1) && hitinfo.collider != null)
        {
            //点击Tag传递射线实现移动
            if (hitinfo.collider.gameObject.CompareTag("Ground"))
            {
                //一个？的意思是：如果hitinfo不为空。把position传给OnMouseClicked这个事件
                //所有订阅了这个事件添加进去的方法都会被执行
                OnMouseClicked?.Invoke(hitinfo.point);
            }
            if (hitinfo.collider.gameObject.CompareTag("Enemy"))
            {
                //在PlayerCol中去添加这个订阅
                OnEnemyClicked?.Invoke(hitinfo.collider.gameObject);
            }
        }
    }
}
