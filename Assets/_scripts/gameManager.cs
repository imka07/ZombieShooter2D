using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public AudioSource musicThem;
    public AudioSource click;
    private Test player;
    private void Start()
    {
        player = FindObjectOfType<Test>();
        pausePanel.SetActive(false);
    }
    public void OpenPause()
    {
        click.Play();
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    public void Click()
    {
        click.Play();
    }
    public void ContinueGame()
    {
        click.Play();
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    public void ToMenu()
    {
     
        click.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
      
        click.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

}
