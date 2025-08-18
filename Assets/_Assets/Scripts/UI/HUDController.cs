
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("TextUI")]
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _highScoreText;
    [SerializeField] TextMeshProUGUI _coinText;
    [SerializeField] TextMeshProUGUI _hpText;
    [SerializeField] TextMeshProUGUI _currentBulletText;
    [SerializeField] TextMeshProUGUI _totalBulletText;

    [Header("HP")]
    [SerializeField] Image _hpSprite;
    [SerializeField] List<Sprite> _hpSpriteList;
    [SerializeField] Animator _hpAnimator;
    private static int HASH_ANI_HP = Animator.StringToHash("Hp");

    private int _highScore;
    private int _score;
    private int _coin;


    private void Start()
    {
        _highScore = PlayerPrefs.GetInt(DataPlayerPrefs.para_HIGHSCORE, 0);
        _highScoreText.text = _highScore.ToString();
    }


    public int GetScore() => _score;
    public int GetHighScore() => _highScore;
    public int GetCoin() => _coin;

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
        PlayerHealth.OnActionHp += HandleUpdateHp;
        GunController.OnActionCurrentBullet += HandleUpdateCurrentBullet;
        GunController.OnActionTotalBullet += HandleUpdateTotalBullet;
    }

    private void UnRegisterEvents()
    {
        PlayerInventory.OnActionCoin -= HandleUpdateCoin;
        PlayerInventory.OnActionScore -= HandleUpdateScore;
        PlayerHealth.OnActionHp -= HandleUpdateHp;
        GunController.OnActionCurrentBullet -= HandleUpdateCurrentBullet;
        GunController.OnActionTotalBullet -= HandleUpdateTotalBullet;
    }

    private void HandleUpdateScore(int score)
    {  
        _score = score;
        _scoreText.text = score.ToString(); 

        if(_score > _highScore)
        {
            _highScore = score;
            _highScoreText.text = _highScore.ToString();
        }

    }
    private void HandleUpdateCoin(int coin)  
    { 
        _coin = coin;
        _coinText.text = coin.ToString();      
    }
    private void HandleUpdateCurrentBullet(int mag) => _currentBulletText.text = mag.ToString();
    private void HandleUpdateTotalBullet(int totalBullet) => _totalBulletText.text = totalBullet.ToString();
    private void HandleUpdateHp(int hp)
    {
        _hpText.text = hp.ToString();
        _hpSprite.sprite = _hpSpriteList[hp];
        _hpAnimator.SetInteger(HASH_ANI_HP, hp);
    }

   

}
