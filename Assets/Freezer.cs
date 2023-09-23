using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Freezer : MonoBehaviour
{
  
    private Rigidbody2D rb;
    public AudioSource freeseSound;
   
    public ParticleSystem snowDie;
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        Destroy();
    }

    // Update is called once per frame
    
    private void Destroy()
    {
        Instantiate(snowDie, transform.position, Quaternion.identity);
       
        Destroy(gameObject, 13f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "zombie")
        {
          
            FindObjectOfType<ZombieAI>().speed = 0.4f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
           
            freeseSound.Play();
        }
    }
}
