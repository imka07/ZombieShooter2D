using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointBullet : MonoBehaviour
{
    [SerializeField] private GameplaySettings gameplaySettings;
    private Vector3 moveVector;
    [SerializeField] private float speed;

    void Update()
    {
        moveVector = transform.right * speed * Time.deltaTime;
        transform.position += moveVector;

        Destroy(gameObject, 3f);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Bullet"))
        {
            switch (collision.transform.tag)
            {
                case "Enemy":
                    collision.GetComponent<EnemyBasic>().TakeDamage(gameplaySettings.weaponSettings.weapons[3].damage);
                    Destroy(gameObject);
                    break;
                case "zombie":
                    collision.GetComponent<ZombieAI>().TakeDamage(gameplaySettings.weaponSettings.weapons[3].damage);
                    Destroy(gameObject);
                    break;
                case "fly":
                    collision.GetComponent<fly>().TakeDamage(gameplaySettings.weaponSettings.weapons[3].damage);
                    Destroy(gameObject);
                    break;
                default:
                    Destroy(gameObject);
                    break;
            }
        }
    }

}
