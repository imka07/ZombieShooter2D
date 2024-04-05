using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel, lostPanel, winPanel;
    public AudioSource musicThem;
    public AudioSource click;
    public static gameManager instance;

    public int currentWave;
    public WaveSpawner waveSpawner;
    public bool isGameActive;


    private void OnEnable()
    {
        waveSpawner.OnEnemyRemoved.AddListener(OnEnemyDestroyed);
    }

    // Функция для обработки нажатия на кнопку и изменения приоритета у выбранного здания

    // Метод для выбора здания (замените этот метод на ваш способ выбора здания)

    /// <summary>
    /// Called when the GameManager Object is Disabled.
    /// </summary>
    private void OnDisable()
    {
        waveSpawner.OnEnemyRemoved.RemoveListener(OnEnemyDestroyed);
    }

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
        isGameActive = true;
        waveSpawner.waveEnemies = 0;
    }

    void OpenPause()
    {
        var player = FindObjectOfType<GunController>();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            click.Play();
            pausePanel.SetActive(true);
            player.CanShoot(false);
        }
    }


    public void LoseController(bool active)
    {
        // As the game is over set the isGameActive bool to reflect this.
        isGameActive = false;
        // Activate the EndScreen object.
        lostPanel.SetActive(active);
        // Set the EndScreen data.
    }

    public void WinController()
    {
        isGameActive = false;
        winPanel.SetActive(true);
    }

    public void OnEnemyDestroyed()
    {
        if (!isGameActive)
        {
            // If the game is not active don't do anything when an Enemy is destroyed.
            return;
        }
        // Check if there are enemies left AND that the Player is on the games last wave.
        if (waveSpawner.remainingEnemies == 0 && waveSpawner.currentWave == waveSpawner.waves.Length && waveSpawner.waveEnemies == 0)
        {
            // Call the win game method, when the Player has killed all enemies and the Player has reached the final wave of the Game.
            WinController();
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
        pausePanel.SetActive(false);
        player.CanShoot(true);
    }

    public void ToMenu()
    {
        click.Play();
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        click.Play();
        SceneManager.LoadScene(1);
    }

}
