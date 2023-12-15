using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//此工具类是用来判断攻击时受击者是否在施法者前方
public static class ExtensionMethod 
{
    //用来比较方向的常量
    private static float dotThreshold = 0.8f;
    //需要传入一个target
    public static bool IsFacingTarget(this Transform transform,Transform target)
    {
        //两个向量的点积,量化
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);
        return dot >= dotThreshold;
    }
}
