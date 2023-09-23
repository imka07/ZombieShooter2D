using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] Image hpBarfill;
    EnemyBasic enemyState;
    [SerializeField] Image arBarfill;
   

    void Start()
    {
        
        enemy();
    }
    private void enemy()
    {
        enemyState = transform.parent.GetComponent<EnemyBasic>();
        enemyState.OnHealthChange += UpdateHpBar;
        enemyState.OnArmorChange += UpdateArBar;
    }
   

    void Update()
    {
        
    }
    public void UpdateHpBar(float maxHP, float currentHP)
    {
        hpBarfill.fillAmount = currentHP / maxHP;
    }
    public void UpdateArBar(float maxAr, float currentAr)
    {
        arBarfill.fillAmount = currentAr / maxAr;
    }

}
