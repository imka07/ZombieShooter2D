using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public GameObject[] luffy;
    void Start()
    {
        int rand = Random.Range(0, luffy.Length);
        Instantiate(luffy[rand], transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
