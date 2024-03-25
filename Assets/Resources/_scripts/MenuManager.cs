using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public AudioSource click;
    public GameObject settingsPanel;
    [SerializeField] private GameObject radio;
    void Start()
    {
       
        settingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OffNight()
    {

      
        click.Play();
        Test.DarkNumber = 2;
        PlayerPrefs.SetInt("darkness", Test.DarkNumber);
    }

    public void OnNight()
    {
       
      
        click.Play();
        Test.DarkNumber = 1;
        PlayerPrefs.SetInt("darkness", Test.DarkNumber);
    }
    public void OpenSettings()
    {
        radio.SetActive(false);
        click.Play();
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        radio.SetActive(true);
        settingsPanel.SetActive(false);
    }
    public void ToGame()
    {
     
        click.Play();
        SceneManager.LoadScene(1);
    }
   
}
