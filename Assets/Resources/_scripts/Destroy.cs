using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float lifeTime;
    public bool isLetsGo;
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        if(!isLetsGo)
        Destroy(gameObject, lifeTime);
    }
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
