using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Script allows the GameManager to spawn in waves of enemies.
/// </summary>
public class WaveSpawner : MonoBehaviour
{
    [Header("WaveSpawner Data")]
    public WaveData[] waves;                // Reference all the waves being spawned in the game.
    public int currentWave = 0;             // Keep track of what wave we have reached.
    public int remainingEnemies;            // How many enemies are left.
    public int waveEnemies;                 // How many enemies to spawn for the current wave.
    public bool isSpawning;
     // Bool to identify if the wavespawner is vurrently spawning units

    [Header("Components")]
    public Transform enemySpawnPosition;    // Where to spawn the enemies.
    public TextMeshProUGUI waveText;        // Reference UI so information about the wave can be displayed for the Player.
    public GameObject nextWaveButton;       // Reference the UI button that can start the next wave.

    [Header("Events")]
    public UnityEvent OnEnemyRemoved;       // Event called when an Enemy is removed from play.

    /// <summary>
    /// Called when the WaveSpawner Object is Enabled.
    /// </summary>
    private void OnEnable()
    {
        // Setup a listener that listens to the Enemy.OnDestroyed event/action from the Enemy script.
        ZombieAI.OnDestroyed += OnEnemyDestroyed;
    }

    /// <summary>
    /// Called when the WaveSpawner Object is Disabled.
    /// </summary>
    private void OnDisable()
    {
        // Remove a listener that listens to the Enemy.OnDestroyed event/action from the Enemy script.
        ZombieAI.OnDestroyed -= OnEnemyDestroyed;
    }

    public void SpawnNextWave()
    {
        // Increment the currentWave counter.
        currentWave++;

        // Chack that there is a new wave to spawn aka that we have not reached the games final wave.
        if (currentWave - 1 == waves.Length)
        {
            return;
        }

        // Update the wave text.
        waveText.text = $"Wave: {currentWave}";

        // Start the SpawnWave Corutine.
        StartCoroutine(SpawnWave());
    }

    // Setup a Corutine that can spawn waves.
    IEnumerator SpawnWave()
    {
        // First disable the NextWaveButton.
        nextWaveButton.SetActive(false);
        // Get the data for the wave to be spawned.
        WaveData wave = waves[currentWave - 1];

        foreach (var enemyset in wave.enemySets)
        {
            waveEnemies += enemyset.spawnAmount;
        }

        // Loop through the waves's enemysets
        for (int x = 0; x < wave.enemySets.Length; x++)
        {
            // Pause the Corutine for the given spawnDelay, in the current enemyset.
            yield return new WaitForSeconds(wave.enemySets[x].spawnDelay);

            // Spawn a enemy for as many times as is given in enemyset.
            for (int y = 0; y < wave.enemySets[x].spawnAmount; y++)
            {
                // Spawn the Enemy given in the enemyset.
                SpawnEnemy(wave.enemySets[x].enemyPrefab);
                // Wait for the given spawnRate set in the current enemyset.
                yield return new WaitForSeconds(wave.enemySets[x].spawnRate);
            }
        }
    }

    /// <summary>
    /// Spawn a given Enemy object prefab into the Scene.
    /// </summary>
    /// <param name="enemyToSpawn">A reference for the Enemy prefab to spawn into the Scene.</param>
    private void SpawnEnemy(GameObject enemyToSpawn)
    {
        // Instantiate an Enemy object and maintain a reference to the prefab.
        GameObject enemy = Instantiate(enemyToSpawn, enemySpawnPosition.position, Quaternion.identity);
        ZombieAI zombiAIComponent = enemy.GetComponent<ZombieAI>();
        enemy.transform.position = new Vector3(enemySpawnPosition.position.x, enemySpawnPosition.position.y + zombiAIComponent.heightAbove, 0);
        remainingEnemies++; // Increment the remainingEnemies since a new Enemy has been instantiated.
    }

    /// <summary>
    /// Remove an Enemy from the remainingEnemies integer when an Enemy is killed.
    /// </summary>
    public void OnEnemyDestroyed()
    {
        // Subtract from remainingEnemies.
        remainingEnemies--;
        // Since an enemy died subtract from the waveSpawner.waveEnemies.
        waveEnemies--;
        // Check if all Enemies has been killed.
        if (remainingEnemies == 0 && waveEnemies == 0)
        {
            // Re-enable the nextWaveButton.
            nextWaveButton.SetActive(true);
        }
        OnEnemyRemoved?.Invoke();
    }
}
