using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public  InventoryManager inventory;
    
    
    private void OnTriggerStay2D(Collider2D coll)
    {

        if (coll.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {

            inventory.PickedKeyItem();
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
