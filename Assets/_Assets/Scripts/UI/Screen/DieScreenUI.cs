using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DieScreenUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textUI;
    [SerializeField] GameStateManager _gameStateManager;

    private static string _textDieForHealth = "ANOMALY DETECTED!";
    private static string _textDieForOutOfAmmor = "OUT OF AMMO!";

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
        PlayerHealth.Die += handleDieForHealth;
        PlayerHealth.Die += OnScreenDie;
        GunController.Die += handleDieForOutOfAmmo;
        GunController.Die += OnScreenDie;
    }

    private void UnRegisterEvents()
    {
        PlayerHealth.Die -= handleDieForHealth;
        PlayerHealth.Die -= OnScreenDie;
        GunController.Die -= handleDieForOutOfAmmo;
        GunController.Die -= OnScreenDie;
    }

    private void handleDieForHealth() => _textUI.text = _textDieForHealth;
    private void handleDieForOutOfAmmo() => _textUI.text = _textDieForOutOfAmmor;

    private void OnScreenDie() => _gameStateManager.ActionDownButtonPauseON();
}
