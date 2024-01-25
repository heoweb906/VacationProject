using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FeverTime_PoolAssist : MonoBehaviour
{
    public static FeverTime_PoolAssist instance;

    public int defaultCapacity = 30;  // Ǯ���� �ʱ��� ũ��
    public int maxPoolSize = 100;  // Ǯ���� �ִ��� ũ��
    public GameObject coinFever;

    public IObjectPool<GameObject> FeverTime_PoolAssist_Pool { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Init();
    }

    private void Init()
    {
        FeverTime_PoolAssist_Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
        OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);

        // �̸� ������Ʈ ���� �س���
        for (int i = 0; i < defaultCapacity; i++)
        {
            CoinFever coin = CreatePooledItem().GetComponent<CoinFever>();
            coin.FeverTime_PoolAssist_Pool.Release(coin.gameObject);
        }
    }

    // ����
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(coinFever);
        CoinFever coin = poolGo.GetComponent<CoinFever>();
        coin.FeverTime_PoolAssist_Pool = this.FeverTime_PoolAssist_Pool;


        return poolGo;
    }

    // ���
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // ��ȯ
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // ����
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }
}



// �����ÿ� ����� �ڵ�
/*
 * 
public IObjectPool<GameObject> Pool { get; set; }

 Pool.Release(this.gameObject);
  */




// ���� �ÿ� ����� �ڵ�
/*
 * 
var coin = FeverTime_PoolAssist.instance.FeverTime_PoolAssist_Pool.Get();
coin.transform.position = spawnPoint.position;

*/