using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float radius;
    [SerializeField] private GameplaySettings gameplaySettings;
    private float damage;
    [SerializeField] private GameObject explosionEffect;
    public LayerMask playerMask;
    void Start()
    {
        radius = 5;
        damage = 10;
    }

    private void PlayerDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, playerMask);
        if (colliders.Length == 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Test>().TakeDamage(damage);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "MapEnd")
        { 
            if (explosionEffect) Instantiate(explosionEffect, transform.position, Quaternion.identity);
            PlayerDamage();
            Destroy(gameObject);
        }
    }
}
