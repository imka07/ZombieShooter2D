using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundHit : MonoBehaviour
{
    private float damage;
    public AudioSource waveSound;
    public AudioSource hit;
    public ParticleSystem lightnes;
    void Start()
    {
        Instantiate(lightnes, transform.position, Quaternion.identity);
        waveSound.Play();
        damage = 14f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            hit.Play();
            collision.GetComponent<Test>().TakeDamage(damage);
        }
    }
}
