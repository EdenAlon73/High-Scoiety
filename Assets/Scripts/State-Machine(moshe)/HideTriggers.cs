using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTriggers : MonoBehaviour
{
    [SerializeField]
    EnemyAI[] enemyAI;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && enemyAI[enemyAI.Length-1].isSmokeOut)
        {
            foreach(var enemyAI in enemyAI)
            enemyAI.seePlayer = false;
        }
    }
}
