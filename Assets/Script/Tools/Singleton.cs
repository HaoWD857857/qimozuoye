using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//泛型单例
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }
    
    //protected,virtual只允许继承类可以访问,重写
    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    //是否已经生成instance
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    //当前的示例如果被销毁的话，把instance为null，清空当前静态类的变量
    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
