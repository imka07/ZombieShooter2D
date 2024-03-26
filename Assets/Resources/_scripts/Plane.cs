using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    private float speed;
    public AudioSource planeSound;
    public GameObject airdropSpawn;
    public GameObject airDrop;
    public AudioSource spawnSound;
    private bool can = false;
    
    public float distanse;
    // Start is called before the first frame update
    void Start()
    {
        planeSound.Play();
        speed = 16f;
    }

    // Update is called once per frame
    void Update()
    {
        
        Fly();
    }
    private void Fly()
    {
        Destroy(gameObject, 10f);
        transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        if(!can)
        {
            if (transform.position.x <= distanse)
            {
                can = true;
                spawnSound.Play();
                Instantiate(airDrop, airdropSpawn.transform.position, Quaternion.identity);
            }
        }
        
    }

}
