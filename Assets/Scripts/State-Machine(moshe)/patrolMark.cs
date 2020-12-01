using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrolMark : MonoBehaviour
{

    [SerializeField]
    EnemyAI[] enemyAI;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            foreach (var enemyAI in enemyAI)
                if (!enemyAI.nextPatrolPoint)
                {
                    
                    enemyAI.nextPatrolPoint = true;
                }
                else enemyAI.nextPatrolPoint = false;
        }
    }
}
