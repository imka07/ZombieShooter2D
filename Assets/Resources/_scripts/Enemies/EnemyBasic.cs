using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyBasic : MonoBehaviour
{
    private HUDController hudController;
    [Header("HUD")]
    [SerializeField] GameObject hudPrefab;
    [SerializeField] Vector3 offset;
    [SerializeField] private GameplaySettings gameplaySettings;
    private SpriteRenderer[] ch_sprites;

    [Header("Settings")]
    public float maxHealth = 100;
    public float damage = 10;
    public float maxArmor = 100;
    public float collisionDamage = 10;
    private float health = 100;
    private float armor = 100;
    [SerializeField] private float maxCooldown = 2;
    [SerializeField] private float maxDistance = 15;
    private float currentCooldown;
    public Transform player;
    float distance;
    public AudioSource hit;
    [Header("Movement")]
    [Range(0, 2)] [SerializeField] float speedFactor = 1;
    [SerializeField] float patrulField;
    private float moveSpeed { get => gameplaySettings.aISettings.enemyMoveSpeed * speedFactor; }
    private Animator ch_animator;
    public GameObject money;
    [Header("Gun")]
    [SerializeField] GameObject handWithGun;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletLauncher;
    public bool isRed;
    public delegate void OnHealthChangeHandler(float maxHP, float currentHP);
    public OnHealthChangeHandler OnHealthChange;

    public delegate void OnArmorChangeHandler(float maxAR, float currentAR);
    public OnArmorChangeHandler OnArmorChange;


    public float GetHealth()
    {
        return health;
    }

    public float GetArmor()
    {
        return armor;
    }

    public void Init()
    {
        hudController = Instantiate(hudPrefab, transform).GetComponent<HUDController>();
        hudController.transform.localPosition = offset;
        health = maxHealth;
        armor = maxArmor;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        ch_sprites = GetComponentsInChildren<SpriteRenderer>();
        ch_animator = GetComponent<Animator>();

    }

    public void TakeDamage(float damage)
    {   // если броня больше 0 , то сносим броню
        if (armor > 0)
        {
            armor = Mathf.Max(armor - damage, 0);
            OnArmorChange?.Invoke(maxArmor, armor);
            if (armor <= 0)
            {
                var armorBreakFx = Instantiate(gameplaySettings.aISettings.armorBreakFx, transform);
                armorBreakFx.transform.localPosition = new Vector3(0, 1, 0);
            }
        }
        // иначе снимаем жизни
        else 
        {
            health = Mathf.Max(health - damage, 0);
            OnHealthChange?.Invoke(maxHealth, health);
        }
        if(isRed)
        DamageEffect();

        if (health == 0)
        {
            PlayerPrefs.SetFloat("kills", killsCounter.instanse.killsCount);
            Death();
        }
    }

    private void DamageEffect()
    {
        for (int i = 0; i < ch_sprites.Length; i++)
        {
            ch_sprites[i].color = new Color(1, 0.5f, 0.5f);
        }
        Invoke("SetWhiteColor", 0.05f);
    }

    private void SetWhiteColor()
    {
        for (int i = 0; i < ch_sprites.Length; i++)
        {
            ch_sprites[i].color = Color.white;
        }
    }


    public void Death()
    {

        Instantiate(money, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    // пройденная дистанция. если это право , то правда
    private float passedDistance = 0;
    private bool right = true;
    public void Patrule()
    {   // провеверяется, если это право , то в этом случае неправильной стороной является лево
        if (right == true)
        {
            if (transform.localScale.x == -1) transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            passedDistance += moveSpeed * Time.deltaTime;
            if (passedDistance >= patrulField)
            {
                right = false;
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
        }
        // иначе , если это левая сторона ,то правая уже будет неправилной стороной
        else
        {
            if (transform.localScale.x == 1) transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            transform.position -= transform.right * moveSpeed * Time.deltaTime;
            passedDistance -= moveSpeed * Time.deltaTime;
            // если пройденная дистанция меньше 0 , то менятеся scale
            if (passedDistance <= 0)
            {
                right = true;
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            hit.Play();
            collision.gameObject.GetComponent<Test>().TakeDamage(collisionDamage);
        }
    }
    
    public IEnumerator EnemyBehaviour()
    {
        bool rightBullet;
        currentCooldown = maxCooldown;
        while (true)
        {
            if (currentCooldown > 0) currentCooldown -= Time.deltaTime;

            Patrule();
            distance = Vector3.Distance(transform.position, player.position);

            //выстрел если соответствует дистанция и кулдаун. После выстрела кулдаун снова = максКулдаун
            if (distance <= maxDistance && currentCooldown <= 0)
            {
                if (player.position.x >= transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    rightBullet = true;
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    rightBullet = false;
                }
                //Здесь мы останавливаем анимацию в момент прицеливания
                ch_animator.SetBool("Walk", false);
                yield return new WaitForSeconds(0.5f);
                //Мы создаем буллет прифаб на место буллеи лаунчера m и также чтоб направление выстрела не зависела от напрвление ходьбы с помощью rightBullet
                // и сразу делаем урон пули равный урону злодея
                var bullet = Instantiate(bulletPrefab, bulletLauncher.position, Quaternion.identity);
                bullet.GetComponent<BulletController>().right = rightBullet;
                bullet.GetComponent<BulletController>().damage = damage;
                // a тут мы обратно включаем анимацию , когда злодей ходит
                yield return new WaitForSeconds(0.5f);
                ch_animator.SetBool("Walk", true);

                currentCooldown = maxCooldown;
            }
            yield return null;
        }
    }
}
