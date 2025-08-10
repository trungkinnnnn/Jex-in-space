using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int coinCount = 0;
    public void AddCoin(int amount)
    {
        coinCount += 1;
        Debug.Log("Coin : " + coinCount);
    }
}
