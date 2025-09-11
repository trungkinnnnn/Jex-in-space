using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchasingManager : MonoBehaviour
{
    private static string ID_PUR_COIN = "coin";

    public PurchasingManager Instance;

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
            Debug.Log("Buy Coin");
        }    
    }    
}
