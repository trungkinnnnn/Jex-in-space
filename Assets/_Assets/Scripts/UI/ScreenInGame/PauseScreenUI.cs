using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseScreenUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _textCoin;
    [SerializeField] TextMeshProUGUI _textScore;

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnRegisterEvents();
    }

    private void RegisterEvents()
    {
        PlayerInventory.OnActionCoin += HandleUpdateCoin;
        PlayerInventory.OnActionScore += HandleUpdateScore;
    }

    private void UnRegisterEvents()
    {
        PlayerInventory.OnActionCoin -= HandleUpdateCoin;
        PlayerInventory.OnActionScore -= HandleUpdateScore; 
    }

    
    private void HandleUpdateCoin(int coin) => _textCoin.text = coin.ToString();
    private void HandleUpdateScore(int score) => _textScore.text = score.ToString();
}
