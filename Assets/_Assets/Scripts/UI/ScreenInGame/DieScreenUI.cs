
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine.UI;

public class DieScreenUI : MonoBehaviour
{
    // TimeLineWatcher
    public static Action Reset;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI _textUIForDie;
    [SerializeField] TextMeshProUGUI _textCoin;
    [SerializeField] TextMeshProUGUI _textScore;
    [SerializeField] TextMeshProUGUI _textHighScore;
    [SerializeField] TextMeshProUGUI _textWave;
    [SerializeField] TextMeshProUGUI _textHighWave;
    [SerializeField] TextMeshProUGUI _textNewBestScore;
    [SerializeField] TextMeshProUGUI _textNewBestWave;

    [Header("Button")]
    [SerializeField] GameObject _buttonDie;
    [SerializeField] Button _showAds;

    [Header("CanvasGroup")]
    [SerializeField] CanvasGroup _canvasScreenUI_HUD;
    [SerializeField] CanvasGroup _canvasScreenUI_PAUSE;
    [SerializeField] CanvasGroup _canvasScreenEmpty;

    [Header("Component")]
    [SerializeField] GameStateManager _gameStateManager;
    [Header("Script")]
    [SerializeField] HUDController _hudController;


    private float _timeOffScreen = 0.3f;

    private static string _textDieForHealth = "ANOMALY DETECTED!";
    private static string _textDieForOutOfAmmor = "OUT OF AMMO!";
    private static string _textDieForPlayBad = "INDUCED COMA!";
    private static string _textNewBest = "NEW BEST:";
    private static string _textYourBest = "YOUR BEST:";

    
    private int _coin;
    private int _totalCoin;
    private int _score;
    private int _highScore = 0;
    private int _wave;
    private int _highWave = 0;

    private int _countDestroyAst;

    private SaveSystem _saveSystem;

    private void Awake()
    {
        _saveSystem = GetComponent<SaveSystem>();
        LoadData();

        _buttonDie.SetActive(true);
    }

    private void LoadData()
    {
        _totalCoin = PlayerPrefs.GetInt(DataPlayerPrefs.para_TOTALCOIN, 0);
        _highWave = PlayerPrefs.GetInt(DataPlayerPrefs.para_HIGHWAVE, 0);
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnRegisterEvents();
    }

    private void Start()
    {
        _showAds.onClick.AddListener(() => ShowAds());
    }

    private void RegisterEvents()
    {
        PlayerHealth.Die += HandleDieForHealth;
        PlayerHealth.Die += OnScreenDie;
        GunController.Die += HandleDieForOutOfAmmo;
        GunController.Die += OnScreenDie;
        SettingScreenUI.Die += HandleDieForPlayBad;
        SettingScreenUI.Die += OnScreenDieForPlayBad;
        WaveManager.GetWave += HandleGetWave;

        AdsManager.OnHandleX2Coin += HandleX2Coin;
    }

    private void UnRegisterEvents()
    {
        PlayerHealth.Die -= HandleDieForHealth;
        PlayerHealth.Die -= OnScreenDie;
        GunController.Die -= HandleDieForOutOfAmmo;
        GunController.Die -= OnScreenDie;
        SettingScreenUI.Die -= HandleDieForPlayBad;
        SettingScreenUI.Die -= OnScreenDieForPlayBad;
        WaveManager.GetWave -= HandleGetWave;

        AdsManager.OnHandleX2Coin -= HandleX2Coin;
    }

    private void HandleDieForHealth() => _textUIForDie.text = _textDieForHealth;
    private void HandleDieForOutOfAmmo() => _textUIForDie.text = _textDieForOutOfAmmor;
    private void HandleDieForPlayBad() => _textUIForDie.text = _textDieForPlayBad;
    private void HandleGetWave(int wave) => _wave = wave;

    private void OnScreenDie()
    {
        StartCoroutine(HandleDie());
    }

    private IEnumerator HandleDie()
    {
        InputManager.isInputLocked = true;

        // Loading Ads
        AdsManager.Instance.LoadRewardedAd();

        yield return StartCoroutine(Wait(4f));

        _buttonDie.SetActive(false);
        _gameStateManager.ActionDownButtonPauseON();
        GetDataFromHUD();
    }    

    private void OnScreenDieForPlayBad()
    {
        GetDataFromHUD();
        InputManager.isInputLocked = true;
    }

    private void GetDataFromHUD()
    {
        _coin = _hudController.GetCoin();
        _textCoin.text = _coin.ToString();


        int score = _hudController.GetScore();
        int highScore = _hudController.GetHighScore();
        _highScore = ShowText(_textScore, _textHighScore, _textNewBestScore, score, highScore);
        _highWave = ShowText(_textWave, _textHighWave, _textNewBestWave, _wave, _highWave);

        _countDestroyAst = _hudController.GetCountDestroyAst();

        SaveData(_highScore, _highWave);    
    }

    private int ShowText(TextMeshProUGUI textValue, TextMeshProUGUI textHighValue, TextMeshProUGUI textTitle, int currentValue, int highValue)
    {
        textTitle.text = _textYourBest;
        textValue.text = currentValue.ToString();
        textHighValue.text = highValue.ToString();
        if (currentValue > highValue)
        {
            highValue = currentValue;
            textTitle.text = _textNewBest;
            textHighValue.text = highValue.ToString();
        }
        return highValue;
    }


    public void ActionReset()
    {
        _canvasScreenUI_HUD.DOFade(0, _timeOffScreen).SetUpdate(true);
        _canvasScreenUI_PAUSE.DOFade(0, _timeOffScreen).SetUpdate(true);
        _canvasScreenEmpty.DOFade(0, _timeOffScreen).SetUpdate(true);
        Reset?.Invoke();

        _saveSystem.SaveDataForRespawn();
    }

    private void SaveData(int highScore, int hightWave)
    {
        PlayerPrefs.SetInt(DataPlayerPrefs.para_TOTALCOIN, _coin + _totalCoin);
        PlayerPrefs.SetInt(DataPlayerPrefs.para_HIGHSCORE, highScore);
        PlayerPrefs.SetInt(DataPlayerPrefs.para_HIGHWAVE, hightWave);
        PlayerPrefs.Save();

        _saveSystem.SaveDataForRespawn();
    }    

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }    

    private void ShowAds()
    {
        AdsManager.Instance.ShowRewardedAd();
    }    

    private void HandleX2Coin()
    {
        int coinX2 = _coin * 2;
        _textCoin.text = coinX2.ToString();
        PlayerPrefs.SetInt(DataPlayerPrefs.para_TOTALCOIN, coinX2 + _totalCoin);
        PlayerPrefs.Save();
    }    

}
