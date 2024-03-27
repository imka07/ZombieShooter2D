using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set up backend code for a Scriptable object that contains the data for the waves in the game.
/// Scriptableobjects store data, they can therefore be considered "datacontainers"
/// </summary>
[CreateAssetMenu(fileName = "Wave Data", menuName = "New Wave Data")]
public class WaveData : ScriptableObject
{
    public EnemySet[] enemySets; // Allows for multiple Enemy prefabs to be set, aka allows for multiple different enemies to be spawned. 

    /// <summary>
    /// Describes individual Enemy objects and how many of them to spawn.
    /// </summary>
    [System.Serializable]
    public class EnemySet
    {
        public GameObject enemyPrefab;  // Reference the Enemy object for the given set.
        public int spawnAmount;         // How many of the referenced enemies to spawn.
        public float spawnDelay;        // How long to wait from wave start until beginning to spawn enemies. Time given in seconds.
        public float spawnRate;         // How long between individual spawns. Time given in seconds.
    }
}