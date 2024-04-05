using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{               
  
    private bool isGameActive;                  // Bool to check if the game is active.

    public int currentWave;



    public WaveSpawner waveSpawner;             // Reference the WaveSpawner.


    [SerializeField] private GameObject pausePanel, lostPanel, winPanel;
    public AudioSource musicThem;
    public AudioSource click;

    public static gameManager instance;         // Reference the GameManager.

    /// <summary>
    /// Called when the GameManager Object is Enabled.
    /// </summary>
    private void OnEnable()
    {
        waveSpawner.OnEnemyRemoved.AddListener(OnEnemyDestroyed);
    }

    /// <summary>
    /// Called when the GameManager Object is Disabled.
    /// </summary>
    private void OnDisable()
    {
        waveSpawner.OnEnemyRemoved.RemoveListener(OnEnemyDestroyed);
    }

    /// <summary>
    /// Using Awake make sure that GameManager is created as a Singleton.
    /// Awake is called before "Start" methods
    /// </summary>
    private void Awake()
    {
        // Set the Instance reference.
        instance = this;
    }

    private void Start()
    {
        // Set the isGameActive bool.
        isGameActive = true;
        // Set the Player current cash to be the Player Start cash.
        // In the beginning of the game there are no enemies set the waveSpawner.waveEnemies to reflect this.
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

    public void GameOver()
    {
        // As the game is over set the isGameActive bool to reflect this.
        isGameActive = false;
        // Activate the EndScreen object.
        lostPanel.SetActive(false);
        // Set the EndScreen data.
    }

    /// <summary>
    /// When the Player has met the requirements to win the game, the Player wins the game.
    /// Activate and set the EndScreen.
    /// </summary>
    private void GameWin()
    {
        // If the Player won the game the game is no longer active, so set the isGameActive to reflect this.
        isGameActive = false;
        // Activate the EndScreen object.
        winPanel.gameObject.SetActive(true);
        // Set the EndScreen data.
    }

    /// <summary>
    /// OnEnemyDestroyed ties into the onEnemyDestroyed event, and is called when an enemy is destroyed.
    /// </summary>
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
            GameWin();
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

    public void AddCash(int amount)
    {
        // Award the currentPlayerCash with the amount the Player is awarded with for killing an Enemy object.
        //currentPlayerCash += amount;
        //// Update the Player healthAndCashText so that the UI reflects the correct amount of available cas�h that the Player has.
        //UpdateHealthAndCashText();
        //// Invoke the onPlayerCashChanged event.
        //onPlayerCashChanged.Invoke();
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
