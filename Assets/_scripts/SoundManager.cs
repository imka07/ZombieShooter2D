using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Image offMus;
    [SerializeField] Image onMus;
    bool mut = false;

    private void Start()
    {

        if (!PlayerPrefs.HasKey("mut"))
        {
            PlayerPrefs.SetInt("mut", 0);
            Load();
        }
        else
        {
            Load();
        }
        UpdateButtonIcon();
        AudioListener.pause = mut;
    }


    private void UpdateButtonIcon()
    {
        if (mut == false)
        {
            onMus.enabled = true;
            offMus.enabled = false;

        }
        else
        {
            onMus.enabled = false;
            offMus.enabled = true;
        }
    }


    public void OnButtonPress()
    {
        if (mut == false)
        {
            mut = true;
            AudioListener.pause = true;

        }
        else
        {
            mut = false;
            AudioListener.pause = false;
        }
        UpdateButtonIcon();
        Save();
    }


    public void Load()
    {
        mut = PlayerPrefs.GetInt("isKnock") == 1;
    }
    public void Save()
    {
        PlayerPrefs.SetInt("isKnock", mut ? 1 : 0);
    }
}
