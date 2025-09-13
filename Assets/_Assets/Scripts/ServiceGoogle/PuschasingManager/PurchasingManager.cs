using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchasingManager : MonoBehaviour
{
    [SerializeField] ShopGunScreenUI _shopGunScreenUI;
    [SerializeField] ShopModuleSceenUI _shopModuleSceenUI;

    private static string ID_PUR_COIN = "coin";
    private static int _coinPurchasing = 50000;
    public static PurchasingManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }    
    }

    public void PurchasingProduct(Product product)
    {
        if(product.definition.id == ID_PUR_COIN)
        {
            PurchasingCompleted();
        }    
    }  
    
    public void PurchasingCompleted()
    {
        int totalCoin = PlayerPrefs.GetInt(DataPlayerPrefs.para_TOTALCOIN, 0);
        PlayerPrefs.SetInt(DataPlayerPrefs.para_TOTALCOIN, totalCoin + _coinPurchasing);
        PlayerPrefs.Save();

        ResetUI(totalCoin + _coinPurchasing);
    }

    public void ResetUI(int totalCoin)
    {
        _shopGunScreenUI.SetTextCoin(totalCoin);
        _shopModuleSceenUI.SetTextCoin(totalCoin);
    }
}
