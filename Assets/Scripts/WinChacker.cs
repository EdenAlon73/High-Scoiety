using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChacker : MonoBehaviour
{
    public bool PlayerHasFiles = false;
    [SerializeField] private GameObject Player;
    [SerializeField] private WinManue winManue;

    public void WinConditionOn()
    {
        PlayerHasFiles = true;
        Debug.Log(PlayerHasFiles);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("PlayerInTriger");

        if (collision.gameObject.tag == "Player" && PlayerHasFiles == true)
        {
            winManue.WinGame();
            Debug.Log("WON");
        }
    }
}
