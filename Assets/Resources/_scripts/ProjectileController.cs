﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float speed = 10;
    [Range(0, 3)]
    [SerializeField] private float gravity = 0;
    [SerializeField] private GameplaySettings gameplaySettings;
    public float radius = 5;
    public int damage = 50;
    private Vector3 moveVector;
    public LayerMask fly;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void DamageAllUnitsInRadius(float m_radius)
    {


        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_radius, gameplaySettings.unitsMask);
        if (colliders.Length == 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<EnemyBasic>().TakeDamage(damage);
        }


    }

    // Update is called once per frame
    void Update()
    {
        moveVector = transform.up * speed * Time.deltaTime;

        var angleZ = transform.eulerAngles.z;

        angleZ = Mathf.Lerp(angleZ, angleZ > 0 ? 180 : -180, Time.deltaTime * gravity);

        transform.eulerAngles = new Vector3(0, 0, angleZ);
        transform.position += moveVector;
    }
    private void forZombie()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, gameplaySettings.zombie);
        if (colliders.Length == 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<ZombieAI>().TakeDamage(damage);
            
        }
    }
 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageAllUnitsInRadius(radius);
        switch (collision.transform.tag)
        {
            case "zombie":
                if (explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);
                forZombie();
                Destroy(gameObject);
                break;
            case "Ground":
                if (explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
            case "bunker":
                break;
            case "MapEnd":
                break;
            default:
                if (explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;

        }

    }
   
}
