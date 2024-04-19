using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mute : MonoBehaviour
{

    public Sprite mute, unmute;
    public int isMute;
    void Start()
    {
        isMute = PlayerPrefs.GetInt("isMute");
        if (isMute == 1)
        {
            GetComponent<Image>().sprite = mute;
            AudioListener.volume = 0;
        }
    }
    public void OnMouseDown()
    {
        if (isMute == 1)
        {
            GetComponent<Image>().sprite = unmute;
            AudioListener.volume = 1;
            isMute = 0;
        }
        else if (isMute == 0)
        {
            GetComponent<Image>().sprite = mute;
            AudioListener.volume = 0;
            isMute = 1;
        }
        PlayerPrefs.SetInt("isMute", isMute);
    }
}