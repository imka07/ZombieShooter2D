using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class point : MonoBehaviour
{
   
    public GameObject[] stuffs;
    void Start()
    {
        
        var rand = Random.Range(0, stuffs.Length);
        Instantiate(stuffs[rand], transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
