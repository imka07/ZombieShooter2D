﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using DG.Tweening.Core.Easing;

public class ZombieAI : MonoBehaviour
{
    [Header("Zombie Stats")]
    public float speed;
    public float collisionDamage;
    public float health;
    public float maxHealth;
    public float heightAbove;
    protected float timeBetweenAttack;
    public float startBetweenAttack;
    public float attackRange;
    public float damage;
    public float cashOnDeath;
    public float distance;
    bool isWalking;
    protected bool canAttack;

    [Header("Zombie Components")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] GameObject hudPrefab;
    private Rigidbody2D rb;
    [SerializeField] Vector3 offset;
    private ZombieHud hudController;
    public ParticleSystem deathParticles;
    public Transform attackPos;
    public AudioClip[] audioClips;
    protected Animator anim;
    public LayerMask playerMask;
    public LayerMask bunker;

    public bool isBoss = false;

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
        cashOnDeath *= gameManager.instance.cashFactor;
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
            anim.SetBool("isWalking", false);
        }
    }

    public void PlayClips(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
    
    public virtual void Attack()
    {
        if (canAttack) OnAttack();
    }

    public virtual void OnAttack()
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
        FloatDamageController.instance.FloatDamageEffect(transform.position, damage);
        if (health <= 0)
        {
            Death();
        }
    }
    private void Death()
    {
        gameManager.instance.AddCash(cashOnDeath);
        OnDestroyed.Invoke();
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);

        if (isBoss) gameManager.instance.GameWin();
    }

}
