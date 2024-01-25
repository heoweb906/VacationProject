using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CoinFever : MonoBehaviour
{
    public IObjectPool<GameObject> FeverTime_PoolAssist_Pool { get; set; }
    // #. È¹µæµÇ¾ú´ÂÁö È¹µæµÇÁö ¾Ê¾Ò´ÂÁ¦ ±¸ºÐ
    private bool bIsGet = false;


    private void Update()
    {
        MoveForward();
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * -20f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && !bIsGet)
            {
                bIsGet = true;
                player.GetCoin();
                FeverTime_PoolAssist_Pool.Release(this.gameObject);
            }
        }
    }
}
