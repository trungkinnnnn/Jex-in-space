
using TMPro;
using UnityEngine;

public class DieScreenUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textUIForDie;
    [SerializeField] TextMeshProUGUI _textCoin;
    [SerializeField] TextMeshProUGUI _textScore;
    [SerializeField] TextMeshProUGUI _textHighScore;
    [SerializeField] TextMeshProUGUI _textWave;
    [SerializeField] TextMeshProUGUI _textHighWave;
    [SerializeField] TextMeshProUGUI _textNewBestScore;
    [SerializeField] TextMeshProUGUI _textNewBestWave;
    [SerializeField] GameStateManager _gameStateManager;

    private static string _textDieForHealth = "ANOMALY DETECTED!";
    private static string _textDieForOutOfAmmor = "OUT OF AMMO!";
    private static string _textNewBest = "NEW BEST:";
    private static string _textYourBest = "YOUR BEST:";

    private HUDController _hudController;
    private int _coin;
    private int _totalCoin;
    private int _score;
    private int _highScore = 0;
    private int _wave;
    private int _highWave = 0;

    private SaveSystem _saveSystem;

    private void Awake()
    {
        _hudController = GetComponent<HUDController>();
        _saveSystem = GetComponent<SaveSystem>();
        LoadData();
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

    private void RegisterEvents()
    {
        PlayerHealth.Die += HandleDieForHealth;
        PlayerHealth.Die += OnScreenDie;
        GunController.Die += HandleDieForOutOfAmmo;
        GunController.Die += OnScreenDie;
        WaveManager.GetWave += HandleGetWave;
    }

    private void UnRegisterEvents()
    {
        PlayerHealth.Die -= HandleDieForHealth;
        PlayerHealth.Die -= OnScreenDie;
        GunController.Die -= HandleDieForOutOfAmmo;
        GunController.Die -= OnScreenDie;
        WaveManager.GetWave -= HandleGetWave;
    }

    private void HandleDieForHealth() => _textUIForDie.text = _textDieForHealth;
    private void HandleDieForOutOfAmmo() => _textUIForDie.text = _textDieForOutOfAmmor;

    private void HandleGetWave(int wave) => _wave = wave;   

    private void OnScreenDie()  
    { 
        _gameStateManager.ActionDownButtonPauseON();
        GetDataFromHUD();

    }

    private void GetDataFromHUD()
    {
        _coin = _hudController.GetCoin();
        _textCoin.text = _coin.ToString();


        int score = _hudController.GetScore();
        int highScore = _hudController.GetHighScore();
        _highScore = ShowText(_textScore, _textHighScore, _textNewBestScore, score, highScore);
        _highWave = ShowText(_textWave, _textHighWave, _textNewBestWave, _wave, _highWave);

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

    private void SaveData(int highScore, int hightWave)
    {
        PlayerPrefs.SetInt(DataPlayerPrefs.para_TOTALCOIN, _coin + _totalCoin);
        PlayerPrefs.SetInt(DataPlayerPrefs.para_HIGHSCORE, highScore);
        PlayerPrefs.SetInt(DataPlayerPrefs.para_HIGHWAVE, hightWave);
        PlayerPrefs.Save();

        _saveSystem.SaveData();
    }    

    
}
