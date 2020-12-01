using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    [SerializeField] private float CurrentTime;
    [SerializeField] private float StartingTime;

    void Start()
    {
        CurrentTime = StartingTime;
    }

    
    void Update()
    {
        CurrentTime -= 1 * Time.deltaTime;
       
        TurnOffSmoke();
    }

    private void TurnOffSmoke()
    {
        if (CurrentTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
