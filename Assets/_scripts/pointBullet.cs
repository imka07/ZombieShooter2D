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

        var angleZ = transform.eulerAngles.z;

        angleZ = Mathf.Lerp(angleZ, angleZ > 0 ? 180 : -180, Time.deltaTime);

        //transform.eulerAngles = new Vector3(0, 0, angleZ);
        transform.position += moveVector;

        Destroy(this.gameObject, 3f);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyBasic>().TakeDamage(gameplaySettings.weaponSettings.weapons[3].damage);
            Destroy(gameObject);
        }
        if (collision.tag == "zombie")
        {
            collision.GetComponent<ZombieAI>().TakeDamage(gameplaySettings.weaponSettings.weapons[3].damage);
            Destroy(gameObject);
        }
        if (collision.tag == "fly")
        {
            collision.GetComponent<fly>().TakeDamage(gameplaySettings.weaponSettings.weapons[3].damage);

        }
    }
}
