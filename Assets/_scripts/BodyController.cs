using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] float targetScale = 0.07f;
    public Joystick joystick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (joystick.Horizontal > 0)
        {
            transform.localScale = new Vector3(targetScale, targetScale, targetScale);
        }
        else if(joystick.Horizontal < 0)
        {
            transform.localScale = new Vector3(-targetScale, targetScale, targetScale);
        }
    }
}
