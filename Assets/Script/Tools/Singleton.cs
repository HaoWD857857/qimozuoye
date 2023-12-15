using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���͵���
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }
    
    //protected,virtualֻ����̳�����Է���,��д
    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    //�Ƿ��Ѿ�����instance
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    //��ǰ��ʾ����������ٵĻ�����instanceΪnull����յ�ǰ��̬��ı���
    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
