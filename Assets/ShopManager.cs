using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;
   
    [SerializeField] private GameplaySettings gameplaySettings;
    public Image[] image;
    GunController gunController;
    GameObject player;
    private Test test;
    private FreezerSpawn fridge;
    private AcidSpawner acidRain;
    public GameObject[] shopPanels;
    private MainUiController MUC;
    public Text moneyText;
    private float money;
    public Text TntText;
    private float tntCount;
    public Text FridgeText;
    private float fridgeCount;
    public Text AcidText;
    private float AcidRainCount;
    [SerializeField] TextMeshProUGUI ammoCounter;
    [SerializeField] TextMeshProUGUI genAmmoCounter;
    public AudioSource failed;
    public AudioSource Click;
    void Start()
    {
        fridge = FindObjectOfType<FreezerSpawn>();
        acidRain = FindObjectOfType<AcidSpawner>();
        player = GameObject.FindGameObjectWithTag("Player");
        gunController = player.GetComponentInChildren<GunController>();
        gunController.OnAmmoChange += OnAmmoChange;
        test = FindObjectOfType<Test>();
        MUC = FindObjectOfType<MainUiController>();
        shopPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TextWork();
    }
    private void TextWork()
    {
        FridgeText.text = fridgeCount.ToString();
        fridgeCount = fridge.freezeCount;
        AcidText.text = AcidRainCount.ToString();
        AcidRainCount = acidRain.AcidRainCount;
        TntText.text = tntCount.ToString();
        tntCount = test.tntCount;
        moneyText.text = money.ToString();
        money = test.cash;
    }
    private void OnAmmoChange()
    {
        ammoCounter.text = gunController.currentAmmo.ToString();
        genAmmoCounter.text = gunController.currentGunIndex == 0 ? "∞" : gunController.genAmmo.ToString();
    }
    public void OpenShop()
    {
        Time.timeScale = 0;
        shopPanels[0].SetActive(false);
        shopPanel.SetActive(true);
    }
    public void CloseShop()
    {
        Time.timeScale = 1;
        shopPanels[0].SetActive(false);
        shopPanel.SetActive(false);
    }
    public void SpeedUp()
    {
        if(test.cash >= 50)
        {
      
            Click.Play();
            test.speedUp = true;
            MUC.StartSpeedUp();
            test.cash -= 50;
        }
        else 
        {
            failed.Play();
        }
    }
    public void JumpUp()
    {
        if (test.cash >= 50)
        {
      
            Click.Play();
            test.buffJump = true;
            MUC.StartBuffJump();
            test.cash -= 50;
        }
        else 
        {
            failed.Play();
        }
    }
    public void Armor()
    {
        if (test.cash >= 70)
        {
       
            Click.Play();
            test.armor = true;
            MUC.StartArmor();
            test.cash -= 70;
        }
        else 
        {
            failed.Play();
        }
    }
    public void UnlockAssault()
    {
        if (test.cash >= 50)
        {
         
            Click.Play();
            gunController.ChangeGun(1);
            test.ass();
            test.cash -= 50;
        }
        else 
        {
            failed.Play();
        }
    }
    public void UnlockShotGun()
    {
        if (test.cash >= 70)
        {
         
            Click.Play();
            gunController.ChangeGun(3);
            test.ShotGun();
            test.cash -= 70;
        }
        else 
        {
            failed.Play();
        }
    }
    public void UnlockGran()
    {
        if (test.cash >= 100)
        {
      
            Click.Play();
            gunController.ChangeGun(2);
            test.gran();
            test.cash -= 100;
        }
        else 
        {
            failed.Play();
        }
    }
    public void UnlockCrossBow()
    {
        if (test.cash >= 50)
        {
       
            Click.Play();
            gunController.ChangeGun(5);
            test.Crossbow();
            test.cash -= 50;
        }
        else 
        {
            failed.Play();
        }
    }
    public void UnlockUzi()
    {
        if (test.cash >= 65)
        {
        
            Click.Play();
            gunController.ChangeGun(4);
            test.uzi();
            test.cash -= 65;
        }
        else 
        {
            failed.Play();
        }
    }
    public void UnlockLazer()
    {
        if (test.cash >= 85)
        {
          
            Click.Play();
            gunController.ChangeGun(6);
            test.laser();
            test.cash -= 85;
        }
        else 
        {
            failed.Play();
        }
    }
    public void TntBuy()
    {
        if(test.cash >= 65)
        {
           
            Click.Play();
            test.tntCount += 2;
            test.cash -= 65;
        }
        else 
        {
            failed.Play();
        }
    }
    public void Fridge()
    {
        if(test.cash >= 75)
        {
          
            Click.Play();
            fridge.freezeCount += 1;
            test.cash -= 75;
        }
        else 
        {
            failed.Play();
        }
    }
    public void AcidRain()
    {
        if (test.cash >= 85)
        {
            Click.Play();
            acidRain.AcidRainCount += 1;
            test.cash -= 85;
        }
        else
        {
            failed.Play();
        }
    }
    public void Next1()
    {
        shopPanels[0].SetActive(true);
       
    }
    public void Next2()
    {
        shopPanels[0].SetActive(false);
        shopPanel.SetActive(true);
    }
    
}
