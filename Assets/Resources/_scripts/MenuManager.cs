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
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.Rendering;
//using YG;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [SerializeField] private Button[] levelButtons;
    //public YandexGame sdk;

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
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource music;

    public AudioSource click;
    public Toggle musicToggle;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();

        instance = this;
        WeaponShopManager.PurchaseWeapon(0);
        StartData();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int level = i + 1;
            if (PlayerPrefs.GetInt("Level_" + level, level == 1 ? 1 : 0) == 1) // Default to unlocked if it's the first level
            {
                levelButtons[i].interactable = true;
                levelButtons[i].onClick.AddListener(() => ToGame(level));
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
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
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }


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

    //private void OnEnable() => YandexGame.CloseVideoEvent += Rewarded;
    //private void OnDisable() => YandexGame.CloseVideoEvent -= Rewarded;

    public void Rewarded(int id)
    {
        if (id == 0)
        {
            BonusForAd();
        }
    }

    public void BonusForAd()
    {
        playerCash += 300;
        UpdateCashAmmount();
        PlayerPrefs.SetFloat("Cash", playerCash);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        ToggleMusic();
        BoostButtons();
    }

    private void BoostButtons()
    {
        UpdateButton(heartButtonText, playerCash > HeartPrice);
        UpdateButton(grenadeButtonText, playerCash > GrenadePrice);
        UpdateButton(cashButtonText, playerCash >= CashPrice);
    }

    private void UpdateButton(Text buttonText, bool canAfford)
    {
        if (canAfford)
        {
            buttonText.text = "Купить";
            buttonText.color = new Color32(58, 115, 93, 255);
        }
        else
        {
            buttonText.color = new Color32(217, 79, 64, 255);
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
        UpdateButton(buyButtonText, playerCash > gameplaySettings.weaponSettings.weapons[weaponID].weaponPrice);
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
        if (playerCash < HeartPrice || heartIndex >= heartBoostPanels.Length) return;

        boostCount++;
        PlayerPrefs.SetInt("BoostCount", boostCount);

        BoostShopManager.PurchaseHeartBoost(heartIndex);

        heartIndex = Mathf.Clamp(++heartIndex, 0, heartBoostPanels.Length);
        PlayerPrefs.SetInt("HeartCount", heartIndex);

        UpdateBoostCount();
        UpdateHeartBoostPanels();

        TakeCash(HeartPrice);
        HeartPrice *= 2;

        UpdateBoosts();
        PlayPurchaseSound();
    }

    private void UpdateHeartBoostPanels()
    {
        Color32 activeColor = new Color32(137, 221, 187, 255);
        for (int i = 0; i < heartIndex; i++)
        {
            heartBoostPanels[i].color = activeColor;
        }
    }

    private void PlayPurchaseSound()
    {
        audioSource.clip = clips[0];
        audioSource.Play();
    }


    public void BuyGrenadeBoost()
    {
        if (playerCash < GrenadePrice || grenadeIndex >= grenadeBoostPanels.Length) return;

        boostCount++;
        PlayerPrefs.SetInt("BoostCount", boostCount);

        BoostShopManager.PurchaseGrenadeBoost(grenadeIndex);

        grenadeIndex = Mathf.Clamp(++grenadeIndex, 0, grenadeBoostPanels.Length);
        PlayerPrefs.SetInt("GrenadeCount", grenadeIndex);

        UpdateBoostCount();
        UpdateGrenadeBoostPanels();

        TakeCash(GrenadePrice);
        GrenadePrice *= 2;

        UpdateBoosts();
        PlayPurchaseSound();
    }

    private void UpdateGrenadeBoostPanels()
    {
        Color32 activeColor = new Color32(137, 221, 187, 255);
        for (int i = 0; i < grenadeIndex; i++)
        {
            grenadeBoostPanels[i].color = activeColor;
        }
    }


    public void PlayGame()
    {
        // Получить номер последнего открытого уровня, по умолчанию 1 (первый уровень)
        int lastOpenedLevel = PlayerPrefs.GetInt("LastOpenedLevel", 1);

        // Загрузить последний открытый уровень
        SceneManager.LoadScene("Level" + lastOpenedLevel);
    }

    public void BuyCashBoost()
    {
        if (playerCash < CashPrice || cashIndex >= cashBoostPanels.Length) return;

        boostCount++;
        PlayerPrefs.SetInt("BoostCount", boostCount);

        BoostShopManager.PurchaseCashBoost(cashIndex);

        cashIndex = Mathf.Clamp(++cashIndex, 0, cashBoostPanels.Length);
        PlayerPrefs.SetInt("CashCount", cashIndex);

        UpdateBoostCount();
        UpdateCashBoostPanels();

        TakeCash(CashPrice);
        CashPrice *= 2;

        UpdateBoosts();
        PlayPurchaseSound();
    }

    private void UpdateCashBoostPanels()
    {
        Color32 activeColor = new Color32(137, 221, 187, 255);
        for (int i = 0; i < cashIndex; i++)
        {
            cashBoostPanels[i].color = activeColor;
        }
    }


    private void UpdateWeaponCount()
    {
        weaponCountText.text = weaponCount.ToString() + "/6";
    }
    private void UpdateBoostCount()
    {
        boostCountText.text = boostCount.ToString() + "/12";
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

    public void ChangeVolume()
    {
        music.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    private void SetMusicVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void ToGame(int level)
    {
        click.Play();
        SceneManager.LoadScene("Level" + level);
    }

}
