using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Base : MonoBehaviour
{
    public delegate void OnHealthChangeHandler(float maxHP, float currentHP);
    public OnHealthChangeHandler OnHealthChange;
    private float health;
    public float maxHealth = 500f;
    private SpriteRenderer[] ch_sprites;
    public GameObject diePannel;
    private void Awake()
    {
        diePannel.SetActive(false);
        health = maxHealth;
        ch_sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       
       
    }
    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        OnHealthChange?.Invoke(maxHealth, health);
        DamageEffect();
        if(health <= 0)
        {
            diePannel.SetActive(true);
            Time.timeScale = 0;
            Destroy(gameObject);
        }
    }
    public void Heal(float heal)
    {
        // это у нас аптека повышается жизни
        health = Mathf.Min(health + heal, maxHealth);
        OnHealthChange?.Invoke(maxHealth, health);
    }
    public void DamageEffect()
    {
        for (int i = 0; i < ch_sprites.Length; i++)
        {
            ch_sprites[i].color = new Color(1, 0.5f, 0.5f);
        }
        Invoke("SetWhiteColor", 0.05f);
    }

    private void SetWhiteColor()
    {
        for (int i = 0; i < ch_sprites.Length; i++)
        {
            ch_sprites[i].color = Color.white;
        }
    }

}
