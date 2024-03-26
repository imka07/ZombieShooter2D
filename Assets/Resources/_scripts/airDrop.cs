using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airDrop : MonoBehaviour
{

    public GameObject variant;
    //public AudioSource landingSound;
    public GameObject Spawnvariant;
    //public ParticleSystem redSmoke;
    //public GameObject redSmokeSpawn;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       

    }
    private void LootAir()
    {
        //landingSound.Play();
        Instantiate(variant, Spawnvariant.transform.position, Quaternion.identity);
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
           
            LootAir();
            Destroy(gameObject);
          
        }
    }
}
