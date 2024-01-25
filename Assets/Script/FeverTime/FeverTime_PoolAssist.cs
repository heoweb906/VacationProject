using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FeverTime_PoolAssist : MonoBehaviour
{
    public static FeverTime_PoolAssist instance;

    public int defaultCapacity = 30;  // 풀링의 초기의 크기
    public int maxPoolSize = 100;  // 풀링의 최대희 크기
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

        // 미리 오브젝트 생성 해놓기
        for (int i = 0; i < defaultCapacity; i++)
        {
            CoinFever coin = CreatePooledItem().GetComponent<CoinFever>();
            coin.FeverTime_PoolAssist_Pool.Release(coin.gameObject);
        }
    }

    // 생성
    private GameObject CreatePooledItem()
    {
        GameObject poolGo = Instantiate(coinFever);
        CoinFever coin = poolGo.GetComponent<CoinFever>();
        coin.FeverTime_PoolAssist_Pool = this.FeverTime_PoolAssist_Pool;


        return poolGo;
    }

    // 사용
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // 반환
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }
}



// 삭제시에 사용할 코드
/*
 * 
public IObjectPool<GameObject> Pool { get; set; }

 Pool.Release(this.gameObject);
  */




// 생성 시에 사용할 코드
/*
 * 
var coin = FeverTime_PoolAssist.instance.FeverTime_PoolAssist_Pool.Get();
coin.transform.position = spawnPoint.position;

*/