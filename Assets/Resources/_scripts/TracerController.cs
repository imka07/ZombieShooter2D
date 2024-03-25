using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyTracer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyTracer()
    {
        yield return new WaitForSeconds(0.03f);
        Destroy(gameObject);
    }
}
