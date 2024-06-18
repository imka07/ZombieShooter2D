using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BossController : ZombieAI
{
    [SerializeField] private GameObject[] spawnEnemies;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject meteorPrefabs;
    public float spawnY;
    public float restTime = 1.5f; // время отдыха между мувсетами

    private float attackDuration;
    private Transform player; // Ссылка на игрока
    private float healthThreshold = 0.5f; // Порог здоровья (20%)

    public bool isAttacking;

    void Start()
    {
        Init();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Предполагается, что у игрока есть тег "Player"
    }

    private void Update()
    {
        // Проверка здоровья босса и переход к третьему мувсету при необходимости
        if (GetHealthPercentage() <= healthThreshold)
        {
            StopAllCoroutines();
            StartCoroutine(LowHealthMoveSet());
        }
    }

    public void StartAttackiing()
    {
        isAttacking = true;
        StartCoroutine(AttackPattern());
    }

    private IEnumerator AttackPattern()
    {
        while (isAttacking)
        {
            // Случайное время выполнения текущего мувсета
            attackDuration = Random.Range(5f, 7f);

            // Выбор случайного мувсета
            int moveSet = Random.Range(0, 2); // 0 для EnemySpawnMoveSet, 1 для MeteorSpawnMoveSet

            if (moveSet == 0)
            {
                yield return StartCoroutine(EnemySpawnMoveSet(attackDuration));
            }
            else
            {
                yield return StartCoroutine(MeteorSpawnMoveSet(attackDuration));
            }

            // Время отдыха между мувсетами
            yield return new WaitForSeconds(restTime);
        }
    }

    private IEnumerator EnemySpawnMoveSet(float duration)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            if (isAttacking)
            {
                if (timeBetweenAttack <= 0)
                {
                    var rand = Random.Range(0, spawnEnemies.Length);
                    var delay = 3;
                    PlayClips(1);
                    anim.SetTrigger("attack");
                    Instantiate(spawnEnemies[rand], spawnPosition.position, Quaternion.identity);
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

    private IEnumerator MeteorSpawnMoveSet(float duration)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            if (isAttacking)
            {
                if (timeBetweenAttack <= 0)
                {
                    var rand = Random.Range(0, 80);
                    var delay = 0.5f;
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
    public float rotationModifier;
    private IEnumerator LowHealthMoveSet()
    {
        while (GetHealthPercentage() <= healthThreshold)
        {
            if (player != null)
            {
                if (Vector2.Distance(transform.position, player.position) < attackRange)
                {
                    anim.SetBool("isWalking", false);
                    Attack();
                }
                else
                {
                    // Rotate to face the player
                    FacePlayer();

                    transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * speed);
                    anim.SetBool("isWalking", true);
                }
            }
            yield return null;
        }
    }

    private void FacePlayer()
    {
        if (player != null)
        {
            Vector3 scale = transform.localScale;
            if (player.position.x < transform.position.x)
            {
                scale.x = Mathf.Abs(scale.x); // Face left
            }
            else
            {
                scale.x = Mathf.Abs(scale.x) * -1; // Face right
            }
            transform.localScale = scale;
        }
    }

    public override void Attack()
    {
        if (isAttacking) OnAttack();
    }

    public override void OnAttack()
    {
        if (timeBetweenAttack <= 0)
        {
            PlayClips(1);
            anim.SetTrigger("attack");
            Collider2D[] player = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerMask);
            for (int i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<Test>().TakeDamage(damage);
            }
            timeBetweenAttack = startBetweenAttack;
        }
        else
        {
            timeBetweenAttack -= Time.deltaTime;
        }
    }

    private float GetHealthPercentage()
    {
        // Предполагается, что у вас есть метод для получения текущего здоровья босса
        return (float)health / maxHealth;
    }
}
