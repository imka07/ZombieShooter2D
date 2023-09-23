using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class killsCounter : MonoBehaviour
{
    public static killsCounter instanse;
    public TextMeshProUGUI killsText;
    public float killsCount;
    private void Awake()
    {
        if(instanse == null)
        {
            instanse = this;
        }
        else
        {
            Destroy(gameObject);
        } 
            
    }

    // Update is called once per frame
    public void KillUpdate()
    {
        killsText.text = killsCount.ToString();
    }
}
