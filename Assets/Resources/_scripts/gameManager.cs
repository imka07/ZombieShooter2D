using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class gameManager : MonoBehaviour
{               
  
    public bool isGameActive;                 
    public int currentWave;
    public WaveSpawner waveSpawner;

    [Header("GameData")]           
    public int currentPlayerCash;               
    public int playerStartCash;        


    [SerializeField] private GameObject pausePanel, lostPanel, winPanel;
    public AudioSource musicThem;
    public AudioSource click;
    public int tntCount = 0;
    public TextMeshProUGUI tntText;
    public Text cashText;
    public Toggle musicToggle;

    public UnityEvent onPlayerCashChanged;

    public static gameManager instance;        

    private void OnEnable()
    {
        waveSpawner.OnEnemyRemoved.AddListener(OnEnemyDestroyed);
    }

    private void OnDisable()
    {
        waveSpawner.OnEnemyRemoved.RemoveListener(OnEnemyDestroyed);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isGameActive = true;
        currentPlayerCash = playerStartCash;
        UpdateCashText();
        waveSpawner.waveEnemies = 0;
    }

    private void UpdateCashText()
    {
        cashText.text = currentPlayerCash.ToString();
    }

    void OpenPause()
    {
        var player = FindObjectOfType<GunController>();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            click.Play();
            pausePanel.SetActive(true);
            player.CanShoot(false);
            isGameActive = false;
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        lostPanel.gameObject.SetActive(true);
    }

    private void GameWin()
    {
        isGameActive = false;
        winPanel.gameObject.SetActive(true);
    }

    public void OnEnemyDestroyed()
    {
        if (!isGameActive)
        {
            return;
        }
        if (waveSpawner.remainingEnemies == 0 && waveSpawner.currentWave == waveSpawner.waves.Length && waveSpawner.waveEnemies == 0)
        {
            GameWin();
        }
    }

    public void ToggleMusic()
    {
        if (musicToggle.isOn)
        {
            // Включаем музыку
            musicThem.Play();
        }
        else
        {
            // Выключаем музыку
            musicThem.Stop();
        }

    }

    private void Update()
    {
        tntText.text = tntCount.ToString();
        OpenPause();
    }

    public void Click()
    {
        click.Play();
    }

    public void AddCash(int amount)
    {
        currentPlayerCash += amount;
        UpdateCashText();
        onPlayerCashChanged.Invoke();
    }

    public void TakeCash(int amount)
    {
        currentPlayerCash -= amount;
        UpdateCashText();

        onPlayerCashChanged.Invoke();
    }

    public void ContinueGame()
    {
        var player = FindObjectOfType<GunController>();
        click.Play();
        pausePanel.SetActive(false);
        player.CanShoot(true);
        isGameActive = true;
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
