using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System.ComponentModel;
using System.Threading;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;


    [Header("Player Cash")]
    private float playerCash;
    public TextMeshProUGUI playerCashText;

    [Header("Heart Boost Components")]
    public int heartIndex = 0;
    public Image[] heartBoostPanels;
    int HeartPrice = 500;
    public Text heartPriceText;

    [Header("Grenade Boost Components")]
    public int grenadeIndex = 0;
    public Image[] grenadeBoostPanels;
    int GrenadePrice = 500;
    public Text grenadePriceText;

    [Header("Cash Boost Components")]
    public int cashIndex = 0;
    public Image[] cashBoostPanels;
    int CashPrice = 500;
    public Text cashPriceText;


    [Header("Weapon Components")]
    [SerializeField] private Image[] weaponLookButton;
    [SerializeField] private GameplaySettings gameplaySettings;
    public Text weaponCountText, weaponCostText, buyButtonText, boostCountText;
    public Text heartButtonText, grenadeButtonText, cashButtonText;
    int weaponID = 0;
    public int weaponCount, boostCount;
    public GameObject buyButton;
    public Image gunImage;
    [SerializeField] private Sprite[] guns;
   
    

    [Header("Audio")]
    private const string MusicPrefKey = "MusicEnabled";
    AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    public AudioSource click;
    public Toggle musicToggle;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        instance = this;
        StartData();
        WeaponShopManager.PurchaseWeapon(0);
    }

 

    private void StartData()
    {
        // cash
        playerCash = PlayerPrefs.GetFloat("Cash");
        // weaponID
        weaponCount = PlayerPrefs.GetInt("WeaponCount", 1);
        // boostCount
        boostCount = PlayerPrefs.GetInt("BoostCount", 0);
        // heartBoost
        heartIndex = PlayerPrefs.GetInt("HeartCount", 0);
        // grenadeBoost
        grenadeIndex = PlayerPrefs.GetInt("GrenadeCount", 0);
        // cashBoost
        cashIndex = PlayerPrefs.GetInt("CashCount", 0);
        // boostPrices
        HeartPrice = PlayerPrefs.GetInt("HeartPrice", 500);
        GrenadePrice = PlayerPrefs.GetInt("GrenadePrice", 500);
        CashPrice = PlayerPrefs.GetInt("CashPrice", 500);
        // music
        bool musicEnabled = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
        musicToggle.isOn = musicEnabled;
        SetMusicVolume(musicEnabled ? 1 : 0);

        // Инициализация панелей бустов
        for (int i = 0; i < heartBoostPanels.Length; i++)
        {
            if (BoostShopManager.IsHeartPurchased(i))
            {
                heartBoostPanels[i].color = new Color32(137, 221, 187, 255);
            }
        }
        for (int i = 0; i < grenadeBoostPanels.Length; i++)
        {
            if (BoostShopManager.IsGrenadePurchased(i))
            {
                grenadeBoostPanels[i].color = new Color32(137, 221, 187, 255);
            }
        }
        for (int i = 0; i < cashBoostPanels.Length; i++)
        {
            if (BoostShopManager.IsCashBoostPurchased(i))
            {
                cashBoostPanels[i].color = new Color32(137, 221, 187, 255);
            }
        }
    }


    private void Start()
    {
        UpdateBoosts();
        UpdateCashAmmount();
        UpdateWeaponCount();
        UpdateBoostCount();
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < gameplaySettings.weaponSettings.weapons.Count; i++)
        {
            if (WeaponShopManager.IsWeaponPurchased(i))
            {
                weaponLookButton[i].color = new Color32(145, 253, 143, 255);
            }
        }

    }

    private void Update()
    {
        ToggleMusic();
        BoostButtons();
    }

    private void BoostButtons()
    {
        if (playerCash < HeartPrice)
        {
            heartButtonText.text = "Недостаточно";
            heartButtonText.color = new Color32(217, 79, 64, 255);
        }
        else
        {
            heartButtonText.text = "Купить";
            heartButtonText.color = new Color32(58, 115, 93, 255);
        }

        if (playerCash < GrenadePrice)
        {
            grenadeButtonText.text = "Недостаточно";
            grenadeButtonText.color = new Color32(217, 79, 64, 255);
        }
        else
        {
            grenadeButtonText.text = "Купить";
            grenadeButtonText.color = new Color32(58, 115, 93, 255);
        }
        if (playerCash < CashPrice)
        {
            cashButtonText.text = "Недостаточно";
            cashButtonText.color = new Color32(217, 79, 64, 255);
        }
        else
        {
            heartButtonText.text = "Купить";
            heartButtonText.color = new Color32(58, 115, 93, 255);
        }

    }

    public void ChangeIndex(int index)
    {
        weaponID = index;
        GunDemonstration();
        weaponCostText.text = gameplaySettings.weaponSettings.weapons[index].weaponPrice.ToString();
        if (WeaponShopManager.IsWeaponPurchased(index))
        {
            buyButton.SetActive(false);
        }
        else
        {
            buyButton.SetActive(true);
        }

        if (playerCash < gameplaySettings.weaponSettings.weapons[weaponID].weaponPrice)
        {
            buyButtonText.text = "Недостаточно";
            buyButtonText.color = new Color32(217, 79, 64, 255);
        }
        else
        {
            buyButtonText.text = "Купить";
            buyButtonText.color = new Color32(58, 115, 93, 255);
        }
    }

    public void TakeCash(int amount)
    {
        playerCash -= amount;
        UpdateCashAmmount();
        PlayerPrefs.SetFloat("Cash", playerCash);
        PlayerPrefs.Save();
    }

    private void SaveBoostPrices()
    {
        PlayerPrefs.SetInt("HeartPrice", HeartPrice);
        PlayerPrefs.SetInt("GrenadePrice", GrenadePrice);
        PlayerPrefs.SetInt("CashPrice", CashPrice);
    }

    private void GunDemonstration()
    {
        gunImage.sprite = guns[weaponID];
    }

    public void BuyWeapon()
    {
        if (playerCash >= gameplaySettings.weaponSettings.weapons[weaponID].weaponPrice)
        {
            weaponCount++;
            WeaponShopManager.PurchaseWeapon(weaponID);
            PlayerPrefs.SetInt("WeaponCount", weaponCount);
            UpdateWeaponCount();
            buyButton.SetActive(false);
            TakeCash(gameplaySettings.weaponSettings.weapons[weaponID].weaponPrice);
            audioSource.clip = clips[0];
            audioSource.Play();
            weaponLookButton[weaponID].color = new Color32(145, 253, 143, 255);
        }
    }

    public void BuyHeartBoost()
    {
        if (playerCash >= HeartPrice)
        {
            boostCount++;
            PlayerPrefs.SetInt("BoostCount", boostCount);
            BoostShopManager.PurchaseHeartBoost(heartIndex);
            heartIndex++;
            heartIndex = Mathf.Clamp(heartIndex, 0, heartBoostPanels.Length);
            PlayerPrefs.SetInt("HeartCount", heartIndex);
            UpdateBoostCount();
            for (int i = 0; i < heartIndex; i++)
            {
                heartBoostPanels[i].color = new Color32(137, 221, 187, 255);
            }
            TakeCash(HeartPrice);
            HeartPrice *= 2;
            UpdateBoosts();
            audioSource.clip = clips[0];
            audioSource.Play();
        }
    }

    public void BuyGrenadeBoost()
    {
        if (playerCash >= GrenadePrice)
        {
            boostCount++;
            PlayerPrefs.SetInt("BoostCount", boostCount);
            BoostShopManager.PurchaseGrenadeBoost(grenadeIndex);
            grenadeIndex++;
            grenadeIndex = Mathf.Clamp(grenadeIndex, 0, grenadeBoostPanels.Length);
            PlayerPrefs.SetInt("GrenadeCount", grenadeIndex);
            UpdateBoostCount();
            for (int i = 0; i < grenadeIndex; i++)
            {
                grenadeBoostPanels[i].color = new Color32(137, 221, 187, 255);
            }
            TakeCash(GrenadePrice);
            GrenadePrice *= 2;
            UpdateBoosts();
            audioSource.clip = clips[0];
            audioSource.Play();
        }

    }

    public void BuyCashBoost()
    {
        if (playerCash >= CashPrice)
        {
            boostCount++;
            PlayerPrefs.SetInt("BoostCount", boostCount);
            BoostShopManager.PurchaseCashBoost(cashIndex);
            cashIndex++;
            cashIndex = Mathf.Clamp(cashIndex, 0, cashBoostPanels.Length);
            PlayerPrefs.SetInt("CashCount", cashIndex);
            UpdateBoostCount();
            for (int i = 0; i < cashIndex; i++)
            {
                cashBoostPanels[i].color = new Color32(137, 221, 187, 255);
            }
            TakeCash(CashPrice);
            CashPrice *= 2;
            UpdateBoosts();
            audioSource.clip = clips[0];
            audioSource.Play();
        }

    }


    private void UpdateWeaponCount()
    {
        weaponCountText.text = weaponCount.ToString() + "/6";
    }
    private void UpdateBoostCount()
    {
        boostCountText.text = boostCount.ToString() + "/6";
    }
    private void UpdateCashAmmount()
    {
        playerCashText.text = playerCash.ToString();
    }
    private void UpdateBoosts()
    {
        heartPriceText.text = HeartPrice.ToString();
        grenadePriceText.text = GrenadePrice.ToString();
        cashPriceText.text = CashPrice.ToString();
        SaveBoostPrices();
    }

    private void ToggleMusic()
    {
        bool musicEnabled = musicToggle.isOn;
        SetMusicVolume(musicEnabled ? 1 : 0);

        PlayerPrefs.SetInt(MusicPrefKey, musicEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetMusicVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void ToGame(int scene)
    {
        click.Play();
        SceneManager.LoadScene(scene);
    }

}
