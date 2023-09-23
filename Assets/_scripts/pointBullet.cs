using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointBullet : MonoBehaviour
{
  
    private float damage;
    private Rigidbody2D rb;
    private Vector3 moveVector;
    private float speed;
    void Start()
    {
        speed = 75f;
        rb = GetComponent<Rigidbody2D>();
        damage = 25f;
    }

    // Update is called once per frame
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
            collision.GetComponent<EnemyBasic>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.tag == "zombie")
        {
            collision.GetComponent<ZombieAI>().TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.tag == "fly")
        {
            collision.GetComponent<fly>().TakeDamage(damage);

        }
    }
}
