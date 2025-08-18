using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopModuleSceenUI : MonoBehaviour
{
    [Header("TextUI")]
    [SerializeField] TextMeshProUGUI _textTotalCoin;

    private int _totalCoin;
    private void Start()
    {
        _totalCoin = PlayerPrefs.GetInt(DataPlayerPrefs.para_TOTALCOIN);
        _textTotalCoin.text = _totalCoin.ToString();
    }

}

