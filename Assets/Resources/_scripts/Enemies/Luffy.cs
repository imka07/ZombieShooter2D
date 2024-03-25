using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luffy : EnemyBasic
{

    
    // Start is called before the first frame update
    void Start()
    {
        Init();
        StartCoroutine(EnemyBehaviour());
    }

    // Update is called once per frame
    void Update()
    {
    }

    
}
