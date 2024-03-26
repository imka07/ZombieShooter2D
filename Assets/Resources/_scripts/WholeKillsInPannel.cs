using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WholeKillsInPannel : MonoBehaviour
{
    public Text killsText;
    private float killsInPannel;
    private void Awake()
    {
        killsInPannel = PlayerPrefs.GetFloat("kills");
    }

    // Update is called once per frame
    void Update()
    {
        killsInPannel = killsCounter.instanse.killsCount;
        killsText.text = killsInPannel.ToString();
    }
}
