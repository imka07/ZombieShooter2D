using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    public AudioSource musicThem;
    public AudioSource click;

    private void Start()
    {
        pausePanel.SetActive(false);
    }
    void OpenPause()
    {
        var player = FindObjectOfType<GunController>();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            click.Play();
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            player.CanShoot(false);
        }
    }
    private void Update()
    {
        OpenPause();
    }
    public void Click()
    {
        click.Play();
    }
    public void ContinueGame()
    {
        var player = FindObjectOfType<GunController>();
        click.Play();
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        player.CanShoot(true);
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
