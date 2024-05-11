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
    public AudioSource click;
    public Toggle musicToggle;
    public Image gunImage;
    [SerializeField] private Sprite[] guns;
    private int playerCash;
    public TextMeshProUGUI playerCashText;
    public Text weaponCountText, weaponCostText, buyButtonText;
    int weaponID = 0;
    public int weaponCount;
    public GameObject buyButton;
    [SerializeField] private GameplaySettings gameplaySettings;
    AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private Image[] weaponLookButton;

    private const string MusicPrefKey = "MusicEnabled";

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        instance = this;
        playerCash = PlayerPrefs.GetInt("Cash");
        weaponCount = PlayerPrefs.GetInt("WeaponCount", 1);
        WeaponShopManager.PurchaseWeapon(0);
    }

    private void Start()
    {
        bool musicEnabled = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
        musicToggle.isOn = musicEnabled;
        SetMusicVolume(musicEnabled ? 1 : 0);
        playerCashText.text = playerCash.ToString();
        UpdateWeaponCount();
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
        playerCashText.text = playerCash.ToString();
        PlayerPrefs.SetInt("Cash", playerCash);
        PlayerPrefs.Save();
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

    private void UpdateWeaponCount()
    {
        weaponCountText.text = weaponCount.ToString() + "/6";
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
