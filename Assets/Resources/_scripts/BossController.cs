using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : ZombieAI
{
    [SerializeField] private GameObject[] spawnEnemies;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private int spawnCount, maxSpawnCount;


    [SerializeField] private GameObject meteorPrefabs;
    public float spawnY;

    void Start()
    {
        canAttack = true;
        Init();
    }


    private void Update()
    {
        BossAttackMoveSets();
    }

    private void BossAttackMoveSets()
    {
        StartCoroutine(MeteorSpawnMoveSet());
    }


    private IEnumerator EnemySpawnMoveSet()
    {
        if (canAttack)
        {
            if (timeBetweenAttack <= 0)
            {
                if (spawnCount < maxSpawnCount)
                {
                    var rand = Random.Range(0, spawnEnemies.Length);
                    PlayClips(1);
                    anim.SetTrigger("attack");
                    Instantiate(spawnEnemies[rand], spawnPosition.position, Quaternion.identity);
                    timeBetweenAttack = startBetweenAttack;
                    spawnCount++;
                }
            }
            else
            {
                timeBetweenAttack -= Time.deltaTime;
            }
        }
        yield return null;
    }


    private IEnumerator MeteorSpawnMoveSet()
    {
        if (canAttack)
        {
            if (timeBetweenAttack <= 0)
            {
                var rand = Random.Range(0, 100);
                var delay = 1;
                PlayClips(1);
                Instantiate(meteorPrefabs, new Vector2(rand, spawnY), Quaternion.identity);
                timeBetweenAttack = delay;
            }
            else
            {
                timeBetweenAttack -= Time.deltaTime;
            }
        }
        yield return null;
    }
}
