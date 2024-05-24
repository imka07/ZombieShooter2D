using UnityEngine;

public class WeaponShopManager : MonoBehaviour
{
    private const string PurchasedWeaponsKey = "PurchasedWeapons";

    public static bool IsWeaponPurchased(int weaponID)
    {
        return PlayerPrefs.GetInt(PurchasedWeaponsKey + weaponID, 0) == 1;
    }

    // Покупка оружия
    public static void PurchaseWeapon(int weaponID)
    {
        PlayerPrefs.SetInt(PurchasedWeaponsKey + weaponID, 1);
        PlayerPrefs.Save();
    }
}
