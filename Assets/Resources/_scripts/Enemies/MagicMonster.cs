using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMonster : ZombieAI
{
    Base bunkerController;
    void Start()
    {
        Init();
        bunkerController = FindObjectOfType<Base>();
    }

    void Update()
    {
        float distanceToBuker = Vector2.Distance(transform.position, bunkerController.transform.position);

        if (gameManager.instance.isGameActive)
        {
            if (distance < distanceToBuker + 9.601265f)
            {
                MoveToBunker();
            }
        }
    }
}
