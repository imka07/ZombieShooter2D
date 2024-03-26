using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DronHud : MonoBehaviour
{
    public Image lifeHud;
    private float timer;
    void Start()
    {
        lifeHud.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
