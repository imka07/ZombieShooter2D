using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public AudioSource click;
    public GameObject settingsPanel;

    public void OpenSettings()
    {
        click.Play();
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
    public void ToGame(int scene)
    {
        click.Play();
        SceneManager.LoadScene(scene);
    }
   
}
