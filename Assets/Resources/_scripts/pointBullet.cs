using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class pointBullet : MonoBehaviour
{
    [SerializeField] private GameplaySettings gameplaySettings;
    private Vector3 moveVector;
    [SerializeField] private float speed, damage;
    [SerializeField] private GameObject destroyEffect;

    void Update()
    {
        moveVector = transform.right * speed * Time.deltaTime;
        transform.position += moveVector;

        Destroy(gameObject, 3f);

    }

    private void DestroyBullet()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void DealDamageInRadius(float radius)
    {
        // Найти всех врагов в заданном радиусе
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

        if (!collision.CompareTag("Bullet"))
        {
            switch (collision.transform.tag)
            {
                case "Enemy":
                    collision.GetComponent<EnemyBasic>().TakeDamage(gameplaySettings.weaponSettings.weapons[3].damage);
                    DestroyBullet();
                    break;
                case "zombie":
                    DealDamageInRadius(1);
                    DestroyBullet();
                    break;
                case "MapEnd":
                    break;
                case "bunker":
                    break;
                default:
                    DestroyBullet();
                    break;
            }

        }
    }

}
