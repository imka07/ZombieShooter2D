using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUiController : MonoBehaviour
{
    GameObject player;
    GameObject bunker;
    GunController gunController;
    [SerializeField] Image speedFill;
    [SerializeField] Image jumpFill;
    [SerializeField] Image armorFill;
    [SerializeField] Image hpFill;
    [SerializeField] Image MedFill;
    [SerializeField] Image DronFill;
    [SerializeField] TextMeshProUGUI ammoCounter;
    [SerializeField] TextMeshProUGUI genAmmoCounter;
    [SerializeField] Image hpFillBase;
    Coroutine speedUpCoroutine, jumpCoroutine, armorCoroutine, medicineCourine, droncoroutine;

    public Image weaponIcon;
    public Sprite[] weaponSprites;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bunker = GameObject.FindGameObjectWithTag("bunker");
        bunker.GetComponent<Base>().OnHealthChange += UpdateHpBase;
        gunController = player.GetComponentInChildren<GunController>();
        gunController.OnAmmoChange += OnAmmoChange;
        player.GetComponent<Test>().OnHpChange += UpdateHp;
        speedFill.fillAmount = 0;
        jumpFill.fillAmount = 0;
        armorFill.fillAmount = 0;
        MedFill.fillAmount = 1;
        DronFill.fillAmount = 1;
    }

    public void ChangeWeaponIcon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weaponSprites.Length)
        {
            weaponIcon.sprite = weaponSprites[weaponIndex];
        }
        else
        {
            Debug.LogWarning("Weapon index out of range!");
        }
    }


    public void StartArmor()
    {
        if (armorCoroutine != null) StopCoroutine(armorCoroutine);
        armorCoroutine = StartCoroutine(Armor());
    }
    public void StartMed()
    {
        if (medicineCourine != null) StopCoroutine(medicineCourine);
        medicineCourine = StartCoroutine(Medicine());
    }
    public void StartDron()
    {
        if (droncoroutine != null) StopCoroutine(droncoroutine);
        droncoroutine = StartCoroutine(DronStart());
    }
    private IEnumerator DronStart()
    {
        DronFill.fillAmount = 1;
        float timer = 17;
        var dron = FindObjectOfType<Test>();

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            DronFill.fillAmount = timer / 17;

            yield return null;
            if (timer <= 0)
            {
                DronFill.fillAmount = 1;


            }
        }

        dron.StopDron();
        droncoroutine = null;
    }
    private IEnumerator Armor()
    {
        armorFill.fillAmount = 1;
        float timer = 7;
        player.GetComponent<Test>().ArmorSetActive(true);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            armorFill.fillAmount = timer / 7;
            yield return null;
        }
        armorFill.fillAmount = 0;
        player.GetComponent<Test>().StopArmor();
        player.GetComponent<Test>().ArmorSetActive(false);
        armorCoroutine = null;
    }
    private IEnumerator Medicine()
    {
        MedFill.fillAmount = 1;
        float timer = 6;
        var med = FindObjectOfType<Test>();
      
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            MedFill.fillAmount = timer / 6;
            
            yield return null;
            if (timer <= 0)
            {
                MedFill.fillAmount = 1;


            }
        }
      
        med.StopMed();
        medicineCourine = null;
    }

    public void StartBuffJump()
    {
        if (jumpCoroutine != null) StopCoroutine(jumpCoroutine);
        jumpCoroutine = StartCoroutine(BuffJump());
    }
    private IEnumerator BuffJump()
    {
        jumpFill.fillAmount = 1;
        float timer = 7;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            jumpFill.fillAmount = timer / 7;
            yield return null;
        }
        jumpFill.fillAmount = 0;
        player.GetComponent<Test>().StopBuffJump();
        jumpCoroutine = null;
    }

    public void StartSpeedUp()
    {
        if (speedUpCoroutine != null) StopCoroutine(speedUpCoroutine);
        speedUpCoroutine = StartCoroutine(SpeedUp());
    }
    private IEnumerator SpeedUp()
    {
        speedFill.fillAmount = 1;
        float timer = 7;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            speedFill.fillAmount = timer / 7;
            yield return null; 
        }
        speedFill.fillAmount = 0;
        player.GetComponent<Test>().StopSpeedUp();
        speedUpCoroutine = null;
    }

    private void UpdateHp(float maxHp, float currentHp)
    {
        hpFill.fillAmount = currentHp / maxHp;
    }
    private void UpdateHpBase(float maxHp, float currentHp)
    {
        hpFillBase.fillAmount = currentHp / maxHp;
    }

    private void OnAmmoChange()
    {
        ammoCounter.text = gunController.currentAmmo.ToString();
        genAmmoCounter.text = gunController.currentGunIndex >= 0 ? "∞" : gunController.genAmmo.ToString();
    }
  
}
