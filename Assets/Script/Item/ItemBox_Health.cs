using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox_Health : MonoBehaviour
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

                player.ActivateHealth();



                Destroy(gameObject);
            }
        }
    }
}
