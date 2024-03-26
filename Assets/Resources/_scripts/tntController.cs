using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tntController : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameplaySettings gameplaySettings;
    [SerializeField] private float heigthPerMeter;
    [SerializeField] private float durationPerMeter;
    public float radius = 5;
    public int damage = 50;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    public void Throw(Vector3 startPosition, Vector3 endPosition)
    {
        var distance = Vector3.Distance(startPosition, endPosition);
        var jumpPower = heigthPerMeter * distance;
        var duration = durationPerMeter * distance;
        transform.position = startPosition;
        transform.DOJump(endPosition, jumpPower, 1, duration).SetEase(Ease.Linear).OnComplete(()=>
        {
            Explosion();
        });
    }

    private void Explosion()
    {
        var pos = transform.position;
        pos.z = 0;
        if (explosionEffect != null) Instantiate(explosionEffect, pos, Quaternion.identity);
        DamageAllUnitsInRadius(radius);
        
    }

    private void DamageAllUnitsInRadius(float m_radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_radius, gameplaySettings.unitsMask);
        if (colliders.Length == 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<EnemyBasic>().TakeDamage(damage);
            
        }
    }
    private void DamageZombieUnitsInRadius(float m_radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_radius, gameplaySettings.zombie);
        if (colliders.Length == 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<ZombieAI>().TakeDamage(damage);

        }
    }
    private void DamageFlyUnitsInRadius(float m_radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_radius, gameplaySettings.fly);
        if (colliders.Length == 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<fly>().TakeDamage(damage);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "zombie")
        DamageZombieUnitsInRadius(radius);
        if (collision.tag == "fly")
            DamageFlyUnitsInRadius(radius);
    }
}
