using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float radius;
    [SerializeField] private GameplaySettings gameplaySettings;
    private float damage;
    [SerializeField] private GameObject explosionEffect;
    void Start()
    {
        radius = 20f;
        damage = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Destroy(gameObject);
        if (explosionEffect) Instantiate(explosionEffect, transform.position, Quaternion.identity);
        if (collision.tag == "zombie")
        {
           
            forZombie();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (explosionEffect) Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
