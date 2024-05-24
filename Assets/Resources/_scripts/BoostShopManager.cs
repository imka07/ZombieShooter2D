using UnityEngine;

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
        PlayerPrefs.SetInt(PurchasedHeartKey + index, 1);
        PlayerPrefs.Save();
    }
    #endregion
}
