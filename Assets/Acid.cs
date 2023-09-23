using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float speed = 10;
    [Range(0, 3)]
    [SerializeField] private float gravity = 0;
    [SerializeField] private GameplaySettings gameplaySettings;
    public float radius = 5;
    public int damage = 50;
    private Vector3 moveVector;
    public LayerMask bunker;

    // Start is called before the first frame update
    void Start()
    {

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
   
    private void forBunker()
    {
        FindObjectOfType<Base>().DamageEffect();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, bunker);
        if (colliders.Length == 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Base>().TakeDamage(damage);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (explosionEffect) Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        if (collision.tag == "bunker")
        {
            forBunker();
        }
        
    }


}
