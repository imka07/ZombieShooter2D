using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsController : MonoBehaviour
{
    [SerializeField] float targetScale = 0.07f;
    [SerializeField] float xOffset = -0.005f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput > 0) // Проверяем направление движения вправо
        {
            transform.localScale = new Vector3(targetScale, targetScale, targetScale);
            transform.localPosition = new Vector3(xOffset, transform.localPosition.y, transform.localPosition.z);
        }
        else if (horizontalInput < 0) // Проверяем направление движения влево
        {
            transform.localScale = new Vector3(-targetScale, targetScale, targetScale);
            transform.localPosition = new Vector3(-xOffset, transform.localPosition.y, transform.localPosition.z);
        }
        // Если horizontalInput == 0, мы ничего не делаем, ноги остаются в том же положении

    }
}
