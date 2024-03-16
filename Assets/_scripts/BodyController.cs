using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] float targetScale = 0.07f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x > transform.position.x)
        {
            transform.localScale = new Vector3(targetScale, targetScale, targetScale);
        }
        else if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-targetScale, targetScale, targetScale);
        }

    }
}
