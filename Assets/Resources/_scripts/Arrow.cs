using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed;
    private float damage;
    private Vector3 moveVector;
    public float radius;
    [SerializeField] private GameplaySettings gameplaySettings;
    void Start()
    {
        radius = 0.1f;
        damage = 35f;
        speed = 70f;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 5f);
        moveVector = transform.right * speed * Time.deltaTime;

        var angleZ = transform.eulerAngles.z;

        angleZ = Mathf.Lerp(angleZ, angleZ > 0 ? 180 : -180, Time.deltaTime);

        //transform.eulerAngles = new Vector3(0, 0, angleZ);
        transform.position += moveVector;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyBasic>().TakeDamage(damage);
           
        }
        if (collision.tag == "zombie")
        {
            collision.GetComponent<ZombieAI>().TakeDamage(damage);
           
        }
        if (collision.tag == "fly")
        {
            collision.GetComponent<fly>().TakeDamage(damage);

        }
    }
}
