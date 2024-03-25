using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ZombieAI : MonoBehaviour
{
    private Sequence sequence;
    public float speed;
    [SerializeField] private float collisionDamage;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    private SpriteRenderer[] ch_sprites;
    [SerializeField] private AudioSource acidDrop;
    [SerializeField] GameObject hudPrefab;
    [SerializeField] Vector3 offset;
    private ZombieHud hudController;
    public GameObject[] stuffs;
    public GameObject moneyPointOfSpawn;
    private float timeBTattack;
    public float startBTattack;
    public ParticleSystem diePart;
    [SerializeField] private Animator anim;
    public AudioSource hit;
    public Transform attackPos;
    public float attackRange;
    [SerializeField] private float damage;
    public LayerMask pl;
    public LayerMask playerMask;
    public Transform bunker;
    [SerializeField] private AudioSource baseHit;
    public delegate void OnHealthChangeHandler(float maxHP, float currentHP);
    public OnHealthChangeHandler OnHealthChange;
    [SerializeField] private GameplaySettings gameplaySettings;
    private SpriteRenderer sprite;
    [SerializeField] private AudioSource zombieRoar;
    public float distanse;
    public bool isBig;
    public bool isdropZombie;
    public bool is2WaveZombie;
    public bool isZombieSpear;
    public GameObject secondZombie;
    public GameObject acid;
    private Transform player;
   
    void Start()
    {
       
        player = GameObject.FindGameObjectWithTag("PlayerPoint").transform;
       anim.SetBool("isWalking", true);
        zombieRoar.Play();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        hudController = Instantiate(hudPrefab, transform).GetComponent<ZombieHud>();
        hudController.transform.localPosition = offset;
        health = maxHealth;
        ch_sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    { 
        Ai();
    }
    private void Ai()
    {
        anim.SetBool("walk", true);
        if (!isZombieSpear)
        {
            if (transform.position.x < distanse)
            {
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            }
            else
            {
                Attack();
                    anim.SetBool("isWalking", false);
                if(isdropZombie)
                {
                    anim.SetBool("walk", false);
                    DropZombieAttack();
                }


            }
        }
        else
            ChasePlayer();
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hit.Play();
            collision.gameObject.GetComponent<Test>().TakeDamage(collisionDamage);
        }
    }
    
    private void Attack()
    {
        OnAttack();
    }
    
    private void ChasePlayer()
    {
        if (transform.position.x < player.position.x - 2)
        {
            transform.localScale = new Vector2(1, 1);
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }
        else if (transform.position.x > player.position.x + 1)
        {
            transform.localScale = new Vector2(-1, 1);
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }
        
    }
    private void OnAttack()
    {
        if(timeBTattack <= 0)
        {
            baseHit.Play();
            anim.SetTrigger("attack");
            Collider2D[] player = Physics2D.OverlapCircleAll(attackPos.position, attackRange, pl);
            for (int i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<Base>().TakeDamage(damage);
            }
            timeBTattack = startBTattack;
        }
        else
        {
            timeBTattack -= Time.deltaTime;
        }
        
    }
   
    
    private void ZombieSpearAttack()
    {
        if (timeBTattack <= 0)
        {
            hit.Play();
            anim.SetTrigger("attack");
            Collider2D[] player = Physics2D.OverlapCircleAll(attackPos.position, attackRange, playerMask);
            for (int i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<Test>().TakeDamage(damage);
            }
            timeBTattack = startBTattack;
        }
        else
        {
            timeBTattack -= Time.deltaTime;
        }
    }
    public void DropZombieAttack()
    {
        if(timeBTattack <= 0)
        {
            acidDrop.Play();
            Instantiate(acid, attackPos.position, Quaternion.identity);
            anim.SetBool("attack", true);
            timeBTattack = startBTattack;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Freeze")
        {
            anim.SetFloat("Speed", 1);
            speed = 1.9f;
        }
    }
    
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isZombieSpear)
        {
            ZombieSpearAttack();
        }
    }
    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        OnHealthChange?.Invoke(maxHealth, health);
      
        if (health == 0)
        {
            PlayerPrefs.SetFloat("kills", killsCounter.instanse.killsCount);
            killsCounter.instanse.killsCount++;
            killsCounter.instanse.KillUpdate();
            Death();
        }
    }
    private void Death()
    {
        if (is2WaveZombie)
        {
            Instantiate(secondZombie, transform.position, Quaternion.identity);
        }

        var rand = Random.Range(0, stuffs.Length);
         Instantiate(stuffs[rand], moneyPointOfSpawn.transform.position, Quaternion.identity);
         Instantiate(diePart, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
   
}
