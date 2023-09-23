using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsController : MonoBehaviour
{
    [SerializeField] float targetScale = 0.07f;
    [SerializeField] float xOffset = -0.005f;
    public Joystick joystick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (joystick.Horizontal >= 0)
        {
            transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            transform.localPosition = new Vector3(xOffset, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            transform.localScale = new Vector3(-targetScale, targetScale, targetScale);
            transform.localPosition = new Vector3(-xOffset, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
