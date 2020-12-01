using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath;


    private void Update()
    {
        if(aiPath.desiredVelocity.x>=0.01f)//Right
        {
            transform.localScale = new Vector2(-1, 1);
        } else if(aiPath.desiredVelocity.x<=-0.01f)//Left
        {
            transform.localScale = new Vector2(1, 1);
        }
    }
}
