using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "New Gameplay Settings", menuName = "Gameplay Settings", order = 51)]
public class GameplaySettings : ScriptableObject
{
    [Header("Player Settings")]
    public float playerMaxMoveSpeed = 8;
    public float speedBonus = 8;

    public WeaponSettings weaponSettings;
    public AISettings aISettings;
 
    [Header("LayerMasks")]
    public LayerMask unitsMask;
    public LayerMask zombie;

}

[System.Serializable]
public class AISettings
{
    public GameObject armorBreakFx;
    public float enemyMoveSpeed = 8;
}

[System.Serializable]
public class WeaponSettings
{
    [Header("Global Settings")]
    public float shootDistance = 25;

    public List<Weapon> weapons;
}

[System.Serializable]
public class Weapon
{
    public int id;
    public string name;
    public float damage = 30;
    public float shootDelay = 0.2f;
    public int maxAmmo = 10;
    public float reloadTime = 1;
    public AudioClip shootClip;
    public AudioClip reloadClip;
    public Sprite weaponSprite;
    public int weaponPrice;
    public Sprite weaponLootSprite;
    public Vector3 bulletLauncherPos;
    public GameObject projectile;
    public GameObject arrow;
    public GameObject ShotGunBullet;
    public Vector3 point;
    
}
