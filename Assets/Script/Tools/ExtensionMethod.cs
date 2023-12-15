using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˹������������жϹ���ʱ�ܻ����Ƿ���ʩ����ǰ��
public static class ExtensionMethod 
{
    //�����ȽϷ���ĳ���
    private static float dotThreshold = 0.8f;
    //��Ҫ����һ��target
    public static bool IsFacingTarget(this Transform transform,Transform target)
    {
        //���������ĵ��,����
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);
        return dot >= dotThreshold;
    }
}
