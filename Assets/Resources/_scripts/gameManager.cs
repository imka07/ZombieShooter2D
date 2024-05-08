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


    [SerializeField] private GameObject pausePanel, lostPanel, winPanel;
    public AudioSource musicThem;
    public AudioSource click;
    public int tntCount = 0;
    public TextMeshProUGUI tntText;
    public Text cashText;
    public Toggle musicToggle;
    public bool[] weaponsToUnlock = new bool[6];
    public Image[] locks;


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
        bool musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        musicToggle.isOn = musicEnabled;
        SetMusicVolume(musicEnabled ? 1 : 0);
        currentPlayerCash = PlayerPrefs.GetInt("Cash");
    }

    private void Start()
    {
        isGameActive = true;
        UpdateCashText();
        waveSpawner.waveEnemies = 0;

        for (int i = 0; i < weaponsToUnlock.Length; i++)
        {
            if (WeaponShopManager.IsWeaponPurchased(i))
            {
                weaponsToUnlock[i] = true;
                locks[i].enabled = false;
            }
            else
            {
                weaponsToUnlock[i] = false;
                locks[i].enabled = true;
            }
        }

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

    private void ToggleMusic()
    {
        bool musicEnabled = musicToggle.isOn;
        SetMusicVolume(musicEnabled ? 1 : 0);

        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetMusicVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void Update()
    {
        ToggleMusic();
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
        PlayerPrefs.SetInt("Cash", currentPlayerCash);
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
