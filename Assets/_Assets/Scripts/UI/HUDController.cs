
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HUDController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _coinText;
    [SerializeField] TextMeshProUGUI _hpText;
    [SerializeField] TextMeshProUGUI _currentBulletText;
    [SerializeField] TextMeshProUGUI _totalBulletText;

    [Header("HP")]
    [SerializeField] Image _hpSprite;
    [SerializeField] List<Sprite> _hpSpriteList;
    [SerializeField] Animator _hpAnimator;
    private static int HASH_ANI_HP = Animator.StringToHash("Hp");

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

    private void HandleUpdateScore(int score) => _scoreText.text = score.ToString(); 
    private void HandleUpdateCoin(int coin) => _coinText.text = coin.ToString();      
    private void HandleUpdateCurrentBullet(int mag) => _currentBulletText.text = mag.ToString();
    private void HandleUpdateTotalBullet(int totalBullet) => _totalBulletText.text = totalBullet.ToString();
    private void HandleUpdateHp(int hp)
    {
        _hpText.text = hp.ToString();
        _hpSprite.sprite = _hpSpriteList[hp];
        _hpAnimator.SetInteger(HASH_ANI_HP, hp);
    }

}
