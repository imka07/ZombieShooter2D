using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flyHud : MonoBehaviour
{
    [SerializeField] Image hpBarfill;
    fly flytate;



    void Start()
    {

        enemy();
    }
    private void enemy()
    {

        flytate = transform.parent.GetComponent<fly>();
        flytate.OnHealthChange += UpdateHpBar;

    }
   

    void Update()
    {

    }
    public void UpdateHpBar(float maxHP, float currentHP)
    {
        hpBarfill.fillAmount = currentHP / maxHP;
    }


}
