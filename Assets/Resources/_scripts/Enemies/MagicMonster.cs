using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMonster : ZombieAI
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
