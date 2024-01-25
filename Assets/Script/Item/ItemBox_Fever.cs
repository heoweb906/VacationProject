using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox_Fever : MonoBehaviour
{
    // #. ȹ��Ǿ����� ȹ����� �ʾҴ��� ����
    private bool bIsGet = false;



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && !bIsGet)
            {
                bIsGet = true;

                for(int i = 0; i < 5; i++)
                {
                    Debug.Log("����");
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
