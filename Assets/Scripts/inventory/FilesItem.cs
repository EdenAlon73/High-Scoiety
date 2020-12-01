using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilesItem : MonoBehaviour
{
    public InventoryManager inventory;


    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {

            inventory.PickedFilesItem();
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    
}
