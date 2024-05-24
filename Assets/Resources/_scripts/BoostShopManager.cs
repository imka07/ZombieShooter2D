using UnityEngine;
using UnityEngine.UI;

public class BoostShopManager : MonoBehaviour
{
    #region Heart Boost
    private const string PurchasedHeartKey = "PurchasedHeart";
    public static bool IsHeartPurchased(int index)
    {
        return PlayerPrefs.GetInt(PurchasedHeartKey + index, 0) == 1;
    }

    public static void PurchaseHeartBoost(int index)
    {
        PlayerPrefs.SetInt(PurchasedHeartKey + index, 1); // Сохраняем 1, чтобы отметить покупку
        PlayerPrefs.Save();
    }

    #endregion

    #region Grenade Boost
    private const string PurchaseGrenadeKey = "PurchasedGrenade";
    public static bool IsGrenadePurchased(int index)
    {
        return PlayerPrefs.GetInt(PurchaseGrenadeKey + index, 0) == 1;
    }

    public static void PurchaseGrenadeBoost(int index)
    {
        PlayerPrefs.SetInt(PurchaseGrenadeKey + index, 1); // Сохраняем 1, чтобы отметить покупку
        PlayerPrefs.Save();
    }

    #endregion

    #region Money Boost
    private const string PurchaseCashKey = "PurchasedCashBoost";
    public static bool IsCashBoostPurchased(int index)
    {
        return PlayerPrefs.GetInt(PurchaseCashKey + index, 0) == 1;
    }

    public static void PurchaseCashBoost(int index)
    {
        PlayerPrefs.SetInt(PurchaseCashKey + index, 1); // Сохраняем 1, чтобы отметить покупку
        PlayerPrefs.Save();
    }

    #endregion
}
