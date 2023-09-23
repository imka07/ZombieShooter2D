using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class fly : MonoBehaviour
{
    [SerializeField] private float collisionDamage;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    public float speed;
    private flyHud hudController;
    public AudioSource hit;
    private SpriteRenderer[] ch_sprites;
    [SerializeField] GameObject hudPrefab;
    [SerializeField] Vector3 offset;
    [SerializeField] private AudioSource flySound;
    public float distanse;
    public GameObject[] stuffs;
    private float timeBTattack;
    public float startBTattack;
    public GameObject wave;
    public Transform attackPos;
    public GameObject moneyPointOfSpawn;
    public ParticleSystem diePart;
    public GameObject acid;
    [SerializeField] private AudioSource acidDrop;
    public delegate void OnHealthChangeHandler(float maxHP, float currentHP);
    public OnHealthChangeHandler OnHealthChange;
    [SerializeField] private GameplaySettings gameplaySettings;
    public bool secondFly;
    void Start()
    {
       
        transform.DOMoveY(4f, 1f).SetEase(Ease.InOutSine).SetLoops(-12, LoopType.Yoyo);
        flySound.Play();
        hudController = Instantiate(hudPrefab, transform).GetComponent<flyHud>();
        hudController.transform.localPosition = offset;
        health = maxHealth;
        ch_sprites = GetComponentsInChildren<SpriteRenderer>();
    
    }

    // Update is called once per frame
    void Update()
    {
        AttackandFly();
    }
    private void AttackandFly()
    {

    
        if (transform.position.x < distanse)
        {
            
           
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }
        else
        {
            if(secondFly)
            {
                MyxaAttack();
            }
            else
            BigMyxaAttack();
        }

    }
    private void BigMyxaAttack()
    {
        if (timeBTattack <= 0)
        {

            Instantiate(wave, attackPos.position, Quaternion.identity);
            timeBTattack = startBTattack;
        }
        else
        {
            timeBTattack -= Time.deltaTime;
        }
    }
    private void MyxaAttack()
    {
        if (timeBTattack <= 0)
        {
            acidDrop.Play();
            Instantiate(acid, attackPos.position, Quaternion.identity);
            timeBTattack = startBTattack;
        }
        else
        {
            timeBTattack -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hit.Play();
            collision.gameObject.GetComponent<Test>().TakeDamage(collisionDamage);
        }



    }
    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        OnHealthChange?.Invoke(maxHealth, health);

        if (health == 0)
        {
            PlayerPrefs.SetFloat("kills", killsCounter.instanse.killsCount);
            killsCounter.instanse.killsCount++;
            killsCounter.instanse.KillUpdate();
            Death();
        }
    }
    private void Death()
    {
        Instantiate(diePart, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}
