using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Dron : MonoBehaviour
{
    private float speed;
    private float distanse;
    public AudioSource dronSound;
    public GameObject bulletLauncher;
    private float shootDistance;
    private float damage;
    public GameObject ricohetPrefab;
    public GameObject hitPrefab;
    public GameObject tracerPrefab;
    public AudioSource shootSound;
    private float TimeShot;
    private float startTimeShoot;
    public Image lifeBar;
    private Coroutine time;
    private float timer;
    public AudioSource destroySound;
    void Start()
    {
        StartlifeTime();
        lifeBar.fillAmount = 1;
        startTimeShoot = 0.09f;
        damage = 18f;
        shootDistance = 25f;
        dronSound.Play();
        distanse = -20f;
        speed = 5f;
        transform.DOMoveY(6f, 3f).SetEase(Ease.InOutSine).SetLoops(-12, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        AttackandFly();
    }
    private void StartlifeTime()
    {
        if (time != null) StopCoroutine(time);
        time = StartCoroutine(lifeTime());
    }
    private void AttackandFly()
    {
        if (transform.position.x > distanse)
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }
        else
        {
            if(TimeShot <= 0)
            {
                Shoot();
                TimeShot = startTimeShoot;
            }
            else
            {
                TimeShot -= Time.deltaTime;
            }
            
        }
           
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private void Shoot()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(bulletLauncher.transform.position, bulletLauncher.transform.right, shootDistance);
        CheckHit(hit);
        DrawTraccer(hit);
    }
    private void CheckHit(RaycastHit2D hit)
    {
        if (hit)
        {
            switch (hit.transform.tag)
            {
                case "Enemy":
                    hit.transform.GetComponent<EnemyBasic>().TakeDamage(damage);
                    if (hit.transform.GetComponent<EnemyBasic>().GetArmor() > 0)
                    {
                        Instantiate(ricohetPrefab, hit.point, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(hitPrefab, hit.point, Quaternion.identity);
                    }
                    break;
                case "zombie":
                    hit.transform.GetComponent<ZombieAI>().TakeDamage(damage);
                    Instantiate(hitPrefab, hit.point, Quaternion.identity);
                    break;
                default:
                    Instantiate(ricohetPrefab, hit.point, Quaternion.identity);
                    break;
            }
        }
    }
    private IEnumerator lifeTime()
    {
        lifeBar.fillAmount = 1;
        timer = 12f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            lifeBar.fillAmount = timer / 12;
            yield return null;
        }
        destroySound.Play();
        Instantiate(hitPrefab, transform.position, Quaternion.identity);
        Die();
        lifeBar.fillAmount = 0;
        time = null;

    }

    private void DrawTraccer(RaycastHit2D hit)
    {
            if (hit)
            {
                var line = Instantiate(tracerPrefab).GetComponent<LineRenderer>();
                line.SetPosition(0, bulletLauncher.transform.position);
                line.SetPosition(1, hit.point);
            }
            else
            {
                var line = Instantiate(tracerPrefab).GetComponent<LineRenderer>();
                line.SetPosition(0, bulletLauncher.transform.position);
                line.SetPosition(1, bulletLauncher.transform.position + bulletLauncher.transform.right * shootDistance);
            }
        
        
    }
}
