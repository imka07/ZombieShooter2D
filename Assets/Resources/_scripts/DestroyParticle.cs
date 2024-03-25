using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField] bool destroyWhenEndSound = false;
    ParticleSystem particle;
    AudioSource sound;
    
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        sound = GetComponent<AudioSource>();

        if (destroyWhenEndSound)
        {
            sound.pitch = Random.Range(0.85f, 1.15f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyWhenEndSound)
        {
            if (!sound.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        else 
        {
            if (!particle.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        
        
    }
    
   
}
