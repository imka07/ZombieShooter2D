using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class ZombieAI : MonoBehaviour
{
    [Header("Zombie Stats")]
    public float speed;
    [SerializeField] private float collisionDamage;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    public float heightAbove;
    private float timeBetweenAttack;
    public float startBetweenAttack;
    public float attackRange;
    [SerializeField] private float damage;
    public float distanse;
    bool isWalking;
    bool canAttack;

    [Header("Zombie Components")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] GameObject hudPrefab;
    private Rigidbody2D rb;
    [SerializeField] Vector3 offset;
    private ZombieHud hudController;
    public ParticleSystem deathParticles;
    public Transform attackPos;
    public AudioClip[] audioClips;
    [SerializeField] private Animator anim;
    public LayerMask playerMask;
    public LayerMask bunker;



    public delegate void OnHealthChangeHandler(float maxHP, float currentHP);
    public OnHealthChangeHandler OnHealthChange;

    public static event UnityAction OnDestroyed;

    public void Init()
    {
        isWalking = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hudController = Instantiate(hudPrefab, transform).GetComponent<ZombieHud>();
        hudController.transform.localPosition = offset;
        health = maxHealth;
    }

    public void MoveToBunker()
    {
        if (isWalking)
        {
            anim.SetBool("isWalking", true);
            Vector2 movement = new Vector2(-speed * Time.deltaTime, 0f); // Создаем вектор для перемещения
            rb.MovePosition(rb.position + movement); // Перемещаем объект с использованием Rigidbody
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayClips(0);
            collision.gameObject.GetComponent<Test>().TakeDamage(collisionDamage);
        }
        else if (collision.tag == "bunker")
        {
            canAttack = true;
            isWalking = false;
        }
    }

    public void PlayClips(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
    
    public void Attack()
    {
        if (canAttack) OnAttack();
    }

    private void OnAttack()
    {
        if(timeBetweenAttack <= 0)
        {
            PlayClips(1);
            anim.SetTrigger("attack");
            Collider2D[] player = Physics2D.OverlapCircleAll(attackPos.position, attackRange, bunker);
            for (int i = 0; i < player.Length; i++)
            {
                player[i].GetComponent<Base>().TakeDamage(damage);
            }
            timeBetweenAttack = startBetweenAttack;
        }
        else
        {
            timeBetweenAttack -= Time.deltaTime;
        }
        
    }
   
    
    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        OnHealthChange?.Invoke(maxHealth, health);
        if (health <= 0)
        {
            Death();
        }
    }
    private void Death()
    {
        OnDestroyed.Invoke();
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
