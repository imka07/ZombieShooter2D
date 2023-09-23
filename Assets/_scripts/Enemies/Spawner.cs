using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] pointsOfLuffy;
    [SerializeField] private GameObject[] pointsOfZombie;
    [SerializeField] private GameObject[] pointsOfFly;
    [SerializeField] private GameObject pointsOfSpearZombie;
    [SerializeField] private GameObject[] pointsOfBigZombie;
    [SerializeField] private GameObject spawnZombiesspear;
    [SerializeField] private GameObject spawnFly;
    [SerializeField] private GameObject[] spawnDropZombie;
    private float timeBtSpawnBigZombie;
    private float StartIntervalBtSpawnBigZombie;
    private float timeBtSpawnSpearZombie;
    private float StartIntervalBtSpawnSpearZombie;
    private float timeBtSpawnFly;
    private float StartIntervalBtSpawnFly;
    private float timeBtSpawnluffy;
    private float StartIntervalBtluffy;
    [SerializeField] private float timer;
    private float waves;
    private float TimerBTSpawn;
    public Text waveText;
    private float waveCounts;
    [SerializeField] private float StartTimerBtWSpawnl;
    private Coroutine time;
    [SerializeField] private Image zombieWaveFlBr;
    public float minTime = 0.65f;
    public float decreaseTime = 0.5f;
    public GameObject plane;
    public GameObject breakDown, zombieOn;
    //public float decrezeTime;
    //public float Mintime;
    void Start()
    {
        StartSettings();
    }
    private void StartSettings()
    {
        zombieOn.SetActive(true);
        breakDown.SetActive(false);
        plane.SetActive(false);
        StartIntervalBtluffy = 10f;
        StartIntervalBtSpawnFly = 15f;
        StartIntervalBtSpawnSpearZombie = 10f;
        StartIntervalBtSpawnBigZombie = 24f;
        waveCounts = 0;
        StartSpawn();
        zombieWaveFlBr.fillAmount = 1;
    }
    private void StartSpawn()
    {
        if (time != null) StopCoroutine(time);
        time = StartCoroutine(SpawnerOfMobs());
    }
    // Update is called once per frame
    private void Update()
    {
        SpawnCoontroller();
        
    }
    private void SpawnCoontroller()
    {
        waveText.text = waveCounts.ToString();
        if (waves == 10)
        {
            StartTimerBtWSpawnl = 3.5f;
            waves = 0;
        }
        if (waves <= 5)
        {

            plane.SetActive(true);
            FlySpawn();
            ZombieWave();
        }
        if (waves == 6)
        {
            zombieOn.SetActive(false);
            breakDown.SetActive(true);
          
        }
        if (waves == 7)
        {
            ZombieWave();
            zombieOn.SetActive(true);
            breakDown.SetActive(false);
            plane.SetActive(false);
            ZombieSpearWave();
        }
        if (waves == 8)
        {
            plane.SetActive(true);
            ZombieWave();
        }
        if(waves >= 8)
        {
           
            LuffyWave();
        }
        if(waveCounts == 9)
        {
            SpawnBigZombie();
            
        }
    }
    private void FlySpawn()
    {
        if (timeBtSpawnFly <= 0)
        {

            int rand = Random.Range(0, pointsOfFly.Length);

            Instantiate(pointsOfFly[rand], spawnFly.transform.position, Quaternion.identity);
            timeBtSpawnFly = StartIntervalBtSpawnFly;
        }
        else
            timeBtSpawnFly -= Time.deltaTime;
    }
    private  void SpawnBigZombie()
    {
        if (timeBtSpawnBigZombie <= 0)
        {
            //int rand = Random.Range(0, pointsOfBigZombie.Length);
            Instantiate(pointsOfBigZombie[0], spawnDropZombie[1].transform.position, Quaternion.identity);
            Instantiate(pointsOfBigZombie[1], spawnDropZombie[0].transform.position, Quaternion.identity);
            timeBtSpawnBigZombie = StartIntervalBtSpawnBigZombie;
            
        }
        else
        {
            timeBtSpawnBigZombie -= Time.deltaTime;
        }

    }
    private void ZombieWave()
    {
        if (TimerBTSpawn <= 0)
        {
            
            int rand = Random.Range(0, pointsOfZombie.Length);
   
            Instantiate(pointsOfZombie[rand], transform.position, Quaternion.identity);
            TimerBTSpawn = StartTimerBtWSpawnl;
            if(StartTimerBtWSpawnl > minTime)
            {
                StartTimerBtWSpawnl -= decreaseTime;
            }
        }
        else
            TimerBTSpawn -= Time.deltaTime;
    }
    private void ZombieSpearWave()
    {
        if (timeBtSpawnSpearZombie <= 0)
        {

            Instantiate(pointsOfSpearZombie, spawnZombiesspear.transform.position, Quaternion.identity);
            timeBtSpawnSpearZombie = StartIntervalBtSpawnSpearZombie;

        }
        else
            timeBtSpawnSpearZombie -= Time.deltaTime;
    }
    private void LuffyWave()
    {
            if (timeBtSpawnluffy <= 0)
            {
                int rand = Random.Range(0, pointsOfLuffy.Length);

                Instantiate(pointsOfLuffy[rand], transform.position, Quaternion.identity);
                timeBtSpawnluffy = StartIntervalBtluffy;
                
            }
            else
            timeBtSpawnluffy -= Time.deltaTime;
    }
    private IEnumerator SpawnerOfMobs()
    {
        zombieWaveFlBr.fillAmount = 1;
        timer = 23;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            zombieWaveFlBr.fillAmount = timer / 23;
            yield return null;
            if(timer <= 0)
            {
                zombieWaveFlBr.fillAmount = 1;
                timer = 23f;
                waveCounts++;
                waves++;
            }
            zombieWaveFlBr.fillAmount = 0;
        }
        time = null;
       
    }
}
