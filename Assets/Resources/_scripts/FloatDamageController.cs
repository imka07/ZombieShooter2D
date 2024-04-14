using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatDamageController : MonoBehaviour
{
    public static FloatDamageController instance;

    [SerializeField] private Transform popUpDamage;

    private void Start()
    {
        instance = this;
    }

    public void FloatDamageEffect(Vector3 spawnPos, float damageAmount)
    {
        Transform damagePopupTransform = Instantiate(popUpDamage, spawnPos, Quaternion.identity);
        DamagePopUp damagePopUp = damagePopupTransform.GetComponent<DamagePopUp>();
        damagePopUp.SetUp(damageAmount);
    }
}
