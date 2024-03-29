using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel, lostPanel;
    public AudioSource musicThem;
    public AudioSource click;
    public static gameManager instance;

    public int currentWave;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public void LoseController(bool active)
    {
        lostPanel.SetActive(active);
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
