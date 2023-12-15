using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;
    //�б������м�����IEndGameObserver�ӿڵĹ۲���
    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    //�۲���ģʽ����ע�ᣬ��player�����ɵ�ʱ�����GameManager����playerStats
    public void RigisterPlayer(CharacterStats player)
    {
        playerStats = player;
    }

    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    //�㲥
    public void NotifyObserver()
    {
        foreach (var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
}
