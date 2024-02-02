using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox_Fever : MonoBehaviour
{
    // #. È¹µæµÇ¾ú´ÂÁö È¹µæµÇÁö ¾Ê¾Ò´ÂÁ¦ ±¸ºÐ
    private bool bIsGet = false;



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && !bIsGet)
            {
                bIsGet = true;
                player.RecordItemCnt();
                for (int i = 0; i < 5; i++)
                {
                    Debug.Log("½ÇÇà");
                    if(player.iNowFeverGauge >= 5)
                    {
                        break;
                    }
                    player.GetFeverGauge();
                }
                
                Destroy(gameObject);
            }
        }
    }
}
