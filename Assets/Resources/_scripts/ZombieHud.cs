using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieHud : MonoBehaviour
{
    [SerializeField] Image hpBarfill;
    ZombieAI zombieState;
    


    void Start()
    {
        enemy();
    }
    private void enemy()
    {
        
        zombieState = transform.parent.GetComponent<ZombieAI>();
        zombieState.OnHealthChange += UpdateHpBar;
       
    }
    

    void Update()
    {

    }
    public void UpdateHpBar(float maxHP, float currentHP)
    {
        hpBarfill.fillAmount = currentHP / maxHP;
    }
   
}
