using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleZombie : ZombieAI
{
   
    void Start()
    {
        Init();
    }

    void Update()
    {
        if (gameManager.instance.isGameActive)
        {
            Attack();
            MoveToBunker();
        }
    }
}
