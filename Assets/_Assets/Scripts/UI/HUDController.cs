
using DG.Tweening;
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

    [Header("ButtonOpenSkill")]
    [SerializeField] Button _onSkillButton;
    public float openButtonRotationZ = 5f;
    public float closeButtonRotationZ = -5f;

    [Header("TextNumberSkill")]
    [SerializeField] List<TextMeshProUGUI> _textNumberSkills;
    [SerializeField] SkillController _skillController;

    [Header("Panel")]
    [SerializeField] RectTransform _panelRect;
    public float sizeOpen = 260f;
    public float sizeClose = 32.5f;
    public float duration = 1f;
    public float timeClose = 3f;
    private bool _isOpen = false;

    private static int HASH_ANI_HP = Animator.StringToHash("Hp");

    private int _countDestroyAst;
    private int _highScore;
    private int _score;
    private int _coin;

    private void OnEnable()
    {
        RegisterEvents();
        SetTextNumberSkill(_skillController.GetListNumberSkill());
    }

    private void OnDisable()
    {
        UnRegisterEvents();
    }

    public void SetTextNumberSkill(List<int> numberSkillList)
    {
        for(int i = 0; i < numberSkillList.Count; i++)
        {
            _textNumberSkills[i].text = numberSkillList[i].ToString();
        }
    }


    private void Start()
    {
        _highScore = PlayerPrefs.GetInt(DataPlayerPrefs.para_HIGHSCORE, 0);
        _highScoreText.text = _highScore.ToString();
        _onSkillButton.onClick.AddListener(() => ActionOpenSkill());
        _countDestroyAst = PlayerPrefs.GetInt(DataPlayerPrefs.para_COUNT_DESTROYASTEROID, 0);
    }


    public int GetScore() => _score;
    public int GetHighScore() => _highScore;
    public int GetCoin() => _coin;
    public int GetCountDestroyAst() => _countDestroyAst;
    
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

    private void HandleUpdateScore(int score, AsteroidType type)
    {  
        _score = score;
        _scoreText.text = score.ToString(); 

        if(_score > _highScore)
        {
            _highScore = score;
            _highScoreText.text = _highScore.ToString();
        }

        if (type != AsteroidType.AstNon) _countDestroyAst += 1;

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

   private void ActionOpenSkill()
   {
        _isOpen = !_isOpen;
        if(_isOpen)
        {
            _panelRect.DOSizeDelta(new Vector2(_panelRect.sizeDelta.x, sizeOpen), duration).SetEase(Ease.OutQuad);
            _onSkillButton.transform.DOLocalRotate(new Vector3(0f, 0f, closeButtonRotationZ), duration).SetEase(Ease.OutQuad);
        }       
        else
        {
            _panelRect.DOSizeDelta(new Vector2(_panelRect.sizeDelta.x, sizeClose), duration).SetEase(Ease.OutQuad);
            _onSkillButton.transform.DOLocalRotate(new Vector3(0f, 0f, openButtonRotationZ), duration).SetEase(Ease.OutQuad);
        }    

    }

}
