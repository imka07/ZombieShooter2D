using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class pointBullet : MonoBehaviour
{
    [SerializeField] private GameplaySettings gameplaySettings;
    private Vector3 moveVector;
    [SerializeField] private float speed;
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
                    collision.GetComponent<ZombieAI>().TakeDamage(gameplaySettings.weaponSettings.weapons[3].damage);
                    DestroyBullet();
                    break;
                case "fly":
                    collision.GetComponent<fly>().TakeDamage(gameplaySettings.weaponSettings.weapons[3].damage);
                    DestroyBullet();
                    break;
            }

            if (!collision.CompareTag("MapEnd")) DestroyBullet();
        }
    }

}
