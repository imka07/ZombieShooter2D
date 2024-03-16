using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] float targetScale = 0.07f;
    [SerializeField] float targetPositionX = -0.05f;

    [SerializeField] bool limit;
    [SerializeField] float maxAngle, minAngle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
        // Чтобы спраит с рукой следовал за курсором
    {
        //var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;

        if (mousePos.x > transform.position.x)
        {
            transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            transform.localPosition = new Vector3(targetPositionX, transform.localPosition.y, 0);
            if (limit)
            {
                angle = Mathf.Clamp(angle, -25, 35);
            }
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(targetScale, -targetScale, targetScale);
            transform.localPosition = new Vector3(-targetPositionX, transform.localPosition.y, 0);
            if (limit)
            {
                if (angle > 0) angle = Mathf.Clamp(angle, 145, 180);
                else angle = Mathf.Clamp(angle, -180, -155);
            }
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }
}

