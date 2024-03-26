using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planeSpawn : MonoBehaviour
{
    [SerializeField] private GameObject plane;
    private float TimerBTSpawn;
    private float StartTimerBtWSpawnl;
    public float minTime = 0.65f;
    public float decreaseTime = 0.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartTimerBtWSpawnl = Random.Range(50f, 75f);
        if (TimerBTSpawn <= 0)
        {


          
            Instantiate(plane, transform.position, Quaternion.identity);
            TimerBTSpawn = StartTimerBtWSpawnl;
            if (StartTimerBtWSpawnl > minTime)
            {
                StartTimerBtWSpawnl -= decreaseTime;
            }
        }
        else
            TimerBTSpawn -= Time.deltaTime;
    }

}
